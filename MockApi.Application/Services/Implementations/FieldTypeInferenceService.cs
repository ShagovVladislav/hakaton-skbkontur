using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using MockApi.Application.Dto;
using MockApi.Application.Services.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Services.Implementations;

public class FieldTypeInferenceService(HttpClient http, IConfiguration configuration) : IFieldTypeInferenceService
{
    private readonly string _apiKey = configuration["OPENROUTER_API_KEY"]
                                      ?? throw new InvalidOperationException("OpenRouter API Key is missing.");

    private const string OpenRouterUrl = "https://openrouter.ai/api/v1/chat/completions";

    public async Task<Dictionary<string, FieldConfig>> InferAndFillMissingTypesAsync(Dictionary<string, FieldConfig> fields)
    {
        var keysToInfer = GetEmptyKeysRecursive(fields);
        if (keysToInfer.Count == 0) return fields;

        var allowedTypes = string.Join(", ", Enum.GetNames<FieldTypeEnum>());

        var requestBody = new
        {
            model = "google/gemini-2.0-flash-001",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = $"Return ONLY JSON. Use ONLY these types: [{allowedTypes}]. " +
                              "Predict the most suitable data type for each key. " +
                              "Example: 'email' -> 'String', 'age' -> 'Integer', 'isValid' -> 'Boolean'."
                },
                new { role = "user", content = JsonSerializer.Serialize(keysToInfer) }
            },
            response_format = new { type = "json_object" },
            temperature = 0,
            max_tokens = 500
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, OpenRouterUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        request.Headers.Add("HTTP-Referer", "http://localhost:5255");
        request.Headers.Add("X-Title", "MockApi.Development");
        request.Content = JsonContent.Create(requestBody);

        var response = await http.SendAsync(request);
        if (!response.IsSuccessStatusCode) return fields;

        var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
        var content = jsonResponse.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

        if (string.IsNullOrWhiteSpace(content)) return fields;

        content = Regex.Replace(content, "```[a-z]*|```", "").Trim();

        var predictions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        try
        {
            var raw = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
            if (raw != null) predictions = raw;
        }
        catch
        {
            // ignored
        }

        ProcessFieldsRecursive(fields, predictions);
        return fields;
    }

    public async Task<string> GenerateMockDataFromDescriptionAsync(string description)
    {
        var requestBody = new
        {
            model = "google/gemini-2.0-flash-001",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = "You are a specialized mock data generator. " +
                              "Return ONLY raw JSON object. NO markdown, NO text. " +
                              "Use realistic data. ONLY RUSSIAN LANGUAGE."
                },
                new { role = "user", content = description }
            },
            response_format = new { type = "json_object" },
            temperature = 0,
            max_tokens = 500
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, OpenRouterUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        request.Headers.Add("HTTP-Referer", "http://localhost:5255");
        request.Headers.Add("X-Title", "MockApi.Development");
        request.Content = JsonContent.Create(requestBody);

        var response = await http.SendAsync(request);
        if (!response.IsSuccessStatusCode) return $"{{\"error\": \"AI provider error\"}}";

        var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
        var content = jsonResponse.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

        return string.IsNullOrWhiteSpace(content) ? "{}" : Regex.Replace(content, "```[a-z]*|```", "").Trim();
    }

    private List<string> GetEmptyKeysRecursive(Dictionary<string, FieldConfig> fields)
    {
        var keys = new List<string>();
        foreach (var (key, config) in fields)
        {
            if (config.Type == null && (config.Properties == null || config.Properties.Count == 0))
            {
                keys.Add(key);
            }
            if (config.Properties != null && config.Properties.Count > 0)
            {
                keys.AddRange(GetEmptyKeysRecursive(config.Properties));
            }
            if (config.Type == FieldTypeEnum.Array && config.Items != null)
            {
                var subDict = new Dictionary<string, FieldConfig> { { $"{key}_item", config.Items } };
                keys.AddRange(GetEmptyKeysRecursive(subDict));
            }
        }
        return keys;
    }

    private void ProcessFieldsRecursive(Dictionary<string, FieldConfig> fields, Dictionary<string, string> predictions)
    {
        foreach (var (key, config) in fields)
        {
            if (config.Type == null && (config.Properties == null || config.Properties.Count == 0))
            {
                if (predictions.TryGetValue(key, out var typeStr) && 
                    Enum.TryParse<FieldTypeEnum>(typeStr, true, out var resultType))
                {
                    config.Type = resultType;
                }
                else
                {
                    config.Type = FieldTypeEnum.String;
                }
            }

            if (config.Properties != null && config.Properties.Count > 0)
            {
                ProcessFieldsRecursive(config.Properties, predictions);
            }

            if (config.Type == FieldTypeEnum.Array && config.Items != null)
            {
                var subDict = new Dictionary<string, FieldConfig> { { $"{key}_item", config.Items } };
                ProcessFieldsRecursive(subDict, predictions);
            }
        }
    }
}
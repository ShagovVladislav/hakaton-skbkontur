using System.Net.Http.Json;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using MockApi.Application.Services.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Services.Implementations;

public class FieldTypeInferenceService(HttpClient http, IConfiguration configuration) : IFieldTypeInferenceService
{
    private readonly string _apiKey = configuration["OPENROUTER_API_KEY"]
                                      ?? throw new InvalidOperationException("OpenRouter API Key is missing.");

    private const string OpenRouterUrl = "https://openrouter.ai/api/v1/chat/completions";

    public async Task<Dictionary<string, object>> InferAndFillMissingTypesAsync(Dictionary<string, object?> fields)
    {
        var keysToInfer = GetEmptyKeysRecursive(fields);
        if (keysToInfer.Count == 0) return fields!;

        var allowedTypes = string.Join(",", Enum.GetNames<FieldTypeEnum>());

        var requestBody = new
        {
            model = "google/gemini-2.0-flash-001",
            messages = new[]
            {
                new
                {
                    role = "system",
                    content = $"Return ONLY JSON. Use ONLY these types: [{allowedTypes}]. " +
                              "NEVER return the key name as a value. " +
                              "Example: if key is 'email', return 'String'. If key is 'age', return 'Int'."
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
        if (!response.IsSuccessStatusCode) return fields!;

        var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
        var content = jsonResponse.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

        if (string.IsNullOrWhiteSpace(content)) return fields!;

        content = Regex.Replace(content, "```[a-z]*|```", "").Trim();

        var predictions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        try
        {
            var raw = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(content);
            if (raw != null)
                foreach (var p in raw)
                    predictions[p.Key] = p.Value.ToString();
        }
        catch
        {
            // ignored
        }

        return ProcessFieldsRecursive(fields, predictions);
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
                              "Your task: Return ONLY a raw JSON object based on the user's description. " +
                              "DO NOT include any explanations, markdown code blocks (```json), or text outside the JSON. " +
                              "Use realistic data for values. ONLY RUSSIAN LANGUAGE"
                },
                new { role = "user", content = description }
            },
            response_format = new { type = "json_object" },
            temperature = 0.7,
            max_tokens = 1500
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, OpenRouterUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        request.Headers.Add("HTTP-Referer", "http://localhost:5255");
        request.Headers.Add("X-Title", "MockApi.Development");
        request.Content = JsonContent.Create(requestBody);

        var response = await http.SendAsync(request);
    
        if (!response.IsSuccessStatusCode)
            return $"{{\"error\": \"AI provider returned {response.StatusCode}\"}}";

        var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>();
    
        // Извлекаем строку контента из ответа OpenRouter
        var content = jsonResponse
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        if (string.IsNullOrWhiteSpace(content)) 
            return "{}";

        // На всякий случай чистим от возможных markdown-тегов, если модель их добавила
        content = Regex.Replace(content, "```[a-z]*|```", "").Trim();

        return content;
    }

    private List<string> GetEmptyKeysRecursive(Dictionary<string, object?> fields)
    {
        var keys = new List<string>();
        foreach (var (key, value) in fields)
        {
            if (IsValueEmpty(value)) keys.Add(key);
            else
                switch (value)
                {
                    case JsonElement { ValueKind: JsonValueKind.Object } el:
                        keys.AddRange(
                            GetEmptyKeysRecursive(
                                JsonSerializer.Deserialize<Dictionary<string, object?>>(el.GetRawText())!));
                        break;
                    case Dictionary<string, object?> sub:
                        keys.AddRange(GetEmptyKeysRecursive(sub));
                        break;
                }
        }

        return keys;
    }

    private Dictionary<string, object> ProcessFieldsRecursive(Dictionary<string, object?> fields,
        Dictionary<string, string> predictions)
    {
        var result = new Dictionary<string, object>();
        var validTypes = Enum.GetNames<FieldTypeEnum>().ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var (key, value) in fields)
        {
            switch (value)
            {
                case JsonElement { ValueKind: JsonValueKind.Object } el:
                {
                    var sub = JsonSerializer.Deserialize<Dictionary<string, object?>>(el.GetRawText())!;
                    result[key] = ProcessFieldsRecursive(sub, predictions);
                    break;
                }
                case Dictionary<string, object?> subDict:
                    result[key] = ProcessFieldsRecursive(subDict, predictions);
                    break;
                default:
                {
                    if (IsValueEmpty(value))
                    {
                        if (predictions.TryGetValue(key, out var type) && validTypes.Contains(type))
                            result[key] = type;
                        else
                            result[key] = "String";
                    }
                    else result[key] = value ?? "String";

                    break;
                }
            }
        }

        return result;
    }

    private bool IsValueEmpty(object? value)
    {
        if (value == null) return true;
        var s = value.ToString();
        return string.IsNullOrWhiteSpace(s) || s == "empty" || (value is JsonElement e &&
                                                                e.ValueKind == JsonValueKind.String &&
                                                                string.IsNullOrWhiteSpace(e.GetString()));
    }
}
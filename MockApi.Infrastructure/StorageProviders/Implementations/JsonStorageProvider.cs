using System.Text.Json;
using MockApi.Infrastructure.StorageProviders.Abstractions;

namespace MockApi.Infrastructure.StorageProviders.Implementations;

public class JsonStorageProvider : IStorageProvider
{
    private readonly string _jsonFilePath;

    public JsonStorageProvider(string jsonFilePath = "generatorValues.json")
    {
        _jsonFilePath = jsonFilePath;
    }

    public string[] GetValues(string section)
    {
        if (!File.Exists(_jsonFilePath))
            return [];

        var json = File.ReadAllText(_jsonFilePath);

        if (string.IsNullOrWhiteSpace(json))
            return [];

        using var document = JsonDocument.Parse(json);

        if (!document.RootElement.TryGetProperty(section, out var sectionElement) ||
            sectionElement.ValueKind != JsonValueKind.Array)
            return [];

        return (from item in sectionElement.EnumerateArray()
            where item.ValueKind == JsonValueKind.String
            select item.GetString()!).ToArray();
    }
}
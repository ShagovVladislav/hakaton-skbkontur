using MockApi.Application.Services.Abstractions;
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;
using System.Text.Json;

namespace MockApi.Application.Services.Implementations;

public class MockService : IMockService
{
    private readonly Dictionary<FieldTypeEnum, IGenerationalValue> _generatorsCache;

    public MockService(IEnumerable<IGenerationalValue> generators)
    {
        var generatorsList = generators.ToList();
        
        _generatorsCache = Enum.GetValues<FieldTypeEnum>()
            .Select(type => new { Type = type, Generator = generatorsList.FirstOrDefault(g => g.CanHandle(type)) })
            .Where(x => x.Generator != null)
            .ToDictionary(x => x.Type, x => x.Generator!);
    }

    public Dictionary<string, object> GenerateMockData(Dictionary<string, object> schema)
    {
        var result = new Dictionary<string, object>(schema.Count);

        foreach (var field in schema)
        {
            result[field.Key] = field.Value switch
            {
                Dictionary<string, object> nestedDict => GenerateMockData(nestedDict),

                JsonElement { ValueKind: JsonValueKind.Object } element => 
                    ProcessJsonObject(element),

                JsonElement { ValueKind: JsonValueKind.String } element => 
                    ProcessSimpleField(element.GetString()),

                string typeString => ProcessSimpleField(typeString),

                _ => "Invalid schema format"
            };
        }

        return result;
    }
    
    private Dictionary<string, object> ProcessJsonObject(JsonElement element)
    {
        var result = new Dictionary<string, object>();
        
        foreach (var property in element.EnumerateObject())
        {
            result[property.Name] = property.Value.ValueKind switch
            {
                JsonValueKind.Object => ProcessJsonObject(property.Value),
                JsonValueKind.String => ProcessSimpleField(property.Value.GetString()),
                _ => "Invalid schema format"
            };
        }
        
        return result;
    }
    
    private object ProcessSimpleField(string? typeString)
    {
        if (string.IsNullOrWhiteSpace(typeString))
        {
             return "Unknown Type: null or empty";
        }

        if (!Enum.TryParse<FieldTypeEnum>(typeString, true, out var typeEnum))
        {
            return $"Unknown Type: {typeString}";
        }
        
        return _generatorsCache.TryGetValue(typeEnum, out var generator) 
            ? generator.Generate() 
            : $"Unsupported type: {typeString}";
    }
}
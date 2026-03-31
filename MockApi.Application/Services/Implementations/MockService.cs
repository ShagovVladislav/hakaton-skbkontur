using MockApi.Application.Services.Abstractions;
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;
using MockApi.Application.Dto;

namespace MockApi.Application.Services.Implementations;

public class MockService : IMockService
{
    private readonly Dictionary<FieldTypeEnum, IValueGenerator> _generatorsCache;
    private readonly IFieldTypeInferenceService _fieldTypeInferenceService;

    public MockService(
        IEnumerable<IValueGenerator> generators,
        IFieldTypeInferenceService fieldTypeInferenceService)
    {
        _fieldTypeInferenceService = fieldTypeInferenceService;

        var generatorsList = generators.ToList();
        _generatorsCache = Enum.GetValues<FieldTypeEnum>()
            .ToDictionary(
                type => type,
                type => generatorsList.FirstOrDefault(g => g.CanHandle(type))!
            )
            .Where(x => x.Value != null)
            .ToDictionary(x => x.Key, x => x.Value);
    }

    public async Task<Dictionary<string, object>> GenerateMockData(Dictionary<string, FieldConfig?> schema)
    {
        var enrichedSchema = await _fieldTypeInferenceService.InferAndFillMissingTypesAsync(schema);
        return ProcessSchemaRecursive(enrichedSchema);
    }

    public async Task<string> GenerateMockDataWithAi(string description)
    {
        return await _fieldTypeInferenceService.GenerateMockDataFromDescriptionAsync(description);
    }

    private Dictionary<string, object> ProcessSchemaRecursive(Dictionary<string, FieldConfig> schema)
    {
        var result = new Dictionary<string, object>();

        foreach (var (key, config) in schema)
        {
            result[key] = GenerateValue(config, key);
        }

        return result;
    }

    private object GenerateValue(FieldConfig config, string fieldName)
    {
        if (config.Properties != null && config.Properties.Count > 0)
        {
            return ProcessSchemaRecursive(config.Properties);
        }

        if (config.Type == FieldTypeEnum.Array)
        {
            return GenerateArray(config, fieldName);
        }

        return GenerateSimpleField(config, fieldName);
    }

    private List<object> GenerateArray(FieldConfig config, string fieldName)
    {
        var list = new List<object>();
        var size = config.ArraySize ?? 5;

        if (config.Items == null)
        {
            return list;
        }

        for (int i = 0; i < size; i++)
        {
            list.Add(GenerateValue(config.Items, fieldName));
        }

        return list;
    }

    private object GenerateSimpleField(FieldConfig config, string fieldName)
    {
        if (config.Type == null)
            return "Type is not defined";

        if (!_generatorsCache.TryGetValue(config.Type.Value, out var generator))
            return $"Unsupported type: {config.Type}";
        
        var mode = Enum.TryParse<StringMode>(fieldName, true, out var result) 
            ? result 
            : StringMode.None;
        
        return generator.WithMode(mode).Generate(config);
    }
}
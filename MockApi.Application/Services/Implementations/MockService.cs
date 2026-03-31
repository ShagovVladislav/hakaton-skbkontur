using MockApi.Application.Services.Abstractions;
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Services.Implementations;

public class MockService(IEnumerable<IGenerationalValue> generators) : IMockService
{
    public Dictionary<string, object> GenerateMockData(Dictionary<string, string> schema)
    {
        var result = new Dictionary<string, object>();

        foreach (var field in schema)
        {
            if (Enum.TryParse<FieldTypeEnum>(field.Value, true, out var typeEnum))
            {
                var generator = generators.FirstOrDefault(g => g.CanHandle(typeEnum));
                
                result[field.Key] = generator != null 
                    ? generator.Generate() 
                    : $"Unsupported type: {field.Value}";
            }
            else
            {
                result[field.Key] = "Unknown Type";
            }
        }

        return result;
    }
}
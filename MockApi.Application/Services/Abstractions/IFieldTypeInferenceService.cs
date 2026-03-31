namespace MockApi.Application.Services.Abstractions;

public interface IFieldTypeInferenceService
{
    Task<Dictionary<string, object>> InferAndFillMissingTypesAsync(Dictionary<string, object?> fields);
    Task<string> GenerateMockDataFromDescriptionAsync(string description);
}
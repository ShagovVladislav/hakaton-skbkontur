using MockApi.Application.Dto;

namespace MockApi.Application.Services.Abstractions;

public interface IFieldTypeInferenceService
{
    Task<Dictionary<string, FieldConfig>> InferAndFillMissingTypesAsync(Dictionary<string, FieldConfig?> fields);
    Task<string> GenerateMockDataFromDescriptionAsync(string description);
}
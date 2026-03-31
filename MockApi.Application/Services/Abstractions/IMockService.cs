using MockApi.Application.Dto;

namespace MockApi.Application.Services.Abstractions;

public interface IMockService
{
    Task<Dictionary<string, object>> GenerateMockData(Dictionary<string, FieldConfig?> schema);
    Task<string> GenerateMockDataWithAi(string description);
}
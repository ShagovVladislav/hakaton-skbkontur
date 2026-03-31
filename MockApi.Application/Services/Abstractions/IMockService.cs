namespace MockApi.Application.Services.Abstractions;

public interface IMockService
{
    Task<Dictionary<string, object>> GenerateMockData(Dictionary<string, object?> schema);
    Task<string> GenerateMockDataWithAi(string description);
}
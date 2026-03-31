namespace MockApi.Application.Services.Abstractions;

public interface IMockService
{
    Task<Dictionary<string, object>> GenerateMockDataWithAiAsync(Dictionary<string, object?> schema);
}
namespace MockApi.Application.Services.Abstractions;

public interface IMockService
{
    Dictionary<string, object> GenerateMockData(Dictionary<string, object> schema);
}
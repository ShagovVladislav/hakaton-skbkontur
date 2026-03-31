namespace MockApi.Infrastructure.StorageProviders.Abstractions;

public interface IStorageProvider
{
    public string[] GetValues(string section);
}
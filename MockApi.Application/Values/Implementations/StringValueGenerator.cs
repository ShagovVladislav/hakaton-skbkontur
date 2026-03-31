using MockApi.Application.Values.Abstractions;
using MockApi.Domain;
using MockApi.Infrastructure.StorageProviders.Abstractions;

namespace MockApi.Application.Values.Implementations;

public class StringValueGenerator : IValueGenerator
{
    private readonly Random _random;
    private readonly IStorageProvider _storageProvider;
    private readonly StringMode _mode;

    public StringValueGenerator(IStorageProvider storageProvider, Random? random = null,
        StringMode mode = StringMode.Random)
    {
        _storageProvider = storageProvider;
        _random = random ?? new Random();
        _mode = mode;
    }

    public StringValueGenerator WithMode(StringMode mode) => new StringValueGenerator(_storageProvider, _random, mode);

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == FieldTypeEnum.String;
    }

    public object Generate() => _mode switch
    {
        StringMode.FirstName => PickRandom(_storageProvider.GetValues("firstNames")),
        StringMode.LastName => PickRandom(_storageProvider.GetValues("lastNames")),
        StringMode.MiddleName => PickRandom(_storageProvider.GetValues("middleNames")),
        StringMode.FullName => BuildFullName(),
        StringMode.Email => BuildEmail(),
        _ => $"str_{Guid.NewGuid().ToString()[..5]}"
    };

    private string PickRandom(string[] values) => values[_random.Next(values.Length)];

    private string BuildFullName()
    {
        var firstName = PickRandom(_storageProvider.GetValues("firstNames"));
        var lastName = PickRandom(_storageProvider.GetValues("lastNames"));
        var middleName = PickRandom(_storageProvider.GetValues("middleNames"));
        return $"{lastName} {firstName} {middleName}";
    }

    private string BuildEmail()
    {
        var firstName = PickRandom(_storageProvider.GetValues("firstNames"));
        var mailPostfix = PickRandom(_storageProvider.GetValues("mailPostfixes"));
        return $"{firstName}@{mailPostfix}";   
    }
}
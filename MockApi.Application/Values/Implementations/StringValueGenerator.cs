using MockApi.Application.Dto;
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

    public IValueGenerator WithMode(StringMode mode) => new StringValueGenerator(_storageProvider, _random, mode);

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == FieldTypeEnum.String;
    }

    public object Generate(FieldConfig? config)
    {
        var value = _mode switch
        {
            StringMode.FirstName => PickRandom(_storageProvider.GetValues("firstNames")),
            StringMode.LastName => PickRandom(_storageProvider.GetValues("lastNames")),
            StringMode.MiddleName => PickRandom(_storageProvider.GetValues("middleNames")),
            StringMode.FullName => BuildFullName(),
            StringMode.Email => BuildEmail(),
            StringMode.Phone => BuildPhoneNumber(),
            _ => Guid.NewGuid().ToString("N")[..Math.Min(10, 32)] 
        };

        return config is null 
            ?  value 
            : ApplyLengthConstraints(value, config.MinLength, config.MaxLength);
    }
    
    private string ApplyLengthConstraints(string value, int? min, int? max)
    {
        if (max.HasValue && value.Length > max.Value)
        {
            value = value[..max.Value];
        }

        if (!min.HasValue || value.Length >= min.Value) return value;
        var diff = min.Value - value.Length;
        var padding = Guid.NewGuid().ToString("N");
        while (padding.Length < diff) padding += Guid.NewGuid().ToString("N");
        value += padding[..diff];

        return value;
    }

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
        var firstName = PickRandom(_storageProvider.GetValues("mailPrefix"));
        var mailPostfix = PickRandom(_storageProvider.GetValues("mailPostfixes"));
        return $"{firstName}@{mailPostfix}";   
    }
    
    private string BuildPhoneNumber()
    {
        var phone = PickRandom(_storageProvider.GetValues("phones"));
        return phone;
    }
}
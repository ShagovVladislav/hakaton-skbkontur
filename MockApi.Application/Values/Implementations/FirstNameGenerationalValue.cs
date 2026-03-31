using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class FirstNameGenerationalValue : IGenerationalValue
{
    private readonly Random _random;
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.FirstName;

    private readonly string[] _names =
    [
        "John", "Bob", "Matvey", "Danil", "Vlad", "Slava"
    ];

    public FirstNameGenerationalValue(Random? random = null, string[]? names = null)
    {
        _random = random ?? new Random();
        if (names != null)
            _names = names;
    }

    public object Generate()
    {
        return _names[_random.Next(_names.Length)];
    }

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == _fieldType;
    }
}
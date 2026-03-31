using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class BooleanGenerationalValue : IGenerationalValue
{
    private readonly Random _random;
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Boolean;

    public BooleanGenerationalValue(Random? random = null)
    {
        _random = random ?? new Random();
    }

    public object Generate()
    {
        return _random.Next() % 2 == 0;
    }

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == _fieldType;
    }
}
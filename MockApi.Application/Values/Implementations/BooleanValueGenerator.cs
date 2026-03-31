using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class BooleanValueGenerator : IValueGenerator
{
    private readonly Random _random;
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Boolean;

    public BooleanValueGenerator(Random? random = null)
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

    public object GenerateUntyped() => Generate();
}
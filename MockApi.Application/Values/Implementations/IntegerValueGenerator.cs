using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class IntegerValueGenerator : IValueGenerator<int>
{
    private readonly Random _random;
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Integer;

    public IntegerValueGenerator(Random? random)
    {
        _random = random ?? new Random();
    }

    public int Generate()
    {
        return _random.Next();
    }

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == FieldTypeEnum.Integer;
    }

    public object GenerateUntyped() => Generate();
}
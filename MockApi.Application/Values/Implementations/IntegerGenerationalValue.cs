using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class IntegerGenerationalValue : IGenerationalValue
{
    private readonly Random _random;
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Integer;

    public IntegerGenerationalValue(Random? random)
    {
        _random = random ?? new Random();
    }

    public object Generate()
    {
        return _random.Next();
    }

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == FieldTypeEnum.Integer;
    }

    public object GenerateUntyped() => Generate();
}
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class DecimalValueGenerator(Random random) : IValueGenerator
{
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Decimal;

    public object Generate()
    {
        return (Decimal)random.NextDouble();
    }

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == _fieldType;
    }

    public object GenerateUntyped() => Generate();
}
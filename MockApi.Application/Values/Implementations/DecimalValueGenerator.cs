using MockApi.Application.Dto;
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class DecimalValueGenerator(Random random) : IValueGenerator
{
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Decimal;

    public object Generate(FieldConfig? config)
    {
        return (Decimal)random.NextDouble();
    }

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == _fieldType;
    }

    public IValueGenerator WithMode(StringMode mode)
    {
        throw new NotImplementedException();
    }

    public object GenerateUntyped() => Generate(null);
}
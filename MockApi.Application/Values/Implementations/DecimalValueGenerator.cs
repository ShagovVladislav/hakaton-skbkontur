using MockApi.Application.Dto;
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class DecimalValueGenerator(Random random) : IValueGenerator
{
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Decimal;
    private int _min;
    private int _max = 100;

    public object Generate(FieldConfig? config)
    {
        var min = config?.MinValue ?? _min;
        var max = config?.MaxValue ?? _max;
        return (decimal)random.NextDouble() * (max - min) + min;
    }

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == _fieldType;
    }

    public IValueGenerator WithMode(StringMode mode)
    {
        return this;
    }
}
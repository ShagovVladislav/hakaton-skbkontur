using MockApi.Application.Dto;
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class FloatValueGenerator(Random? random) : IValueGenerator
{
    private readonly Random _random = random ?? new Random();
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Float;
    private int _min;
    private int _max = 100;

    public object Generate(FieldConfig? config)
    {
        var min = config?.MinValue ?? _min;
        var max = config?.MaxValue ?? _max;
        return _random.NextDouble() * (max - min) + min;
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
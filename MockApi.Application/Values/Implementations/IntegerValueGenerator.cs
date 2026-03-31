using MockApi.Application.Dto;
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class IntegerValueGenerator : IValueGenerator
{
    private readonly Random _random;
    private readonly int _min;
    private readonly int _max = 100;
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Integer;
    public IntegerValueGenerator(Random? random, int min = 0, int max = 100)
    {
        _random = random ?? new Random();
        _min = min;
        _max = max;
    }
    
    public object Generate(FieldConfig? config)
    {
        var min = config?.MinValue ?? _min;
        var max = config?.MaxValue ?? _max;

        return min >= max ?
            min : _random.Next(min, max);
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
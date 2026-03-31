using MockApi.Application.Dto;
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class DoubleValueGenerator : IValueGenerator
{
    private readonly Random _random;
    private readonly double _min = 0;
    private readonly double _max = 100;
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Double;

    public DoubleValueGenerator(Random? random, double min = 0, double max = 100)
    {
        _random = random ?? new Random();
        _min = min;
        _max = max;
    }

    public DoubleValueGenerator WithMin(double min) => new DoubleValueGenerator(_random, min, _max);
    public DoubleValueGenerator WithMax(double max) => new DoubleValueGenerator(_random, _min, max);
    public DoubleValueGenerator WithRange(double min, double max) => new DoubleValueGenerator(_random, min, max);

    public object Generate(FieldConfig? config)
    {
        return _random.NextDouble() * (_max - _min) + _min;
    }

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == FieldTypeEnum.Double;
    }

    public IValueGenerator WithMode(StringMode mode)
    {
        throw new NotImplementedException();
    }

    public object GenerateUntyped() => Generate(null);
}
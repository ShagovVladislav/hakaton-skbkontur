using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class IntegerValueGenerator : IValueGenerator
{
    private readonly Random _random;
    private readonly int _min = 0;
    private readonly int _max = 100;
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Integer;

    public IntegerValueGenerator(Random? random, int min = 0, int max = 100)
    {
        _random = random ?? new Random();
        _min = min;
        _max = max;
    }
    
    public IntegerValueGenerator WithMin(int min) => new IntegerValueGenerator(_random, min, _max);
    public IntegerValueGenerator WithMax(int max) => new IntegerValueGenerator(_random, _min, max);
    public IntegerValueGenerator WithRange(int min, int max) => new IntegerValueGenerator(_random, min, max);

    public object Generate()
    {
        return _random.Next(_min, _max);
    }

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == FieldTypeEnum.Integer;
    }

    public object GenerateUntyped() => Generate();
}
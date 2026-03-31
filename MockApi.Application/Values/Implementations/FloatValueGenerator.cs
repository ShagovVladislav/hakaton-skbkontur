using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class FloatValueGenerator(Random? random) : IValueGenerator
{
    private readonly Random _random = random ?? new Random();
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Float;

    public object Generate()
    {
        return (float)_random.NextDouble();
    }

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == _fieldType;
    }

    public object GenerateUntyped() => Generate();
}
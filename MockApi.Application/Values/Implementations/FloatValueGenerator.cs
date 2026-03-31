using MockApi.Application.Dto;
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class FloatValueGenerator(Random? random) : IValueGenerator
{
    private readonly Random _random = random ?? new Random();
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Float;

    public object Generate(FieldConfig? config)
    {
        return (float)_random.NextDouble();
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
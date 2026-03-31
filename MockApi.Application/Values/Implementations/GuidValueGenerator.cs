using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class GuidValueGenerator(Random? random) : IValueGenerator
{
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Guid;

    public object Generate()
    {
        return Guid.NewGuid();
    }

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == _fieldType;
    }

    public object GenerateUntyped() => Generate();
}
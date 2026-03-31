using MockApi.Application.Dto;
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class GuidValueGenerator : IValueGenerator
{
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Guid;

    public object Generate(FieldConfig? config)
    {
        return Guid.NewGuid();
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
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class StringGenerationalValue : IGenerationalValue
{
    public bool CanHandle(FieldTypeEnum value)
    {
        return value == FieldTypeEnum.String;
    }

    public object Generate()
    {
        return $"str_{Guid.NewGuid().ToString()[..5]}";
    }
}
using Microsoft.VisualBasic.FileIO;
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class StringGenerationalValue<T> : GenerationalValue<string>
{
    public bool CanHandle(FieldTypeEnum value)
    {
        return value == FieldTypeEnum.String;
    }

    object IGenerationalValue.GenerateUntyped() => Generate();

    public string Generate()
    {
        return $"str_{Guid.NewGuid().ToString().Substring(0, 5)}";
    }
}
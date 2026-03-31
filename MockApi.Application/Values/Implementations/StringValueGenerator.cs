using Microsoft.VisualBasic.FileIO;
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class StringValueGenerator<T> : IValueGenerator<string>
{
    public bool CanHandle(FieldTypeEnum value)
    {
        return value == FieldTypeEnum.String;
    }

    public string Generate()
    {
        return $"str_{Guid.NewGuid().ToString()[..5]}";
    }
}
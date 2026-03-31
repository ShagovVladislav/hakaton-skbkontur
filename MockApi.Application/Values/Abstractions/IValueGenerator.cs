using MockApi.Application.Dto;
using MockApi.Domain;

namespace MockApi.Application.Values.Abstractions;

public interface IValueGenerator
{
    public object Generate(FieldConfig? config = null);
    bool CanHandle(FieldTypeEnum value);
    IValueGenerator WithMode(StringMode mode);
}
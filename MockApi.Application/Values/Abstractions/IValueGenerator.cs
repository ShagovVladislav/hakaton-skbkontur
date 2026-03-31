using MockApi.Domain;

namespace MockApi.Application.Values.Abstractions;

public interface IValueGenerator
{
    public object Generate();
    bool CanHandle(FieldTypeEnum value);
}
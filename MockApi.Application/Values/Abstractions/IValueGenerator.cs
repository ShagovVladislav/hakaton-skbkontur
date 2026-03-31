using MockApi.Domain;

namespace MockApi.Application.Values.Abstractions;

public interface IValueGenerator<out T>
{
    public T Generate();
    bool CanHandle(FieldTypeEnum value);
}
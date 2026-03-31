using MockApi.Domain;

namespace MockApi.Application.Values.Abstractions;

public interface IGenerationalValue 
{
    bool CanHandle(FieldTypeEnum value);
    object Generate();
}
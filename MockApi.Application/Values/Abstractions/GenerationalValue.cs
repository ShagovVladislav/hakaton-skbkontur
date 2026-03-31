using MockApi.Domain;

namespace MockApi.Application.Values.Abstractions;

public interface GenerationalValue<out T> : IGenerationalValue{
    public T Generate();
}
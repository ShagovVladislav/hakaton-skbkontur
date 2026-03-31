namespace MockApi.Application.Values.Abstractions;

public interface GenerationalValue<T>
{
    public T Generate();
}
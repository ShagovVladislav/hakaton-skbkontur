using MockApi.Application.Values.Abstractions;

namespace MockApi.Application.Values.Implementations;

public class StringGenerationalValue<T> : GenerationalValue<string>
{
    public string Generate()
    {
        return $"str_{Guid.NewGuid().ToString().Substring(0, 5)}";
    }
}
using MockApi.Application.Values.Abstractions;

namespace MockApi.Application.Values.Implementations;

public class BooleanGenerationalValue : GenerationalValue<bool>
{
    private readonly Random _random;

    public BooleanGenerationalValue(Random? random = null)
    {
        _random = random ?? new Random();
    }
    
    public bool Generate()
    {
        return _random.Next() % 2 == 0;
    }
}
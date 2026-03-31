using MockApi.Application.Values.Abstractions;

namespace MockApi.Application.Values.Implementations;

public class IntegerGenerationalValue : GenerationalValue<int>
{
    private readonly Random _random;

    public IntegerGenerationalValue(Random? random)
    {
        _random = random ?? new Random();
    }
    
    public int Generate()
    {
        var random = new Random();
        return random.Next();
    }
}
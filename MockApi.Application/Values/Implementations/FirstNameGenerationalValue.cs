using MockApi.Application.Values.Abstractions;

namespace MockApi.Application.Values.Implementations;

public class FirstNameGenerationalValue : GenerationalValue<string>
{
    private readonly Random _random;

    private readonly string[] _names =
    [
        "John", "Bob", "Matvey", "Danil", "Vlad", "Slava"
    ];

    public FirstNameGenerationalValue(Random? random = null, string[]? names = null)
    {
        _random = random ?? new Random();
        if (names != null)
            _names = names;
    }

    public string Generate()
    {
        return _names[_random.Next(_names.Length)];
    }
}
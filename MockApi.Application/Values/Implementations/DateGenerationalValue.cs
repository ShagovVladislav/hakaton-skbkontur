using MockApi.Application.Values.Abstractions;

namespace MockApi.Application.Values.Implementations;

public class DateGenerationalValue : GenerationalValue<string>
{
    private readonly Random _random;

    public DateGenerationalValue(Random? random = null)
    {
        _random = random ?? new Random();
    }
    
    public string Generate()
    {
        var start = new DateTime(1995, 1, 1);
        var range = (DateTime.Today - start).Days;           
        var date = start.AddDays(_random.Next(range));
        return $"{date.Day}.{date.Month}.{date.Year}";
    }
}
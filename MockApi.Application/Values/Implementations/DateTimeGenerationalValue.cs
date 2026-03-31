using MockApi.Application.Values.Abstractions;

namespace MockApi.Application.Values.Implementations;

public class DateTimeGenerationalValue : GenerationalValue<DateTime>
{
    private readonly Random _random;

    public DateTimeGenerationalValue(Random? random = null)
    {
        _random = random ?? new Random();
    }
    
    public DateTime Generate()
    {
        var start = new DateTime(1995, 1, 1);
        var range = (DateTime.Today - start).Days;           
        return start.AddDays(_random.Next(range));
    }
}
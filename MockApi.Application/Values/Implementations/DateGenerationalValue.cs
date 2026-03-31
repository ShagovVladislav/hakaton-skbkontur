using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class DateGenerationalValue : GenerationalValue<string>
{
    private readonly Random _random;
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Date;

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

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == _fieldType;
    }

    public object GenerateUntyped() => Generate();
}
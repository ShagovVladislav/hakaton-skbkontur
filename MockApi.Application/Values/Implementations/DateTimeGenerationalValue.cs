using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class DateTimeGenerationalValue : IGenerationalValue
{
    private readonly Random _random;
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.DateTime;

    public DateTimeGenerationalValue(Random? random = null)
    {
        _random = random ?? new Random();
    }

    public object Generate()
    {
        var start = new DateTime(1995, 1, 1);
        var range = (DateTime.Today - start).Days;
        return start.AddDays(_random.Next(range));
    }

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == _fieldType;
    }
}
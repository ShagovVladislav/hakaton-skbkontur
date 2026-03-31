using MockApi.Application.Dto;
using MockApi.Application.Values.Abstractions;
using MockApi.Domain;

namespace MockApi.Application.Values.Implementations;

public class DateValueGenerator : IValueGenerator
{
    private readonly Random _random;
    private readonly FieldTypeEnum _fieldType = FieldTypeEnum.Date;

    public DateValueGenerator(Random? random = null)
    {
        _random = random ?? new Random();
    }

    public object Generate(FieldConfig? config)
    {
        var start = new DateOnly(1995, 1, 1);
        var today = DateOnly.FromDateTime(DateTime.Today);
        var range = today.DayNumber - start.DayNumber; 
        return start.AddDays(_random.Next(range));
    }

    public bool CanHandle(FieldTypeEnum value)
    {
        return value == _fieldType;
    }

    public IValueGenerator WithMode(StringMode mode)
    {
        return this;
    }
}
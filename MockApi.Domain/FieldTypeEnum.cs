using System.Text.Json.Serialization;

namespace MockApi.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FieldTypeEnum
{
    String,
    Integer,
    Boolean,
    Date,
    DateTime,
    FirstName,
    Double,
    Float,
    Decimal, 
    Array
}
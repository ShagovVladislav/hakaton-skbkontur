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
    Double,
    Float,
    Decimal, 
    Array,
    Guid
}
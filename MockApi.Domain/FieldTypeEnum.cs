using System.Text.Json.Serialization;

namespace MockApi.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FieldTypeEnum
{
    String,
    Integer,
    Boolean,
    Double,
    DateTime,
    Guid,
    Double,
    Float,
    Decimal,
    Array
}
using System.Text.Json.Serialization;

namespace MockApi.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FieldTypeEnum
{
    String,
    Integer,
    Boolean,
    DateTime,
    Date,
    Double,
    Float,
    Decimal,
    Array,
    Guid
}
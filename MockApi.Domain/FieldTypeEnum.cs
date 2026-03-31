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
    LastName,
    MiddleName,
    FullName,
    Email,
    PhoneNumber
}
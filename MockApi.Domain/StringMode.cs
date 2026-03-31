using System.Text.Json.Serialization;

namespace MockApi.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StringMode
{
    Random,
    FirstName,
    MiddleName,
    LastName,
    FullName,
    Email,
    Phone,
    None
}
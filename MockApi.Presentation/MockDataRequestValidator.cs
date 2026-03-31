using System.Text.Json;
using FluentValidation;
using MockApi.Application.Dto;
using MockApi.Domain;

namespace MockApi.Presentation;

public class MockDataRequestValidator : AbstractValidator<MockDataRequest>
{
    public MockDataRequestValidator()
    {
        RuleFor(x => x.Schema)
            .Must(BeValidSchema)
            .WithMessage("Invalid schema format");
    }

    private bool BeValidSchema(Dictionary<string, object?>? schema)
    {
        if (schema == null || schema.Count == 0)
            return true;

        foreach (var field in schema)
        {
            switch (field.Value)
            {
                case null:
                    continue;
                case Dictionary<string, object> nested:
                {
                    if (!BeValidSchema(nested))
                        return false;
                    break;
                }
                case JsonElement element:
                    switch (element.ValueKind)
                    {
                        case JsonValueKind.String:
                            if (!IsValidFieldType(element.GetString()))
                                return false;
                            break;
                        case JsonValueKind.Object:
                            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(element.GetRawText());
                            if (!BeValidSchema(dict))
                                return false;
                            break;
                        case JsonValueKind.Null:
                            break;
                        default:
                            return false;
                    }

                    break;
                case string str:
                {
                    if (!IsValidFieldType(str))
                        return false;
                    break;
                }
                default:
                    return false;
            }
        }

        return true;
    }

    private bool IsValidFieldType(string? value)
    {
        return string.IsNullOrWhiteSpace(value) 
               || Enum.TryParse<FieldTypeEnum>(value.Trim(), ignoreCase: true, out _);
    }
}
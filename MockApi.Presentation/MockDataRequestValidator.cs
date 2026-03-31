using FluentValidation;
using MockApi.Application.Dto;
using MockApi.Domain;

namespace MockApi.Presentation;

public class MockDataRequestValidator : AbstractValidator<MockDataRequest>
{
    public MockDataRequestValidator()
    {
        RuleForEach(x => x.Schema.Values)
            .SetValidator(new FieldConfigValidator());
    }
}

public class FieldConfigValidator : AbstractValidator<FieldConfig>
{
    public FieldConfigValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum()
            .When(x => x.Type.HasValue);

        RuleForEach(x => x.Properties!.Values)
            .SetValidator(this)
            .When(x => x.Properties != null && x.Properties.Count > 0);

        RuleFor(x => x.Items)
            .SetValidator(this)
            .When(x => x.Type == FieldTypeEnum.Array && x.Items != null);
    }
}
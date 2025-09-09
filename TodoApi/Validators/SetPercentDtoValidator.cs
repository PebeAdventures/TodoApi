using FluentValidation;
using TodoApi.DTOs;

namespace TodoApi.Validators;

public class SetPercentDtoValidator : AbstractValidator<SetPercentDto>
{
    public SetPercentDtoValidator()
    {
        RuleFor(x => x.PercentComplete)
            .InclusiveBetween(0, 100)
            .WithMessage("PercentComplete must be between 0 and 100.");
    }
}

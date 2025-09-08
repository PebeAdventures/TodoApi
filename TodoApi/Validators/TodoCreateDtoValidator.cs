using FluentValidation;
using TodoApi.DTOs;

namespace TodoApi.Validators;

public class TodoCreateDtoValidator : AbstractValidator<TodoCreateDto>
{
    public TodoCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters.");
        RuleFor(x => x.DueAt)
            .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");
    }
}

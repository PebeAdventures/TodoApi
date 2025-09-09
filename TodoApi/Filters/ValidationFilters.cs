using FluentValidation;

namespace TodoApi.Filters;

/// <summary>
/// Minimal API endpoint filter that validates a DTO using FluentValidation.
/// Returns 400 with validation errors when invalid.
/// </summary>
public sealed class ValidationFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var dto = context.Arguments.OfType<T>().FirstOrDefault();
        if (dto is null)
            return Results.BadRequest(new { message = "Invalid payload." });

        var result = await _validator.ValidateAsync(dto);
        if (!result.IsValid)
            return Results.ValidationProblem(result.ToDictionary());

        return await next(context);
    }
}

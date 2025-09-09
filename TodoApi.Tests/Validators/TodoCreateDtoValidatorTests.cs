using FluentValidation.TestHelper;
using TodoApi.DTOs;
using TodoApi.Validators;

namespace TodoApi.Tests.Validators;
public class TodoCreateDtoValidatorTests
{
    private readonly TodoCreateDtoValidator _validator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Title_is_required(string? title)
    {
        var dto = new TodoCreateDto { Title = title ?? "", DueAt = DateTime.UtcNow.AddDays(1) };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData(200, true)]
    [InlineData(201, false)]
    public void Title_length_limit(int length, bool isValid)
    {
        var dto = new TodoCreateDto { Title = new string('a', length), DueAt = DateTime.UtcNow.AddDays(1) };
        var result = _validator.TestValidate(dto);

        if (isValid) result.ShouldNotHaveValidationErrorFor(x => x.Title);
        else result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Description_length_limit()
    {
        var dto = new TodoCreateDto
        {
            Title = "ok",
            Description = new string('x', 1001),
            DueAt = DateTime.UtcNow.AddDays(1)
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void DueAt_must_be_now_or_future()
    {
        var dto = new TodoCreateDto { Title = "ok", DueAt = DateTime.UtcNow.AddSeconds(-1) };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.DueAt);
    }
}

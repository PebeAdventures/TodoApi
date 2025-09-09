using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApi.DTOs;
using TodoApi.Validators;

namespace TodoApi.Tests.Validators;
public class TodoUpdateDtoValidatorTests
{
    private readonly TodoUpdateDtoValidator _validator = new();

    [Fact]
    public void Title_required_and_length()
    {
        var tooLong = new string('a', 201);
        var dto = new TodoUpdateDto
        {
            Title = tooLong,
            Description = "desc",
            DueAt = DateTime.UtcNow.AddDays(1),
            PercentComplete = 10,
            IsDone = false
        };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Theory]
    [InlineData(-1, false)]
    [InlineData(0, true)]
    [InlineData(100, true)]
    [InlineData(101, false)]
    public void PercentComplete_range(int percent, bool valid)
    {
        var dto = new TodoUpdateDto
        {
            Title = "ok",
            Description = "desc",
            DueAt = DateTime.UtcNow.AddDays(1),
            PercentComplete = percent,
            IsDone = false
        };

        var result = _validator.TestValidate(dto);
        if (valid) result.ShouldNotHaveValidationErrorFor(x => x.PercentComplete);
        else result.ShouldHaveValidationErrorFor(x => x.PercentComplete);
    }

    [Fact]
    public void DueAt_must_be_now_or_future()
    {
        var dto = new TodoUpdateDto
        {
            Title = "ok",
            Description = "desc",
            DueAt = DateTime.UtcNow.AddMinutes(-5),
            PercentComplete = 0,
            IsDone = false
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.DueAt);
    }
}

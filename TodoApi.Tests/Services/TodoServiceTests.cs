using AutoMapper;
using FluentAssertions;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Mapping;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Services;
using TodoApi.Tests.TestUtilities;

namespace TodoApi.Tests.Services;
public class TodoServiceTests
{
    private static (TodoDbContext ctx, TodoRepository repo, TodoService svc) CreateSut()
    {
        var ctx = SqliteDbFactory.CreateContext();
        var repo = new TodoRepository(ctx);
        var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<MappingProfile>()));
        var svc = new TodoService(repo, mapper);
        return (ctx, repo, svc);
    }

    [Fact]
    public async Task GetIncoming_Today_returns_only_today()
    {
        var (ctx, _, svc) = CreateSut();

        var today = DateTime.UtcNow.Date;
        ctx.Todos.AddRange(
            new Todo { Title = "yesterday", DueAt = today.AddDays(-1) },
            new Todo { Title = "today-1", DueAt = today.AddHours(10) },
            new Todo { Title = "tomorrow", DueAt = today.AddDays(1) }
        );
        await ctx.SaveChangesAsync();

        var result = await svc.GetIncomingAsync(IncomingRange.Today);

        result.Select(x => x.Title).Should().BeEquivalentTo(new[] { "today-1" });
    }

    [Fact]
    public async Task UpdateAsync_returns_updated_entity()
    {
        var (ctx, _, svc) = CreateSut();
        var item = new Todo { Title = "old", Description = "d", DueAt = DateTime.UtcNow.AddDays(1) };
        ctx.Todos.Add(item);
        await ctx.SaveChangesAsync();

        var dto = new TodoUpdateDto
        {
            Title = "new",
            Description = "d2",
            DueAt = item.DueAt,
            PercentComplete = 20,
            IsDone = false
        };

        var updated = await svc.UpdateAsync(item.Id, dto);

        updated.Should().NotBeNull();
        updated!.Title.Should().Be("new");
        (await ctx.Todos.FindAsync(item.Id))!.Title.Should().Be("new");
    }

    [Fact]
    public async Task SetPercentCompleteAsync_updates_and_returns_entity()
    {
        var (ctx, _, svc) = CreateSut();
        var item = new Todo { Title = "t", DueAt = DateTime.UtcNow.AddDays(1) };
        ctx.Todos.Add(item);
        await ctx.SaveChangesAsync();

        var updated = await svc.SetPercentCompleteAsync(item.Id, 100);

        updated.Should().NotBeNull();
        updated!.PercentComplete.Should().Be(100);
    }

    [Fact]
    public async Task MarkDoneAsync_sets_done_and_100_percent()
    {
        var (ctx, _, svc) = CreateSut();
        var item = new Todo { Title = "t", DueAt = DateTime.UtcNow.AddDays(1), PercentComplete = 10, IsDone = false };
        ctx.Todos.Add(item);
        await ctx.SaveChangesAsync();

        var updated = await svc.MarkDoneAsync(item.Id);

        updated.Should().NotBeNull();
        updated!.IsDone.Should().BeTrue();
        updated.PercentComplete.Should().Be(100);
    }

    [Fact]
    public async Task DeleteAsync_removes_entity_and_returns_true()
    {
        var (ctx, _, svc) = CreateSut();
        var item = new Todo { Title = "t", DueAt = DateTime.UtcNow.AddDays(1) };
        ctx.Todos.Add(item);
        await ctx.SaveChangesAsync();

        var ok = await svc.DeleteAsync(item.Id);

        ok.Should().BeTrue();
        (await ctx.Todos.FindAsync(item.Id)).Should().BeNull();
    }
}

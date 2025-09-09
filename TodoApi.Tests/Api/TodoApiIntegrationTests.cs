using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TodoApi.DTOs;
using TodoApi.Tests.TestUtilities;

namespace TodoApi.Tests.Api;
public class TodoApiIntegrationTests
{
    [Fact]
    public async Task Create_invalid_returns_400()
    {
        await using var factory = new ApiFactory();
        var client = factory.CreateClient();

        var payload = new
        {
            title = "", // invalid
            description = "x",
            dueAt = DateTime.UtcNow.AddDays(-1) // invalid
        };

        var resp = await client.PostAsJsonAsync("/api/todos", payload);
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_and_get_by_id_returns_201_and_200()
    {
        await using var factory = new ApiFactory();
        var client = factory.CreateClient();

        var create = await client.PostAsJsonAsync("/api/todos", new
        {
            title = "Sample",
            description = "D",
            dueAt = DateTime.UtcNow.AddDays(1),
            percentComplete = 0,
            isDone = false
        });

        create.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await create.Content.ReadFromJsonAsync<TodoDto>();
        created.Should().NotBeNull();
        created!.Id.Should().BeGreaterThan(0);

        var get = await client.GetAsync($"/api/todos/{created.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);
        var fetched = await get.Content.ReadFromJsonAsync<TodoDto>();
        fetched!.Title.Should().Be("Sample");
    }

    [Fact]
    public async Task Mark_done_returns_200_with_dto()
    {
        await using var factory = new ApiFactory();
        var client = factory.CreateClient();

        var create = await client.PostAsJsonAsync("/api/todos", new
        {
            title = "T",
            description = "D",
            dueAt = DateTime.UtcNow.AddDays(1),
            percentComplete = 0,
            isDone = false
        });
        var dto = await create.Content.ReadFromJsonAsync<TodoDto>();

        var resp = await client.PostAsync($"/api/todos/{dto!.Id}/done", content: null);
        resp.StatusCode.Should().Be(HttpStatusCode.OK);

        var updated = await resp.Content.ReadFromJsonAsync<TodoDto>();
        updated!.IsDone.Should().BeTrue();
        updated.PercentComplete.Should().Be(100);
    }

    [Fact]
    public async Task Incoming_invalid_enum_returns_400()
    {
        await using var factory = new ApiFactory();
        var client = factory.CreateClient();

        var resp = await client.GetAsync("/api/todos/incoming?range=Foo");
        resp.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;

namespace TodoApi.Tests.TestUtilities;
/// <summary>
/// Creates a new EF Core DbContext backed by an in-memory SQLite database.
/// Each call returns a fresh, isolated database.
/// </summary>
public static class SqliteDbFactory
{
    public static TodoDbContext CreateContext()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<TodoDbContext>()
            .UseSqlite(connection)
            .Options;

        var ctx = new TodoDbContext(options);
        ctx.Database.EnsureCreated();
        return ctx;
    }
}

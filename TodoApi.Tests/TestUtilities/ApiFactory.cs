using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using TodoApi.Data;

namespace TodoApi.Tests.TestUtilities;
/// <summary>
/// Minimal API factory with SQLite in-memory database for integration tests.
/// </summary>
public class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // 1) hard remove any previous registrations related to the DbContext
            services.RemoveAll<DbContextOptions<TodoDbContext>>();
            services.RemoveAll<TodoDbContext>();
            services.RemoveAll<IDbContextFactory<TodoDbContext>>();

            // 2) single provider: SQLite in-memory with a keep-alive connection
            var keepAlive = new SqliteConnection("Filename=:memory:");
            keepAlive.Open();

            services.AddDbContext<TodoDbContext>(opt =>
            {
                opt.UseSqlite(keepAlive);
            });

            // 3) build provider & create schema
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
            db.Database.EnsureCreated();
        });
    }
}

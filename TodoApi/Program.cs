using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Filters;
using TodoApi.Interfaces;
using TodoApi.Mapping;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Services;
using TodoApi.Validators;

var builder = WebApplication.CreateBuilder(args);
var isTesting = builder.Environment.IsEnvironment("Testing");

//Connection string
if (!isTesting)
{
    builder.Services.AddDbContext<TodoDbContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
}

//Automapper registration
builder.Services.AddAutoMapper(typeof(MappingProfile));

//FluentValidation registration
builder.Services.AddValidatorsFromAssemblyContaining<TodoCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TodoUpdateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<SetPercentDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply pending ef Core migrations at startup if isTesting == false
if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.Migrate();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Get all
app.MapGet("/api/todos", async (ITodoService service, IMapper mapper) =>
{
    var entities = await service.GetAllAsync();
    var dtos = mapper.Map<IEnumerable<TodoDto>>(entities);
    return Results.Ok(dtos);
})
    .WithName("GetAllTodo")
    .WithOpenApi(op => { op.Summary = "Get all todo's"; return op; }); ;

// Get by id
app.MapGet("/api/todos/{id:int}", async (int id, ITodoService service, IMapper mapper) =>
{
    var entity = await service.GetByIdAsync(id);
    if (entity is null) return Results.NotFound();
    return Results.Ok(mapper.Map<TodoDto>(entity));
})
    .WithName("GetTodoById")
    .WithOpenApi(op => { op.Summary = "Get todo by ID"; return op; }); ;

// Create
app.MapPost("/api/todos", async (TodoCreateDto dto, ITodoService service, IMapper mapper) =>
{
    var created = await service.CreateAsync(dto);
    var result = mapper.Map<TodoDto>(created);
    return Results.Created($"/api/todos/{result.Id}", result);
})
    .AddEndpointFilter<ValidationFilter<TodoCreateDto>>()
    .WithName("CreateTodo")
    .WithOpenApi(op => { op.Summary = "Create a new todo"; return op; }); ;

// Update (returns updated DTO)
app.MapPut("/api/todos/{id:int}", async (int id, TodoUpdateDto dto, ITodoService service, IMapper mapper) =>
{
    var updated = await service.UpdateAsync(id, dto);
    if (updated is null) return Results.NotFound();
    return Results.Ok(mapper.Map<TodoDto>(updated));
})
    .AddEndpointFilter<ValidationFilter<TodoUpdateDto>>()
    .WithName("UpdateTodo")
    .WithOpenApi(op => { op.Summary = "Update an existing todo"; return op; });

// Set percent (returns updated DTO)
app.MapPatch("/api/todos/{id:int}/percent", async (int id, SetPercentDto dto, ITodoService service, IMapper mapper) =>
{
    var updated = await service.SetPercentCompleteAsync(id, dto.PercentComplete);
    if (updated is null) return Results.NotFound();
    return Results.Ok(mapper.Map<TodoDto>(updated));
})
.AddEndpointFilter<ValidationFilter<SetPercentDto>>()
.WithName("SetPercentTodo")
.WithOpenApi(op => { op.Summary = "Set todo percent complete"; return op; });

// Mark done (returns updated DTO)
app.MapPost("/api/todos/{id:int}/done", async (int id, ITodoService service, IMapper mapper) =>
{
    var updated = await service.MarkDoneAsync(id);
    if (updated is null) return Results.NotFound();
    return Results.Ok(mapper.Map<TodoDto>(updated));
})
    .WithName("markTodoDone")
    .WithOpenApi(op => { op.Summary = "Mark todo as done"; return op; });

// Delete (bool -> 204/404)
app.MapDelete("/api/todos/{id:int}", async (int id, ITodoService service) =>
{
    var ok = await service.DeleteAsync(id);
    return ok ? Results.NoContent() : Results.NotFound();
})
    .WithName("DeleteTodo")
    .WithOpenApi(op => { op.Summary = "Delete todo"; return op; });

// Incoming Todo's
app.MapGet("/api/todos/incoming", async (IncomingRange range, ITodoService service, IMapper mapper) =>
{
    var entities = await service.GetIncomingAsync(range);
    var dtos = mapper.Map<IEnumerable<TodoDto>>(entities);
    return Results.Ok(dtos);
})
.WithName("GetIncomingTodos")
.WithOpenApi(op =>
{
    op.Summary = "Get incoming todos";
    op.Description = "Range: Today, Tomorrow, Week (Mon–Sun, UTC).";
    return op;
});

app.Run();
public partial class Program { }
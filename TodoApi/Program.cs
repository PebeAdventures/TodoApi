using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Interfaces;
using TodoApi.Mapping;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Services;
using TodoApi.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
//Connection string
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Automapper registration
builder.Services.AddAutoMapper(typeof(MappingProfile));

//FluentValidation registration
builder.Services.AddValidatorsFromAssemblyContaining<TodoCreateDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TodoUpdateDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Apply pending ef Core migrations at startup
using (var scope = app.Services.CreateScope())
{
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
});

// Get by id
app.MapGet("/api/todos/{id:int}", async (int id, ITodoService service, IMapper mapper) =>
{
    var entity = await service.GetByIdAsync(id);
    if (entity is null) return Results.NotFound();
    return Results.Ok(mapper.Map<TodoDto>(entity));
});

// Create
app.MapPost("/api/todos", async (TodoCreateDto dto, ITodoService service, IMapper mapper) =>
{
    var created = await service.CreateAsync(dto);
    var result = mapper.Map<TodoDto>(created);
    return Results.Created($"/api/todos/{result.Id}", result);
});

// Update (returns updated DTO)
app.MapPut("/api/todos/{id:int}", async (int id, TodoUpdateDto dto, ITodoService service, IMapper mapper) =>
{
    var updated = await service.UpdateAsync(id, dto);
    if (updated is null) return Results.NotFound();
    return Results.Ok(mapper.Map<TodoDto>(updated));
});

// Set percent (returns updated DTO)
app.MapPatch("/api/todos/{id:int}/percent", async (int id, SetPercentDto dto, ITodoService service, IMapper mapper) =>
{
    var updated = await service.SetPercentCompleteAsync(id, dto.PercentComplete);
    if (updated is null) return Results.NotFound();
    return Results.Ok(mapper.Map<TodoDto>(updated));
});

// Mark done (returns updated DTO)
app.MapPost("/api/todos/{id:int}/done", async (int id, ITodoService service, IMapper mapper) =>
{
    var updated = await service.MarkDoneAsync(id);
    if (updated is null) return Results.NotFound();
    return Results.Ok(mapper.Map<TodoDto>(updated));
});

// Delete (bool -> 204/404)
app.MapDelete("/api/todos/{id:int}", async (int id, ITodoService service) =>
{
    var ok = await service.DeleteAsync(id);
    return ok ? Results.NoContent() : Results.NotFound();
});

app.Run();

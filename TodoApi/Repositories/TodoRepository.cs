using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoDbContext _context;

    public TodoRepository(TodoDbContext context) => _context = context;
    public async Task<Todo> AddAsync(Todo entity)
    {
        _context.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Todos.FindAsync(id);
        if( existing is null ) return false;
        _context.Todos.Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Todo>> GetAllAsync()
    {
        return await _context.Todos.AsNoTracking().ToListAsync();
    }

    public async Task<Todo?> GetByIdAsync(int id)
    {
        return await _context.Todos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Todo>> GetIncomingAsync(DateTime start, DateTime end)
    {
        return await _context.Todos.AsNoTracking()
            .Where(t => t.DueAt >= start && t.DueAt < end)
            .ToListAsync();
    }

    public async Task<Todo?> MarkDoneAsync(int id)
    {
        var entity = await _context.Todos.FindAsync(id);
        if (entity is null) return null;
        entity.IsDone = true;
        entity.PercentComplete = 100;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Todo?> SetPercentCompleteAsync(int id, int percentComplete)
    {
        var entity = await _context.Todos.FindAsync(id);
        if (entity is null) return null;
        entity.PercentComplete = percentComplete;
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Todo?> UpdateAsync(Todo entity)
    {
        var existing = await _context.Todos.FindAsync(entity.Id);
        if (existing is null) return null;
        _context.Entry(existing).CurrentValues.SetValues(entity);
        await _context.SaveChangesAsync();
        return existing;
    }
}

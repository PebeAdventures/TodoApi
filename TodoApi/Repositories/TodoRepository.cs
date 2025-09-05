using TodoApi.Data;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoDbContext _context;

    public TodoRepository(TodoDbContext context)
    {
        _context = context;
    }
    public Task<Todo> AddAsync(Todo entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Todo>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Todo?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Todo>> GetIncomingAsync(DateTime start, DateTime end)
    {
        throw new NotImplementedException();
    }

    public Task<Todo?> MarkDoneAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Todo?> SetPercentCompleteAsync(int id, int percentComplete)
    {
        throw new NotImplementedException();
    }

    public Task<Todo?> UpdateAsync(Todo entity)
    {
        throw new NotImplementedException();
    }
}

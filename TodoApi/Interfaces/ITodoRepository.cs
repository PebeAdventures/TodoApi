using TodoApi.Models;

namespace TodoApi.Interfaces;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<Todo?> GetByIdAsync(int id);
    Task<IEnumerable<Todo>> GetIncomingAsync(DateTime start, DateTime end);
    Task<Todo> AddAsync(Todo entity);
    Task<Todo?> UpdateAsync(Todo entity);
    Task<bool> DeleteAsync(int id);
    Task<Todo?> SetPercentCompleteAsync(int id, int percentComplete);
    Task<Todo?> MarkDoneAsync(int id);
}

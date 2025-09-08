using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Interfaces;

public interface ITodoService
{
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<Todo?> GetByIdAsync(int id);
    Task<IEnumerable<Todo>> GetIncomingAsync(IncomingRange range);
    Task<Todo> CreateAsync(TodoCreateDto dto);
    Task<Todo?> UpdateAsync(int id, TodoUpdateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<Todo?> SetPercentCompleteAsync(int id, int percentComplete);
    Task<Todo?> MarkDoneAsync(int id);
}

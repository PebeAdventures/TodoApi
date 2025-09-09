using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Interfaces;

public interface ITodoService
{
    /// <summary>
    /// Retrieves all todos as domain entities. Use DTO mapping at the API boundary.
    /// </summary>
    Task<IEnumerable<Todo>> GetAllAsync();
    /// <summary>
    /// Retrieves a todo by its identifier in read-only mode. Returns null if not found.
    /// </summary>
    Task<Todo?> GetByIdAsync(int id);
    /// <summary>
    /// Retrieves todos scheduled for a predefined incoming range (Today/Tomorrow/Week).
    /// Week is the current calendar week (Mon–Sun), computed in UTC.
    /// </summary>
    Task<IEnumerable<Todo>> GetIncomingAsync(IncomingRange range);
    /// <summary>
    /// Creates a new todo from the provided DTO and returns the created entity.
    /// </summary>
    Task<Todo> CreateAsync(TodoCreateDto dto);
    /// <summary>
    /// Updates an existing todo by id using the provided DTO.
    /// Returns the updated entity, or null when the entity does not exist.
    /// </summary>
    Task<Todo?> UpdateAsync(int id, TodoUpdateDto dto);
    /// <summary>
    /// Deletes a todo by id. Returns true when the entity existed and was removed; otherwise false.
    /// </summary>
    Task<bool> DeleteAsync(int id);
    /// <summary>
    /// Sets the PercentComplete value (0..100) and returns the updated entity, or null when not found.
    /// </summary>
    Task<Todo?> SetPercentCompleteAsync(int id, int percentComplete);
    /// <summary>
    /// Marks a todo as done (IsDone = true, PercentComplete = 100)
    /// and returns the updated entity, or null when not found.
    /// </summary>
    Task<Todo?> MarkDoneAsync(int id);
}

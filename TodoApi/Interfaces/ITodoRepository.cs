using TodoApi.Models;

namespace TodoApi.Interfaces;

public interface ITodoRepository
{
    /// <summary>
    /// Returns all todos in read-only mode (no tracking).
    /// </summary>
    Task<IEnumerable<Todo>> GetAllAsync();
    /// <summary>
    /// Returns a single todo by id in read-only mode (no tracking). Returns null when not found.
    /// </summary>
    Task<Todo?> GetByIdAsync(int id);
    /// <summary>
    /// Returns todos with DueAt in the half-open interval [fromInclusive, toExclusive] in UTC.
    /// </summary>
    Task<IEnumerable<Todo>> GetIncomingAsync(DateTime start, DateTime end);
    /// <summary>
    /// Adds a new todo and persists it.
    /// </summary>
    Task<Todo> AddAsync(Todo entity);
    /// <summary>
    /// Persists changes to an existing tracked entity.
    /// </summary>
    Task<Todo?> UpdateAsync(Todo entity);
    /// <summary>
    /// Removes the given entity.
    /// </summary>
    Task<bool> DeleteAsync(int id);
    /// <summary>
    /// Set percent complete for given entity.
    /// </summary>
    Task<Todo?> SetPercentCompleteAsync(int id, int percentComplete);
    /// <summary>
    /// Mark given entity as done.
    /// </summary>
    Task<Todo?> MarkDoneAsync(int id);
}

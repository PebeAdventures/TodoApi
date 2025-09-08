using AutoMapper;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Services;

public class TodoService : ITodoService
{

    private readonly ITodoRepository _repository;
    private readonly IMapper _mapper;

    public TodoService(ITodoRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Todo> CreateAsync(TodoCreateDto dto)
    {
        var entity = _mapper.Map<Todo>(dto);
        entity.PercentComplete = 0;
        entity.IsDone = false;
        var result = await _repository.AddAsync(entity);
        return result;
    }

    public async Task<bool> DeleteAsync(int id) => await _repository.DeleteAsync(id);

    public async Task<IEnumerable<Todo>> GetAllAsync() => await _repository.GetAllAsync(); 

    public async Task<Todo?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<IEnumerable<Todo>> GetIncomingAsync(IncomingRange range)
    {
        var now = DateTime.UtcNow;

        switch (range)
        {
            case IncomingRange.Today:
                {
                    var from = now.Date;
                    var to = from.AddDays(1);
                    return await _repository.GetIncomingAsync(from, to);
                }

            case IncomingRange.Tomorrow:
                {
                    var from = now.Date.AddDays(1);
                    var to = from.AddDays(1);
                    return await _repository.GetIncomingAsync(from, to);
                }

            case IncomingRange.Week:
                {
                    var dow = (int)now.DayOfWeek;
                    var monday = now.Date.AddDays(dow == 0 ? -6 : -(dow - 1));
                    var nextMonday = monday.AddDays(7);
                    return await _repository.GetIncomingAsync(monday, nextMonday);
                }

            default:
                throw new ArgumentOutOfRangeException(nameof(range));
        }
    }

    public async Task<Todo?> MarkDoneAsync(int id) => await _repository.MarkDoneAsync(id);

    public async Task<Todo?> SetPercentCompleteAsync(int id, int percentComplete)
    {
        return await _repository.SetPercentCompleteAsync(id, percentComplete);
    }

    public async Task<Todo?> UpdateAsync(int id, TodoUpdateDto dto)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing is null) return null;
        existing.Title = dto.Title;
        existing.Description = dto.Description;
        existing.DueAt = dto.DueAt;
        existing.PercentComplete = dto.PercentComplete;
        existing.IsDone = dto.IsDone;
        return await _repository.UpdateAsync(existing);
    }
}

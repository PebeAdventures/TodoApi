using AutoMapper;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Todo, TodoDto>();
        CreateMap<TodoCreateDto, Todo>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.PercentComplete, opt => opt.MapFrom(_ => 0))
            .ForMember(d => d.IsDone, opt => opt.MapFrom(_ => false));

        CreateMap<TodoUpdateDto, Todo>()
            .ForMember(d => d.Id, opt => opt.Ignore());
    }
}

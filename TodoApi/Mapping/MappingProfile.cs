using AutoMapper;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Todo, TodoDto>().ReverseMap();
        CreateMap<TodoCreateDto, Todo>();
        CreateMap<TodoUpdateDto, Todo>();
    }
}

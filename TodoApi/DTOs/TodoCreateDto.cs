namespace TodoApi.DTOs;

public class TodoCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueAt { get; set; }
}

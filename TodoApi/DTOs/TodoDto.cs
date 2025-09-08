namespace TodoApi.DTOs;

public class TodoDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DueAt { get; set; }
    public int PercentComplete { get; set; }
    public bool IsDone { get; set; }
}

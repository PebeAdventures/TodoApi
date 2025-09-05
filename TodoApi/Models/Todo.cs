namespace TodoApi.Models;

public class Todo
{
    public int Id { get; set; }
    /// <summary>
    ///  Task Title. Required field, max 200 chars.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Detailed task description. Optional field, max 1000 chars.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Date and time when task should be completed.
    /// </summary>
    public DateTime DueAt { get; set; }
    /// <summary>
    /// Task completion percentage (0–100). Default value is 0.
    /// </summary>
    public int PercentComplete { get; set; } = 0;
    /// <summary>
    /// Completion flag, true when done. 
    /// </summary>
    public bool IsDone { get; set; } = false;

}

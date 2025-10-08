using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE.Models;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }

    [Required] 
    public string Title { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    [Required]
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public ICollection<ProjectTask>? ProjectTasks { get; set; }
    
    
}
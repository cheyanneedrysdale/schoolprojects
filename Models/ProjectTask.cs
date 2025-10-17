using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace COMP2139_ICE.Models;

public class ProjectTask
{
    
    public int ProjectTaskId { get; set; }
    

    [Required] 
    public required string Title { get; set; } = string.Empty;
    
    
    
    [Required]
    public required string? Description { get; set; }
    
    
    
    public int ProjectId { get; set; }
    
    
    
    public Project? Project { get; set; }
    
    
    
    
    
    public ICollection<ProjectTask>? ProjectTasks { get; set; }
    
    
}
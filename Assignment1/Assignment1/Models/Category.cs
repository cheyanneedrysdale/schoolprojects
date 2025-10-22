using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models;

public class Category
{
    public int CategoryId { get; set; }

    [Required]
    [Display(Name = "Category Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Description")]
    public string? Description { get; set; }
    
    public ICollection<Event> Events { get; set; } = new List<Event>();
}
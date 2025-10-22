using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models;

public class Event
{
    public int EventId { get; set; }
    

    [Required]
    [Display(Name = "Event Title")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Description")] public string Description { get; set; } = string.Empty;


    public List<PurchaseItem> PurchaseItems { get; set; } = new();
    

    [Required]
    public int CategoryId { get; set; }
    
    
    public Category? Category { get; set; }

    // stores event date + time
    [Required]
    [Display(Name = "Starts At") ]
    public DateTime StartsAt { get; set; }


    [Required]
    [Display(Name = "Ticket Price") ]
    public decimal TicketPrice { get; set; }
    
    [Required]
    [Display(Name = "Available Tickets") ]
    public int AvailableTickets { get; set; }


}
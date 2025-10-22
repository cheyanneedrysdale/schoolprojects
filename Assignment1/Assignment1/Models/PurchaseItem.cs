using System.ComponentModel.DataAnnotations;

namespace Assignment1.Models
{
    public class PurchaseItem
    
    {
        
        // foreign key: Purchase
        [Required]
        public int PurchaseId { get; set; }
        
        public Purchase? Purchase { get; set; }

        
        // foreign key: Event
        [Required]
        public int EventId { get; set; }
        
        public Event? EventInfo { get; set; }
        
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "must be 1 or greater!")]
        public int Quantity { get; set; }
        

        // price (at time of purchase)
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "must be 0 or greater!")]
        [Display(Name = "unit price")]
        public decimal UnitPrice { get; set; }
    }
}
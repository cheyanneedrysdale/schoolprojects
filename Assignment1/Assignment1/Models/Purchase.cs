using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment1.Models
{
    public class Purchase
    {

        public int PurchaseId { get; set; }
        

        [Required]
        [Display(Name = "purchase date")]
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
        

        [Required]
        [Display(Name = "guest name")]
        public string GuestName { get; set; } = string.Empty;
        

        [Required]
        [EmailAddress]
        [Display(Name = "guest email")]
        public string GuestEmail { get; set; } = string.Empty;
        

        [Display(Name = "total cost")]
        [Range(0, double.MaxValue, ErrorMessage = "must be greater than 0!")]
        public decimal TotalCost { get; set; }
        

        public List<PurchaseItem> Items { get; set; } = new();
        
        
    }
}
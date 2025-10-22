namespace Assignment1.Models
{
    public class Dashboard
    {
        public int TotalEvents { get; set; }
        
        public int TotalCategories { get; set; }
        
        public int LowTicketThreshold { get; set; } = 5;
        
        public int LowTicketCount { get; set; }
        
        public List<Event> LowTicketEvents { get; set; } = new();
        
    }
}
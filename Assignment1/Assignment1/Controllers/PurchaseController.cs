using Assignment1.Data;
using Assignment1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Assignment1.Controllers
{
    public class PurchaseController : Controller
    
    {
        private readonly ApplicationDbContext _db;
        
        public PurchaseController(ApplicationDbContext db) => _db = db;
        
        

        // dropdown of events
        private async Task PopulateEventsAsync(int? selectedId = null)
        {
            var events = await _db.Events
                    
                .Where(e => e.AvailableTickets > 0)
                
                .OrderBy(e => e.Title)
                
                .Select(e => new { e.EventId, Title = e.Title + " — " + e.StartsAt.ToString("yyyy-MM-dd HH:mm") })
                
                .ToListAsync();

            ViewBag.EventId = new SelectList(events, "EventId", "Title", selectedId);
        }

        // continue as user or guest?
        [HttpGet]
        public IActionResult UserOrGuest(int? eventId)
        {
            ViewBag.EventId = eventId;
            return View();
        }

        // guest
        [HttpGet]
        public IActionResult GuestPurchase(int? eventId)
        {
            ViewBag.EventId = eventId;
            return View();
        }

        //purchase/create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateEventsAsync();
            return View(new Purchase()); 
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Purchase form, int eventId, int quantity)
        {

            // checks qty is 1 or greater
            if (quantity < 1)
                ModelState.AddModelError(nameof(quantity), "qty must be 1 or greater!");
            


            // checks if event exists
            var ev = await _db.Events.FindAsync(eventId);
            if (ev == null)
                ModelState.AddModelError(nameof(eventId), "no event found!");

            // seat/ticket validation
            if (ev != null && ev.AvailableTickets < quantity)
                ModelState.AddModelError(nameof(quantity),$"only {ev.AvailableTickets} ticket(s) left for {ev.Title}!");
            

            
            if (!ModelState.IsValid)
            {
                await PopulateEventsAsync(eventId);
                return View(form);
            }


            // guest purchase
            var purchase = new Purchase
            {
                GuestName = form.GuestName,
                GuestEmail = form.GuestEmail,
                PurchaseDate = DateTime.UtcNow
            };

            var item = new PurchaseItem
            {
                Purchase = purchase,           
                EventId = ev!.EventId,         
                Quantity = quantity,
                UnitPrice = ev.TicketPrice     
            };

            purchase.Items.Add(item);

            // calculates total cost, modifies available tickets
            purchase.TotalCost = item.UnitPrice * item.Quantity;
            ev.AvailableTickets -= quantity;
            
            // saves changes
            _db.Purchases.Add(purchase);
            await _db.SaveChangesAsync();
            
            
            return RedirectToAction(nameof(Confirmation), new { id = purchase.PurchaseId });
        }
        
        
        [HttpGet]
        public async Task<IActionResult> Confirmation(int id)
        {
            var purchase = await _db.Purchases
                    
                .Include(p => p.Items)
                
                .ThenInclude(i => i.EventInfo)   
                
                .FirstOrDefaultAsync(p => p.PurchaseId == id);

            if (purchase == null)
            {
                return NotFound();
            }
            
            return View(purchase);
        }
        
    }
}
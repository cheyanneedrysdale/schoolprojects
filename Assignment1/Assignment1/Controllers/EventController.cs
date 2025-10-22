
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;             
using Assignment1.Data;
using Assignment1.Models;

namespace Assignment1.Controllers
{
    public class EventController : Controller
    {
        private readonly ApplicationDbContext _db;
        public EventController(ApplicationDbContext db) => _db = db;






        private async Task PopulateCategoriesAsync(int? selectedId = null)
        {
            var cats = await _db.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();
            ViewBag.CategoryId = new SelectList(cats, "CategoryId", "Name", selectedId);
        }


        // ************** INDEX **************  //
        [HttpGet]
        public async Task<IActionResult> Index(string? search, int? categoryId, DateTime? from, DateTime? to,
            string? sort, string? availability)
        {

            // ************** LIST **************  //

            IQueryable<Event> query = _db.Events
                .Include(e => e.Category)
                .AsQueryable();


            // ************** FILTER **************  //

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(e => e.Title.Contains(search));

            if (categoryId.HasValue)
                query = query.Where(e => e.CategoryId == categoryId.Value);

            if (from.HasValue)
                query = query.Where(e => e.StartsAt >= from.Value);

            if (to.HasValue)
                query = query.Where(e => e.StartsAt <= to.Value);

            // ************** AVAILABILITY **************  //

            if (!string.IsNullOrWhiteSpace(availability))

            {
                if (availability == "available")
                    query = query.Where(e => e.AvailableTickets > 0);

                else if (availability == "soldout")
                    query = query.Where(e => e.AvailableTickets == 0);
            }


            // ************** SORT **************  //

            query = sort switch

            {

                "title_asc" => query.OrderBy(e => e.Title),
                "title_desc" => query.OrderByDescending(e => e.Title),


                "date_desc" => query.OrderByDescending(e => e.StartsAt),

                "price_asc" => query.OrderBy(e => e.TicketPrice),

                "price_desc" => query.OrderByDescending(e => e.TicketPrice),

                _ => query.OrderBy(e => e.StartsAt)

            };

            // ************** OUTPUT **************  //

            ViewBag.Search = search;

            ViewBag.From = from?.ToString("yyyy-MM-dd");

            ViewBag.To = to?.ToString("yyyy-MM-dd");

            ViewBag.Sort = sort ?? "date_asc";

            await PopulateCategoriesAsync(categoryId);

            var events = await query.ToListAsync();
            {
                return View(events);
            }
        }




        // ************** CREATE **************  //

        // loads create form, populates dropdown menu w/ exisiting categories
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await PopulateCategoriesAsync();

            return View();
        }


        // checks if input passes validation rules
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesAsync();
                {
                    return View(model);
                }
            }

            _db.Events.Add(model);

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        // ************** DETAILS **************  //    

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var ev = await _db.Events

                .Include(e => e.Category)

                .FirstOrDefaultAsync(e => e.EventId == id);

            if (ev == null) return NotFound();
            {
                return View(ev);
            }
        }



        // ************** EDIT **************  //

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ev = await _db.Events.FindAsync(id);

            if (ev == null)
            {
                return NotFound();
            }

            await PopulateCategoriesAsync(ev.CategoryId);
            return View(ev);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Event model)
        {
            if (id != model.EventId) return BadRequest();
            
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesAsync();
                
                return View(model);
            }

            _db.Update(model);
            
            await _db.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }


// ************** DELETE *************  //

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _db.Events
                    
                .Include(e => e.Category)
                
                .FirstOrDefaultAsync(e => e.EventId == id);
            

            if (ev == null) return NotFound();
            {
                return View(ev);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ev = await _db.Events.FindAsync(id);

            if (ev != null)

            {
                _db.Events.Remove(ev);
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        // ************** DASHBOARD ************** //

        [HttpGet]
        public async Task<IActionResult> Dashboard(int? threshold)
        {
            int low = threshold ?? 5;

            
            int totalEvents = await _db.Events.CountAsync();
            int totalCategories = await _db.Categories.CountAsync();

            
            var lowEvents = await _db.Events
                    
                .Include(e => e.Category)
                
                .Where(e => e.AvailableTickets < low)
                
                .OrderBy(e => e.AvailableTickets)
                
                .ThenBy(e => e.StartsAt)
                
                .ToListAsync();


            ViewBag.Events = await _db.Events
                .Include(e => e.Category)
                .OrderBy(e => e.StartsAt)
                .ToListAsync();

            ViewBag.Categories = await _db.Categories
                .Include(c => c.Events)
                .OrderBy(c => c.Name)
                .ToListAsync();


            var model = new Assignment1.Models.Dashboard
            {
                TotalEvents = totalEvents,
                TotalCategories = totalCategories,
                LowTicketThreshold = low,
                LowTicketCount = lowEvents.Count,
                LowTicketEvents = lowEvents
            };

            return View(model);
        }

    }
}
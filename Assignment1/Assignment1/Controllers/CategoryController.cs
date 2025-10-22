using Assignment1.Data;
using Assignment1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Assignment1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db) => _db = db;


        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Dashboard", "Event");
        }



        // ---------- CREATE ----------

        [HttpGet]
        public IActionResult Create() => View();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.Categories.Add(model);
            await _db.SaveChangesAsync();

            // back to dashboard after create
            return RedirectToAction("Dashboard", "Event");
        }



        // ---------- DETAILS ----------

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _db.Categories
                .Include(c => c.Events)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) return NotFound();
            return View(category);
        }



        // ---------- EDIT ----------

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model)
        {
            if (id != model.CategoryId) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            _db.Update(model);
            await _db.SaveChangesAsync();

            // back to dashboard after edit
            return RedirectToAction("Dashboard", "Event");
        }



        // ---------- DELETE ----------

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _db.Categories
                .Include(c => c.Events)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null) return NotFound();
            return View(category);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category != null)
            {
                _db.Categories.Remove(category);
                await _db.SaveChangesAsync();
            }


            return RedirectToAction("Dashboard", "Event");
        }
    }
}
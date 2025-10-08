using COMP2139_ICE.Data;             
using COMP2139_ICE.Models;            
using Microsoft.AspNetCore.Mvc;        
using Microsoft.EntityFrameworkCore;   

namespace COMP2139_ICE.Controllers;

public class ProjectTaskController : Controller
{
    private readonly ApplicationDbContext _context;
    public ProjectTaskController(ApplicationDbContext context) => _context = context;

    
    public async Task<IActionResult> Index(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null) return NotFound();

        ViewBag.ProjectId = projectId;
        ViewBag.ProjectName = project.Name;

        var tasks = await _context.ProjectTasks
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();

        return View(tasks);
    }

    
    public async Task<IActionResult> Details(int id)
    {
        var task = await _context.ProjectTasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

        if (task == null) return NotFound();
        return View(task);
    }

    
    [HttpGet]
    public async Task<IActionResult> Create(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null) return NotFound();

        ViewBag.ProjectName = project.Name;
        return View(new ProjectTask { ProjectId = projectId });
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProjectTask task)
    {
        if (!ModelState.IsValid) return View(task);

        _context.ProjectTasks.Add(task);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
    }

    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var task = await _context.ProjectTasks.FindAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProjectTask task)
    {
        if (id != task.ProjectTaskId) return NotFound();
        if (!ModelState.IsValid) return View(task);

        _context.Update(task);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
    }

    
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.ProjectTasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.ProjectTaskId == id);
        if (task == null) return NotFound();
        return View(task);
    }

   
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await _context.ProjectTasks.FindAsync(id);
        if (task == null) return NotFound();

        var projectId = task.ProjectId;
        _context.ProjectTasks.Remove(task);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index), new { projectId });
    }
}
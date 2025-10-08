using System.Net.Mime;
using System.Runtime.InteropServices.JavaScript;
using COMP2139_ICE.Models;
using Microsoft.AspNetCore.Mvc;
using COMP2139_ICE.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Controllers;

public class ProjectController : Controller

{

    private readonly ApplicationDbContext _context;

    public ProjectController(ApplicationDbContext context)
    {
        _context = context;
    }
    

    public async Task<IActionResult> Index()
    {
        var projects = _context.Projects.ToList();
        return View(projects);
    }


    [HttpGet]
    public IActionResult Create() => View();
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Project project)
    {
        if (ModelState.IsValid)
        {
            if (project.StartDate.HasValue)
                project.StartDate = DateTime.SpecifyKind(project.StartDate.Value, DateTimeKind.Utc);

            if (project.EndDate.HasValue)
                project.EndDate = DateTime.SpecifyKind(project.EndDate.Value, DateTimeKind.Utc);

            _context.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        return View(project);
    }
    [HttpGet]
    public IActionResult Details(int id)
{
    var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)  return NotFound();
        return View(project);
    }

[HttpGet]
public IActionResult Edit(int? id)
{
    if (id == null) return NotFound();
    var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id.Value);
    if (project == null) return NotFound();
    return View(project);
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, Project project)
{
    if (id != project.ProjectId) return NotFound();
    if (!ModelState.IsValid) return View(project);

    if (project.StartDate.HasValue)
        project.StartDate = DateTime.SpecifyKind(project.StartDate.Value, DateTimeKind.Utc);
    if (project.EndDate.HasValue)
        project.EndDate = DateTime.SpecifyKind(project.EndDate.Value, DateTimeKind.Utc);
    
    _context.Update(project);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}

[HttpGet]
public IActionResult Delete(int? id)
{
    if (id == null) return NotFound();
    var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id.Value);
    if (project == null) return NotFound();
    return View(project);

}

[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
    if (project != null)
    {
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

    return RedirectToAction(nameof(Index));
}

}
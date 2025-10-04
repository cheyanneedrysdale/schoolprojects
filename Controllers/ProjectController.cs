using System.Net.Mime;
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
    

    public IActionResult Index()
    {
        var projects = _context.Projects.ToList();
        return View(projects);
    }


    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(Project project)
{
    if (!ModelState.IsValid)
        return View(project);
    
    _context.Projects.Add(project);
    _context.SaveChanges();
    return RedirectToAction("Index");
}



public IActionResult Details(int id)

{
    var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }

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
public IActionResult Edit(int id, Project project)
{
    if (id != project.ProjectId) return NotFound();
    if (!ModelState.IsValid)
    {
        return View(project);
    }

    _context.Update(project);
    _context.SaveChanges();
    return RedirectToAction(nameof(Index));
}

[HttpGet]
[ValidateAntiForgeryToken]
public IActionResult Delete(int? id)
{
    if (id == null) return NotFound();

    var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id.Value);
    if (project == null) return NotFound();

    return View(project);

}

[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult DeleteConfirmed(int id)
{
    var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
    if (project != null)
    {
        _context.Projects.Remove(project);
        _context.SaveChanges();
    }

    return RedirectToAction(nameof(Index));
}

}
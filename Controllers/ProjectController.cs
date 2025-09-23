using System.Net.Mime;
using COMP2139_ICE.Models;
using Microsoft.AspNetCore.Mvc;
using COMP2139_ICE.Data;
using Microsoft.AspNetCore.Http.HttpResults;

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
    

[HttpPost]
public IActionResult Create(Project project)
{
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


}
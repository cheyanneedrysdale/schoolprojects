using COMP2139_ICE.Models;
using Microsoft.AspNetCore.Mvc;

namespace COMP2139_ICE.Controllers;

public class ProjectController : Controller

{


private static List<Project> projects = new List<Project>();

    public IActionResult Index()

    {

        if (projects.Count == 0)
        {
            projects.Add(new Project { ProjectId = 1, Name = "Project 1", Description = "this is project 1." });
        }

// add more projects here


        return View(projects);

    }

    public IActionResult Create()
    {
        return View();
    }




    public IActionResult Create(Project project)

    {
        project.ProjectId = projects.Count + 1;
        projects.Add(project);

        return RedirectToAction("Index");

    }

    

    public IActionResult Details(int id)

    {
        var project = projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);

    }


}
using Microsoft.AspNetCore.Mvc;

namespace COMP2139_ICE.Controllers;

public class Home1Controller : Controller
{
    // GET: Home1/Index
    
    public IActionResult Index()
    {
        return View();
    }
    // GET: /Home1/About
    
    public IActionResult About()
    {
        return View();
}
}
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Models;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaCA.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        var miembros = new List<Miembro>
            {
                new Miembro
                {
                    Nombre = "Paulo Cabrera",
                    Rol = "Desarrollador Backend",
                    Email = "paulocabrera90@gmail.com",
                    Telefono = "+54 9 266 474-5525",
                    FotoUrl = "/avatars/mario-arriola.png",
                    LinkedInUrl = "https://www.linkedin.com/in/ana-gomez/"
                },
                new Miembro
                {
                    Nombre = "Mario Arriola",
                    Rol = "Desarrollador Backend",
                    Email = "arriola.mario.90@gmail.com",
                    Telefono = "+54 9 266 461-4253",
                    FotoUrl = "/avatars/mario-arriola.png",
                    LinkedInUrl = "https://www.linkedin.com/in/arriola-mario-fabian/"
                }
            };

        return View(miembros);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

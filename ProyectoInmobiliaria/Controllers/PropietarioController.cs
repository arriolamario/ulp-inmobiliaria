using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.Models;
using ProyectoInmobiliaria.Repositorio;

namespace ProyectoInmobiliaria.Controllers;

public class PropietarioController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IConfiguration _Configuration { get; }

    public PropietarioController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _Configuration = configuration;
    }

    public IActionResult Index()
    {
        RepositorioPropietario repositorioPropietario = new RepositorioPropietario(_Configuration);
        return View(repositorioPropietario.GetPropietarios());
    }
    
}

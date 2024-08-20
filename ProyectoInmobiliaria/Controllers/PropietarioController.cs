using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.Models;
using ProyectoInmobiliaria.Repositorio;

namespace ProyectoInmobiliaria.Controllers;

public class PropietarioController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IConfiguration _Configuration;
    private RepositorioPropietario _repositorioPropietario;

    public PropietarioController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _Configuration = configuration;
        _repositorioPropietario = new RepositorioPropietario(_Configuration);
    }

    public IActionResult Index()
    {
        return View(_repositorioPropietario.GetPropietarios());
    }

    public IActionResult Detalle(int Id)
    {
        return View(_repositorioPropietario.GetPropietario(Id));
    }

    public IActionResult AltaEditar(int Id)
    {
        if (Id == 0)
            return View();

        return View(_repositorioPropietario.GetPropietario(Id));
    }

    [HttpPost]
    public IActionResult AltaEditar(Propietario propietario)
    {
        // if (ModelState.IsValid)
        // {
        if (propietario.Id == 0)
        {
            _repositorioPropietario.InsertarPropietario(propietario);
        }
        else
        {
            _repositorioPropietario.ActualizarPropietario(propietario);
        }
        return RedirectToAction("Index");
        // }
        // return View("AltaEditar", propietario);
    }

    public IActionResult Baja(int Id)
    {
        if (Id == 0)
        {
        }
        else
        {
            _repositorioPropietario.BajaPropietario(Id);
        }
        return RedirectToAction("Index");
    }
}

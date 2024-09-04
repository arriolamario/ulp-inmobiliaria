using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;

namespace InmobiliariaCA.Controllers;

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
        try {
             return View(_repositorioPropietario.GetPropietarios());
        } catch (Exception ex) {              
                _logger.LogError("An error occurred while getting propiertor: {Error}", ex.Message);
               
                TempData["ErrorMessage"] = "Error al cargar los propietarios. Por favor intente de nuevo más tarde.";
                return View(new List<Propietario>());
        }
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
        if (_repositorioPropietario.ExistePropietarioPorDni(propietario.Dni) && propietario.Id == 0)
        {
            ModelState.AddModelError(nameof(propietario.Dni), "Documento ya existe");
            TempData["ErrorMessage"] = "El propietario ya existe.";
            return View(propietario);
        }
        if (propietario.Id == 0)
        {
            _repositorioPropietario.InsertarPropietario(propietario);
            TempData["SuccessMessage"] = "Propietario agregado correctamente.";
        }
        else
        {
            _repositorioPropietario.ActualizarPropietario(propietario);
            TempData["SuccessMessage"] = "Propietario actualizado correctamente.";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Baja(int Id)
    {
        if (Id == 0)
        {
        }
        else
        {
            var res =_repositorioPropietario.BajaPropietario(Id);
            if (res)
                TempData["SuccessMessage"] = "Propietario dado de baja correctamente.";
            else
                TempData["ErrorMessage"] = "No se pudo dar de baja al propietario.";
        }
        return RedirectToAction("Index");
    }
}

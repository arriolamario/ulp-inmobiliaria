using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaCA.Controllers;
[Authorize]
public class PropietarioController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IRepositorioPropietario _repositorioPropietario;

    public PropietarioController(ILogger<HomeController> logger, IRepositorioPropietario repositorioPropietario)
    {
        _logger = logger;
        _repositorioPropietario = repositorioPropietario;
    }

    public IActionResult Index()
    {
        try
        {
            List<Propietario> listPropietarios = _repositorioPropietario.GetPropietarios();
            return View(listPropietarios);
        }
        catch (Exception ex)
        {
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
    [Authorize(Policy = "administrador")]
    public IActionResult Baja(int Id)
    {
        if (Id == 0)
        {
            TempData["ErrorMessage"] = "No se pudo dar de baja al propietario.";
        }
        else
        {
            var res = _repositorioPropietario.BajaPropietario(Id);
            if (res)
                TempData["SuccessMessage"] = "Propietario dado de baja correctamente.";
            else
                TempData["ErrorMessage"] = "No se pudo dar de baja al propietario.";
        }
        return RedirectToAction("Index");
    }
}

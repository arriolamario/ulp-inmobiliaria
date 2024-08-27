using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;

namespace InmobiliariaCA.Controllers;

public class InmuebleController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IConfiguration _Configuration;
    private RepositorioInmueble _repositorioInmueble;

    public InmuebleController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _Configuration = configuration;
        _repositorioInmueble = new RepositorioInmueble(_Configuration);
    }

    public IActionResult Index()
    {
        ViewBag.Mensaje = TempData["MensajeExito"] ?? TempData["MensajeError"];
        return View(_repositorioInmueble.GetInmuebles());
    }

    public IActionResult Detalle(int Id)
    {
        return View(_repositorioInmueble.GetInmueble(Id));
    }

    public IActionResult AltaEditar(int Id)
    {
        if (Id == 0)
            return View();

        return View(_repositorioInmueble.GetInmueble(Id));
    }

    [HttpPost]
    public IActionResult AltaEditar(Inmueble inmueble)
    {
        if (inmueble.Id == 0)
        {
            _repositorioInmueble.AltaInmueble(inmueble);
            TempData["MensajeExito"] = "Inmueble insertado correctamente";
        }
        else
        {
            _repositorioInmueble.ActualizarInmueble(inmueble);
            TempData["MensajeExito"] = "Inmueble actualizado correctamente";
        }
        return RedirectToAction("Index");
    }

    public IActionResult Baja(int Id)
    {
        if (Id == 0)
        {
        }
        else
        {
            _repositorioInmueble.BajaLogicaInmueble(Id);
        }
        return RedirectToAction("Index");
    }
}

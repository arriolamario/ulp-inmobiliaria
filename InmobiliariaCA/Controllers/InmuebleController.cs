using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        try {
            return View(_repositorioInmueble.GetInmuebles());
        } catch (Exception ex) {              
                _logger.LogError("An error occurred while getting property: {Error}", ex.Message);
               
                TempData["ErrorMessage"] = "Error al cargar los inmuebles. Por favor intente de nuevo más tarde.";
                return View(new List<Inmueble>());
        }
    }

    public IActionResult Detalle(int Id)
    {
        return View(_repositorioInmueble.GetInmueble(Id));
    }

    public IActionResult AltaEditar(int Id, int idPropietario)
    {
        var tiposInmuebles = _repositorioInmueble.GetTipoInmuebles();
        var tiposInmueblesUsos = _repositorioInmueble.GetTipoInmueblesUsos();
        ViewBag.TipoInmuebles = new SelectList(tiposInmuebles ?? new List<TipoInmueble>(), "Id", "Descripcion");
        ViewBag.TipoInmueblesUsos = new SelectList(tiposInmueblesUsos ?? new List<TipoInmuebleUso>(), "Id", "Descripcion");
        if (Id == 0)
        {
            ViewBag.idPropietario = idPropietario;
            return View(new Inmueble());
        }
        var inmueble = _repositorioInmueble.GetInmueble(Id);
        ViewBag.idPropietario = inmueble?.Id_Propietario;
        return View(inmueble);
    }

    [HttpPost]
    public IActionResult CrearActualizar(Inmueble inmueble)
    {
        if (inmueble.Id == 0)
        {
            _repositorioInmueble.AltaInmueble(inmueble);
            TempData["SuccessMessage"] = "Inmueble insertado correctamente";
        }
        else
        {
            _repositorioInmueble.ActualizarInmueble(inmueble);
            TempData["SuccessMessage"] = "Inmueble actualizado correctamente";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult BajaLogica(int Id)
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

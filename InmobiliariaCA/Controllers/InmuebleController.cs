using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using InmobiliariaCA.Models.ContratoModels;

namespace InmobiliariaCA.Controllers;

[Authorize]
public class InmuebleController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IRepositorioInmueble _repositorioInmueble;
    private IRepositorioContrato _repositorioContrato;

    public InmuebleController(ILogger<HomeController> logger, 
                        IRepositorioInmueble repositorioInmueble,
                        IRepositorioContrato repositorioContrato)
    {
        _logger = logger;
        _repositorioInmueble = repositorioInmueble;
        _repositorioContrato = repositorioContrato;
    }

    public IActionResult Index([FromQuery] bool? Activo)
    {
        try {
            if (Activo.HasValue){
                ViewBag.SelectActivo = Activo.Value ? "Activos" : "Inactivos";
                return View(_repositorioInmueble.GetInmuebles(Activo.Value));
            }
            ViewBag.SelectActivo = "Todos";
            return View(_repositorioInmueble.GetInmuebles());
        } catch (Exception ex) {              
                _logger.LogError("An error occurred while getting property: {Error}", ex.Message);
               
                TempData["ErrorMessage"] = "Error al cargar los inmuebles. Por favor intente de nuevo más tarde.";
                return View(new List<Inmueble>());
        }
    }

    public IActionResult Detalle(int Id)
    {
        Inmueble? inmueble =_repositorioInmueble.GetInmueble(Id, null);
        if(inmueble == null){
            return NotFound();
        }
        List<Contrato> contratos = _repositorioContrato.GetContratos(inmueble.Id);

        InmuebleViewModel inmuebleViewModel = new InmuebleViewModel(inmueble, contratos);
        return View(inmuebleViewModel);
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
        var inmueble = _repositorioInmueble.GetInmueble(Id, null);
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
    [Authorize(Policy = "administrador")]
    public IActionResult BajaLogica(int Id)
    {
        if (Id == 0)
        {
        }
        else
        {
            if(_repositorioInmueble.BajaInmueble(Id)){
                TempData["SuccessMessage"] = "Se elimino correctamente el inmueble";
            }
            else{
                TempData["ErrorMessage"] = "No se puede eliminar el inmueble";
            }
        }
        return RedirectToAction("Index");
    }
}

using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InmobiliariaCA.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IRepositorioUsuario _repositorioUsuario;

    public UsuarioController(ILogger<HomeController> logger,
                        IRepositorioUsuario repositorioUsuario)
    {
        _logger = logger;
        _repositorioUsuario = repositorioUsuario;
    }

    public IActionResult Index()
    {
        return View(_repositorioUsuario.GetUsuarios());
    }

    public IActionResult Detalle(int Id)
    {
        var usuario = _repositorioUsuario.GetUsuario(Id);
        if(usuario == null) {
            return NotFound();
        }
        return View(usuario);
    }

    // public IActionResult AltaEditar(int Id, int idPropietario)
    // {
    //     var tiposInmuebles = _repositorioInmueble.GetTipoInmuebles();
    //     var tiposInmueblesUsos = _repositorioInmueble.GetTipoInmueblesUsos();
    //     ViewBag.TipoInmuebles = new SelectList(tiposInmuebles ?? new List<TipoInmueble>(), "Id", "Descripcion");
    //     ViewBag.TipoInmueblesUsos = new SelectList(tiposInmueblesUsos ?? new List<TipoInmuebleUso>(), "Id", "Descripcion");
    //     if (Id == 0)
    //     {
    //         ViewBag.idPropietario = idPropietario;
    //         return View(new Inmueble());
    //     }
    //     var inmueble = _repositorioInmueble.GetInmueble(Id);
    //     ViewBag.idPropietario = inmueble?.Id_Propietario;
    //     return View(inmueble);
    // }

    // [HttpPost]
    // public IActionResult CrearActualizar(Inmueble inmueble)
    // {
    //     if (inmueble.Id == 0)
    //     {
    //         _repositorioInmueble.AltaInmueble(inmueble);
    //         TempData["SuccessMessage"] = "Inmueble insertado correctamente";
    //     }
    //     else
    //     {
    //         _repositorioInmueble.ActualizarInmueble(inmueble);
    //         TempData["SuccessMessage"] = "Inmueble actualizado correctamente";
    //     }
    //     return RedirectToAction("Index");
    // }

    // [HttpPost]
    // public IActionResult BajaLogica(int Id)
    // {
    //     if (Id == 0)
    //     {
    //     }
    //     else
    //     {
    //         if (_repositorioInmueble.BajaInmueble(Id))
    //         {
    //             TempData["SuccessMessage"] = "Se elimino correctamente el inmueble";
    //         }
    //         else
    //         {
    //             TempData["ErrorMessage"] = "No se puede eliminar el inmueble";
    //         }
    //     }
    //     return RedirectToAction("Index");
    // }
}

using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaCA.Controllers;
[Authorize]
public class InmuebleTiposController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IRepositorioTipos _repositorioTipos;

    public InmuebleTiposController(ILogger<HomeController> logger, IRepositorioTipos repositorioTipos)
    {
        _logger = logger;
        _repositorioTipos = repositorioTipos;
    }

    public IActionResult Index()
    {
        TiposViewModel vm = new TiposViewModel();
        vm.TiposInmuebles = _repositorioTipos.GetTipoInmuebles();
        vm.TiposInmueblesUsos = _repositorioTipos.GetTipoInmueblesUsos();
        return View(vm);
    }

    [HttpPost]
    [Authorize(Policy = "administrador")]
    public IActionResult Baja(int eliminarId, string eliminarTipo)
    {

        if (eliminarTipo == "tipoInmueble")
        {
            if (_repositorioTipos.BajaTipoInmueble(eliminarId))
            {
                TempData["SuccessMessage"] = "Se elimino el tipo de inmueble correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se puede eliminar el tipo de inmueble";
            }
        }
        else if (eliminarTipo == "tipoInmuebleUso")
        {
            if (_repositorioTipos.BajaTipoInmuebleUso(eliminarId))
            {
                TempData["SuccessMessage"] = "Se elimino el tipo de inmueble uso correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se puede eliminar el tipo de inmueble uso";
            }
        }
        return Redirect("Index");
    }

    [HttpPost]
    public IActionResult AltaEdicion(int altaEditarId, string altaEditarTipo, string descripcion)
    {

        if (altaEditarTipo == "tipoInmueble" && altaEditarId == 0)
        {
            TipoInmueble tipoInmueble = new TipoInmueble()
            {
                Descripcion = descripcion
            };
            tipoInmueble.Id = _repositorioTipos.AltaTipoInmueble(tipoInmueble);
            if (tipoInmueble.Id > 0)
            {
                TempData["SuccessMessage"] = "Se agrego correctamente el Tipo de Inmueble";
            }
            else
            {
                TempData["ErrorMessage"] = "Ocurrio un error al agregar el tipo inmueble";
            }
        }
        else if (altaEditarTipo == "tipoInmueble" && altaEditarId > 0)
        {
            var tipoInmueble = _repositorioTipos.GetTipoInmueble(altaEditarId);
            if (tipoInmueble != null)
            {
                tipoInmueble.Descripcion = descripcion;
                if (_repositorioTipos.UpdateTipoInmueble(tipoInmueble))
                {
                    TempData["SuccessMessage"] = "Se actualizo el Tipo de inmueble";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se actualizo el Tipo de inmueble";

                }
            }
            else
            {
                TempData["ErrorMessage"] = "No existe el Tipo de Inmueble para actualizar";
            }

        }
        else if (altaEditarTipo == "tipoInmuebleUso" && altaEditarId == 0)
        {
            TipoInmuebleUso tipoInmuebleUso = new TipoInmuebleUso()
            {
                Descripcion = descripcion
            };
            tipoInmuebleUso.Id = _repositorioTipos.AltaTipoInmuebleUso(tipoInmuebleUso);
            if (tipoInmuebleUso.Id > 0)
            {
                TempData["SuccessMessage"] = "Se agrego correctamente el Tipo de Inmueble Uso";
            }
            else
            {
                TempData["ErrorMessage"] = "Ocurrio un error al agregar el Tipo Inmueble Uso";
            }
        }
        else if (altaEditarTipo == "tipoInmuebleUso" && altaEditarId > 0)
        {
            var tipoInmuebleUso = _repositorioTipos.GetTipoInmuebleUso(altaEditarId);
            if (tipoInmuebleUso != null)
            {
                tipoInmuebleUso.Descripcion = descripcion;
                if (_repositorioTipos.UpdateTipoInmuebleUso(tipoInmuebleUso))
                {
                    TempData["SuccessMessage"] = "Se actualizo el Tipo de Inmueble Uso";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se actualizo el Tipo de Inmueble Uso";

                }
            }
            else
            {
                TempData["ErrorMessage"] = "No existe el Tipo de Inmueble Uso para actualizar";
            }
        }

        return Redirect("Index");
    }
}

using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InmobiliariaCA.Controllers;

public class InmuebleTiposController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IConfiguration _Configuration;
    private RepositorioTipos _repositorioTipos;

    public InmuebleTiposController(ILogger<HomeController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _Configuration = configuration;
        _repositorioTipos = new RepositorioTipos(_Configuration);
    }

    public IActionResult Index()
    {
        TiposViewModel vm = new TiposViewModel();
        vm.TiposInmuebles = _repositorioTipos.GetTipoInmuebles();
        vm.TiposInmueblesUsos = _repositorioTipos.GetTipoInmueblesUsos();
        return View(vm);
    }

    [HttpPost]
    public IActionResult BajaLogica(int eliminarId, string eliminarTipo){

        if(eliminarTipo == "tipoInmueble"){
            if(!_repositorioTipos.ExisteRelacionTipoInmueble(eliminarId)){
                if(_repositorioTipos.BajaTipoInmueble(eliminarId)){
                    TempData["SuccessMessage"] = "Se elimino el tipo de inmueble correctamente.";
                }
                else{
                    TempData["ErrorMessage"] = "No se puede eliminar ocurrio un error inesperado";
                }
            }
            else{
                TempData["ErrorMessage"] = "El tipo de inmueble no se puede eliminar, tiene registros relacionados.";
            }
        }
        else if(eliminarTipo == "tipoInmuebleUso"){
            if(!_repositorioTipos.ExisteRelacionTipoInmuebleUso(eliminarId)){
                if(_repositorioTipos.BajaTipoInmuebleUso(eliminarId)){
                    TempData["SuccessMessage"] = "Se elimino el tipo de inmueble uso correctamente.";
                }
                else{
                    TempData["ErrorMessage"] = "No se puede eliminar ocurrio un error inesperado";
                }
            }
            else{
                TempData["ErrorMessage"] = "El tipo de uso no se puede eliminar, tiene registros relacionados.";
            }
        }
        return Redirect("Index");
    }
}

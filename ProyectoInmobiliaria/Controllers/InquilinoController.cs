using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoInmobiliaria.Models;
using ProyectoInmobiliaria.Repositorio;

namespace ProyectoInmobiliaria.Controllers;

public class InquilinoController: Controller {

    private readonly ILogger<HomeController> _logger;
    private IConfiguration _Configuration;
    private RepositorioInquilino _repositorioInquilino;

    public InquilinoController(ILogger<HomeController> logger, IConfiguration configuration) {
        _logger = logger;
        _Configuration = configuration;
        _repositorioInquilino = new RepositorioInquilino(_Configuration);
    }

    public IActionResult Index() {        
        return View(_repositorioInquilino.GetInquilinos());
    }

     public IActionResult Detalle(int Id) {
        return View(_repositorioInquilino.GetInquilino(Id));
    }
}
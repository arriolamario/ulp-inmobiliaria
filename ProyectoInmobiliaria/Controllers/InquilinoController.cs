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
    public IActionResult AltaEditar(int Id) {
        if (Id == 0)
            return View();

        return View(_repositorioInquilino.GetInquilino(Id));
    }

    [HttpPost]
    public IActionResult CrearActualizar(Inquilino inquilino) {

        var existingInquilino = _repositorioInquilino.ExisteInquilinoPorDni(inquilino.Dni);
        Console.WriteLine("Valor de existingInquilino: " + existingInquilino);
        // o si estás en un entorno donde la consola no es visible:
        Debug.WriteLine("Valor de existingInquilino: " + existingInquilino);

        if (existingInquilino) {
           
            ModelState.AddModelError("Dni", "El DNI ingresado ya está en uso.");
            return View(inquilino);
        }

        if (inquilino.Id == 0) {
            _repositorioInquilino.InsertarInquilino(inquilino);
            TempData["SuccessMessage"] = "Inquilino agregado correctamente.";
        } else {
           _repositorioInquilino.ActualizarInquilino(inquilino);
           TempData["SuccessMessage"] = "Inquilino actualizado correctamente.";
        }
        return RedirectToAction("Index");
    }

}
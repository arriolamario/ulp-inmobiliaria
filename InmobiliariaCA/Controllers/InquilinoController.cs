using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;

namespace InmobiliariaCA.Controllers;

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
        try {  
            return View(_repositorioInquilino.GetInquilinos());
        } catch (Exception ex) {              
                _logger.LogError("An error occurred while getting renter: {Error}", ex.Message);
               
                TempData["ErrorMessage"] = "Error al cargar los contratos. Por favor intente de nuevo más tarde.";
                return View(new List<Inquilino>());
            }
    }

    public IActionResult Detalle(int Id) {
        var inquilino = _repositorioInquilino.GetInquilino(Id);
        if(inquilino == null) {
            return NotFound();
        }
        return View(inquilino);
    }
    public IActionResult AltaEditar(int Id) {
        if (Id == 0)
            return View();

        return View(_repositorioInquilino.GetInquilino(Id));
    }

    [HttpPost]
    public IActionResult CrearActualizar(Inquilino inquilino) {

        var existingInquilino = _repositorioInquilino.ExisteInquilinoPorDni(inquilino.Dni);

        if (existingInquilino && inquilino.Id == 0) {
           
            ModelState.AddModelError("Dni", "El DNI ingresado ya está en uso.");
            TempData["ErrorMessage"] = "El inquilino ya existe.";
            var errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
            foreach (var error in errors) {
                Console.WriteLine("Valor de error: " + error.ErrorMessage);
            }
            return View("AltaEditar",inquilino);
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

    [HttpPost]
    public IActionResult BajaLogica(int id) {
        var success = _repositorioInquilino.BajaLogicaInquilino(id);
        
        if (success) {
            TempData["SuccessMessage"] = "Inquilino dado de baja correctamente.";
        } else {
            TempData["ErrorMessage"] = "No se pudo dar de baja al inquilino.";
        }

        return RedirectToAction("Index");
    }
}
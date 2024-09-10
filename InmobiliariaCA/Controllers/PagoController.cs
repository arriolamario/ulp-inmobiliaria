using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;

namespace InmobiliariaCA.Controllers;

public class PagoController : Controller {
    private readonly ILogger<HomeController> _logger;
    private IConfiguration _Configuration;
    private RepositorioPago _repositorioPago;

    public PagoController(ILogger<HomeController> logger, IConfiguration configuration) {
        _logger = logger;
        _Configuration = configuration;
        _repositorioPago = new RepositorioPago(_Configuration);
    }

    public IActionResult Index() {
        return View(_repositorioPago.GetPagos());
    }

    public IActionResult Detalle(int Id) {
        var pago = _repositorioPago.GetPago(Id);
        if (pago == null) {
            return NotFound();
        }

        return View(pago);
    }

    public IActionResult AltaEditar(int Id) {
        if (Id == 0)
            return View();

        return View(_repositorioPago.GetPago(Id));
    }

    [HttpPost]
    public IActionResult CrearActualizar(Pago pago) {
        if (!ModelState.IsValid) {
            TempData["ErrorMessage"] = "Datos del formulario no son válidos.";
            return RedirectToAction("Index");
        }
        if (pago.Id == 0) {
            _repositorioPago.InsertarPago(pago);
            TempData["SuccessMessage"] = "Pago agregado correctamente.";
        } else {
            _repositorioPago.ActualizarPago(pago);
            TempData["SuccessMessage"] = "Pago actualizado correctamente.";
        }
        return RedirectToAction("Index");
    }
}
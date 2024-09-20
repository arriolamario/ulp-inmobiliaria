using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;

namespace InmobiliariaCA.Controllers;

public class PagoController : Controller {
    private readonly ILogger<HomeController> _logger;
    private IRepositorioPago _repositorioPago;

    public PagoController(ILogger<HomeController> logger, IRepositorioPago repositorioPago, IConfiguration configuration) {
        _logger = logger;
        _repositorioPago = repositorioPago;
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
        try {
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
        }
        catch (Exception ex) {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Anular(int Id, int IdAnulador, int IdContrato) {
        
        if (Id == 0) {
            TempData["ErrorMessage"] = "No se pudo dar de baja al propietario.";
        } else {
            var res = _repositorioPago.AnularPago(Id, IdAnulador, IdContrato);
            if (res)
                TempData["SuccessMessage"] = "Propietario dado de baja correctamente.";
            else
                TempData["ErrorMessage"] = "No se pudo dar de baja al propietario.";
        }
        return RedirectToAction("Index");
    }
}
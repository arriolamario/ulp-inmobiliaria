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
        return View();
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
}
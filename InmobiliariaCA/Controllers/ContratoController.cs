using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InmobiliariaCA.Controllers {
    public class ContratoController : Controller {
        private RepositorioContrato _repositorioContrato;
        private RepositorioInquilino _repositorioInquilino;
        private RepositorioInmueble _repositorioInmueble;
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _Configuration;

        public ContratoController(ILogger<HomeController> logger, IConfiguration configuration) {
            
            _logger = logger;
            _Configuration = configuration;
            _repositorioContrato = new RepositorioContrato(_Configuration);
            _repositorioInquilino = new RepositorioInquilino(_Configuration);
            _repositorioInmueble = new RepositorioInmueble(_Configuration);
        }

        // GET: Contrato
        public IActionResult Index() {
            return View(_repositorioContrato.GetContratos());
        }

        // GET: Contrato/Details/5
        public IActionResult Detalle(int Id) {

            Console.WriteLine("Id detallle: " + Id);
            var contrato = _repositorioContrato.GetContrato(Id);
            if (contrato == null) {
                return NotFound();
            }
            return View(contrato);
        }

        public IActionResult AltaEditar(int Id) {
            ViewBag.Inquilinos = new SelectList(_repositorioInquilino.GetInquilinos(), "Id", "NombreCompletoDNI");
            ViewBag.Inmuebles = new SelectList(_repositorioInmueble.GetInmuebles(), "Id", "NombreInmueble");

            if (Id == 0) {                
                return View();
            }
            
            return View(_repositorioContrato.GetContrato(Id));
        }

        [HttpPost]
        public IActionResult CrearActualizar(Contrato Contrato) {

            Console.WriteLine("Contrato id: " + Contrato.Id);
            if (Contrato.Id == 0) {
                _repositorioContrato.InsertarContrato(Contrato);
                TempData["SuccessMessage"] = "Contrato agregado correctamente.";
            } else {
                _repositorioContrato.ActualizarContrato(Contrato);
                TempData["SuccessMessage"] = "Contrato actualizado correctamente.";
            }
            
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public IActionResult Baja(int id) {
            bool result = _repositorioContrato.BajaLogicaContrato(id);
            if (result) {
                return RedirectToAction(nameof(Index));
            } else {
                ModelState.AddModelError("", "Error al eliminar el contrato");
                return View();
            }
        }
    }
}
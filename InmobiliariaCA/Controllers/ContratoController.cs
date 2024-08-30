using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InmobiliariaCA.Controllers {
    public class ContratoController : Controller {
        private RepositorioContrato _repositorioContrato;
        private RepositorioInquilino _repositorioInquilino;
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _Configuration;

        public ContratoController(ILogger<HomeController> logger, IConfiguration configuration) {
            
            _logger = logger;
            _Configuration = configuration;
            _repositorioContrato = new RepositorioContrato(_Configuration);
            _repositorioInquilino = new RepositorioInquilino(_Configuration);
        }

        // GET: Contrato
        public IActionResult Index() {
            return View(_repositorioContrato.GetContratos());
        }

        // GET: Contrato/Details/5
        public IActionResult Detalle(int Id) {
            var contrato = _repositorioContrato.GetContrato(Id);
            if (contrato == null) {
                return NotFound();
            }
            return View(contrato);
        }

        public IActionResult AltaEditar(int Id) {
            if (Id == 0) {
                ViewBag.Inquilinos = new SelectList(_repositorioInquilino.GetInquilinos(), "Id", "NombreCompletoDNI");
                return View();
            }
            
            return View(_repositorioContrato.GetContrato(Id));
        }

        [HttpPost]
        public IActionResult AltaEditar(Contrato Contrato) {
            if (ModelState.IsValid) {
                
                int result = _repositorioContrato.InsertarContrato(Contrato);
                if (result > 0) {
                    return RedirectToAction(nameof(Index));
                } else {
                    ModelState.AddModelError("", "Error al insertar el contrato");
                }
            }

            ViewBag.Inquilinos = new SelectList(_repositorioInquilino.GetInquilinos(), "Id", "NombreCompletoDNI");
            return View(Contrato);
        }

        public IActionResult Edit(int Id) {
            var contrato = _repositorioContrato.GetContrato(Id);
            if (contrato == null) {
                return NotFound();
            }
            return View(contrato);
        }

        [HttpPost]
        public IActionResult Actualizar(int Id, Contrato Contrato) {
            if (Id != Contrato.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                bool result = _repositorioContrato.ActualizarContrato(Contrato);
                if (result) {
                    return RedirectToAction(nameof(Index));
                } else {
                    ModelState.AddModelError("", "Error al actualizar el contrato");
                }
            }
            return View(Contrato);
       
        }
        
        [HttpPost]
        public IActionResult DeleteConfirmed(int id) {
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
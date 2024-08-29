using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaCA.Controllers {
    public class ContratoController : Controller {
        private RepositorioContrato _repositorioContrato;
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _Configuration;

        public ContratoController(ILogger<HomeController> logger, IConfiguration configuration) {
            
            _logger = logger;
            _Configuration = configuration;
            _repositorioContrato = new RepositorioContrato(_Configuration);
        }

        // GET: Contrato
        public IActionResult Index() {
            return View(_repositorioContrato.GetContratos());
        }

        // GET: Contrato/Details/5
        public IActionResult Details(int id) {
            var contrato = _repositorioContrato.GetContrato(id);
            if (contrato == null) {
                return NotFound();
            }
            return View(contrato);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contrato contrato) {
            if (ModelState.IsValid) {
                int result = _repositorioContrato.InsertarContrato(contrato);
                if (result > 0) {
                    return RedirectToAction(nameof(Index));
                } else {
                    ModelState.AddModelError("", "Error al insertar el contrato");
                }
            }
            return View(contrato);
        }

        public IActionResult Edit(int id) {
            var contrato = _repositorioContrato.GetContrato(id);
            if (contrato == null) {
                return NotFound();
            }
            return View(contrato);
        }

        [HttpPost]
        public IActionResult Actualizar(int id, Contrato contrato) {
            if (id != contrato.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                bool result = _repositorioContrato.ActualizarContrato(contrato);
                if (result) {
                    return RedirectToAction(nameof(Index));
                } else {
                    ModelState.AddModelError("", "Error al actualizar el contrato");
                }
            }
            return View(contrato);
       
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
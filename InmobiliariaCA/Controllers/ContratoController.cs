using System.Globalization;
using InmobiliariaCA.Models;
using InmobiliariaCA.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InmobiliariaCA.Controllers {
    public class ContratoController : Controller {
        private IRepositorioContrato _repositorioContrato;
        private IRepositorioInquilino _repositorioInquilino;
        private IRepositorioInmueble _repositorioInmueble;
        private readonly ILogger<HomeController> _logger;

        public ContratoController(ILogger<HomeController> logger, 
                        IRepositorioContrato repositorioContrato,
                        IRepositorioInquilino repositorioInquilino,
                        IRepositorioInmueble repositorioInmueble) 
        {
            
            _logger = logger;
            _repositorioContrato = repositorioContrato;
            _repositorioInquilino = repositorioInquilino;
            _repositorioInmueble = repositorioInmueble;
        }

        // GET: Contrato        
        public IActionResult Index() {
            try {
                var contratos = _repositorioContrato.GetContratos();
                return View(contratos);
            } catch (Exception ex) {              
                _logger.LogError("An error occurred while getting contracts: {Error}", ex.Message);
               
                TempData["ErrorMessage"] = "Error al cargar los contratos. Por favor intente de nuevo más tarde.";
                return View(new List<Contrato>());
            }
        }

        // GET: Contrato/Details/5
        public IActionResult Detalle(int Id) {
            try {
                Random random = new Random();
                int numeroPago = random.Next(100000, 999999);
                ViewBag.NumeroPago = numeroPago;
                
                var contrato = _repositorioContrato.GetContrato(Id);
                if (contrato == null) {
                    return NotFound();
                }
                
                return View(contrato);
            } catch (Exception ex) {              
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);
               
                TempData["ErrorMessage"] = "Error al cargar la vista de contrato. Por favor intente de nuevo más tarde.";
                return View();
            }
            
        }

        public IActionResult AltaEditar(int Id) {
            ViewBag.Inquilinos = new SelectList(GetInquilinos(), "Id", "NombreCompletoDNI");
            ViewBag.Inmuebles = GetInmueblesSelectList(Id == 0);

            if (Id == 0) {
                return View(new Contrato());
            }

            var contrato = _repositorioContrato.GetContrato(Id);
            return View(contrato);
        }

        [HttpPost]
        public IActionResult CrearActualizar(Contrato Contrato) {
            
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
            bool result = _repositorioContrato.BajaContrato(id);
            if (result) {
                return RedirectToAction(nameof(Index));
            } else {
                ModelState.AddModelError("", "Error al eliminar el contrato");
                return View();
            }
        }

        public IActionResult TerminarContrato(int Id) {
            var contrato = _repositorioContrato.GetContrato(Id);
            if (contrato == null) {
                return NotFound("El contrato solicitado no existe.");
            }
           
            return View("TerminarContrato", contrato);
        }

        [HttpPost]
        public IActionResult FinalizarContrato(Contrato contrato) {
            var contratoDb = _repositorioContrato.GetContrato(contrato.Id);
            if (contratoDb == null) {
                return NotFound("El contrato solicitado no existe.");
            }

            contratoDb.Fecha_Finalizacion_Anticipada = contrato.Fecha_Finalizacion_Anticipada;
            contratoDb.MultaCalculada();
            _repositorioContrato.ActualizarContrato(contratoDb);

            //return View("TerminarContrato", contrato);
            return RedirectToAction("Index");
        }

        private IEnumerable<Inquilino> GetInquilinos() {
            return _repositorioInquilino.GetInquilinos();
        }

        private SelectList GetInmueblesSelectList(bool sinUso) {
            var inmuebles = sinUso ? _repositorioInmueble.GetInmueblesSinUso() : _repositorioInmueble.GetInmuebles();
            ViewBag.InmueblesData = inmuebles.ToDictionary(i => i.Id.ToString(), i => i.Precio.ToString("0.##", CultureInfo.InvariantCulture));
            return new SelectList(inmuebles, "Id", "NombreInmueble");
        }

    }
}
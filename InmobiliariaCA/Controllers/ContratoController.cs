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
        // private IConfiguration _Configuration;

        public ContratoController(ILogger<HomeController> logger, 
                        IRepositorioContrato repositorioContrato,
                        IRepositorioInquilino repositorioInquilino,
                        IRepositorioInmueble repositorioInmueble) 
        {
            
            _logger = logger;
            //_Configuration = configuration;
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
            ViewBag.Inquilinos = new SelectList(_repositorioInquilino.GetInquilinos(), "Id", "NombreCompletoDNI");
            
            var inmuebles = _repositorioInmueble.GetInmueblesSinUso();
            ViewBag.Inmuebles = new SelectList(inmuebles, "Id", "NombreInmueble", "Precio");
            ViewBag.InmueblesData = inmuebles.ToDictionary(i => i.Id.ToString(), i => i.Precio.ToString("0.##", CultureInfo.InvariantCulture));

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
            bool result = _repositorioContrato.BajaContrato(id);
            if (result) {
                return RedirectToAction(nameof(Index));
            } else {
                ModelState.AddModelError("", "Error al eliminar el contrato");
                return View();
            }
        }
    }
}
using System.Globalization;
using InmobiliariaCA.Models;
using InmobiliariaCA.Models.ContratoModels;
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

        // GET: Contratos
        public int? InquilinoId { get; set; }
        public int? InmuebleId { get; set; }
        public EstadoContrato? Estado { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public IActionResult Index(ContratoFilter filters) {
            try {
                var viewModel = new ContratoViewModel {
                    Contratos = _repositorioContrato.GetContratosFiltrados(filters),
                    Filters = filters
                };

                ViewBag.Inquilinos =  new SelectList(GetInquilinos(), "Id", "NombreCompletoDNI");
                ViewBag.Inmuebles = GetInmueblesSelectList(false);                

                return View(viewModel);
            } catch (Exception ex) {
                _logger.LogError("An error occurred while getting contracts: {Error}", ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                return View(new ContratoViewModel { Contratos = new List<Contrato>() });
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
               
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }            
        }

        public IActionResult AltaEditar(int Id) {
            try {
                ViewBag.Inquilinos = new SelectList(GetInquilinos(), "Id", "NombreCompletoDNI");
                ViewBag.Inmuebles = GetInmueblesSelectList(false);

                if (Id == 0) {
                    return View(new Contrato());
                }

                var contrato = _repositorioContrato.GetContrato(Id);
                return View(contrato);
            } catch (Exception ex) {              
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);               
                TempData["ErrorMessage"] = "Error al cargar o editar un contrato. Por favor intente de nuevo más tarde.";
                return View();
            }  
        }

        [HttpPost]
        public IActionResult CrearActualizar(Contrato Contrato) {
            try {
                if(!_repositorioInmueble.EsInmuebleDisponible(Contrato.Id_Inmueble, Contrato.Fecha_Desde, Contrato.Fecha_Hasta)) {
                    throw new Exception("No se puede crear el contrato en ese rango de fechas.");
                }
            
                if (Contrato.Id == 0) {                    
                    _repositorioContrato.InsertarContrato(Contrato);
                    TempData["SuccessMessage"] = "Contrato agregado correctamente.";
                } else {
                    _repositorioContrato.ActualizarContrato(Contrato);
                    TempData["SuccessMessage"] = "Contrato actualizado correctamente.";
                }
                
                return RedirectToAction("Index");
            } catch (Exception ex) {              
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);
               
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }  
        }
        
        [HttpPost]
        public IActionResult Baja(int id) {
            try {
                bool result = _repositorioContrato.BajaContrato(id);
                if (result) {
                    return RedirectToAction(nameof(Index));
                } else {
                    ModelState.AddModelError("", "Error al eliminar el contrato");
                    return View();
                }
            } catch (Exception ex) {              
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);
               
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }  
        }

        public IActionResult TerminarContrato(int Id) {
            try {
                var contrato = _repositorioContrato.GetContrato(Id);
                if (contrato == null) {
                    return NotFound("El contrato solicitado no existe.");
                }
            
                return View("TerminarContrato", contrato);
            } catch (Exception ex) {              
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);
               
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }  
        }

        [HttpPost]
        public IActionResult FinalizarContrato(Contrato contrato) {
            try {
                var contratoDb = _repositorioContrato.GetContrato(contrato.Id);
                if (contratoDb == null) {
                    return NotFound("El contrato solicitado no existe.");
                }

                contratoDb.Fecha_Finalizacion_Anticipada = contrato.Fecha_Finalizacion_Anticipada;
                contratoDb.MultaCalculada();
                contratoDb.Estado = EstadoContrato.Finalizado;

                _repositorioContrato.ActualizarContrato(contratoDb);

                return RedirectToAction("Index");
            } catch (Exception ex) {              
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);
               
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }  
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
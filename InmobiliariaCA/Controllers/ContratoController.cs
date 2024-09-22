using System.Globalization;
using InmobiliariaCA.Models;
using InmobiliariaCA.Models.ContratoModels;
using InmobiliariaCA.Repositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InmobiliariaCA.Controllers
{
    [Authorize]
    public class ContratoController : Controller
    {
        private IRepositorioContrato _repositorioContrato;
        private IRepositorioInquilino _repositorioInquilino;
        private IRepositorioPago _repositorioPago;
        private IRepositorioInmueble _repositorioInmueble;
        private readonly ILogger<HomeController> _logger;

        public ContratoController(ILogger<HomeController> logger,
                        IRepositorioContrato repositorioContrato,
                        IRepositorioInquilino repositorioInquilino,
                        IRepositorioInmueble repositorioInmueble,
                        IRepositorioPago repositorioPago)
        {
            _logger = logger;
            _repositorioContrato = repositorioContrato;
            _repositorioInquilino = repositorioInquilino;
            _repositorioInmueble = repositorioInmueble;
            _repositorioPago = repositorioPago;
        }

        public IActionResult Index(ContratoFilter filters) {
            try {
                var viewModel = new ContratoViewModel {
                    Contratos = _repositorioContrato.GetContratosFiltrados(filters, null),
                    Filters = filters
                };

                ViewBag.Inquilinos = new SelectList(GetInquilinos(), "Id", "NombreCompletoDNI");
                ViewBag.Inmuebles = GetInmueblesSelectList(false);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting contracts: {Error}", ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                return View(new ContratoViewModel { Contratos = new List<Contrato>() });
            }
        }

        // GET: Contrato/Details/5
        public IActionResult Detalle(int Id)
        {
            try
            {
                int numeroPago = NroRandomPago();
                ViewBag.NumeroPago = numeroPago;

                var contrato = _repositorioContrato.GetContrato(Id, null);

                if (contrato == null)
                {
                    return NotFound();
                }

                List<Pago> pagos = _repositorioPago.GetPagosContrato(contrato.Id);

                ContratoDetalleViewModel viewModel = new ContratoDetalleViewModel(contrato);

                viewModel.Pagos = pagos;

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        public IActionResult AltaEditar(
            int Id, 
            [FromQuery] DateTime? FechaDesde, 
            [FromQuery] DateTime? FechaHasta, 
            [FromQuery] int? Id_Inquilino, 
            [FromQuery] decimal? Monto_Alquiler
            )
        {
            try
            {
                ViewBag.Inquilinos = new SelectList(GetInquilinos(), "Id", "NombreCompletoDNI");
                if (!FechaDesde.HasValue)
                    FechaDesde = DateTime.Today;
                if (!FechaHasta.HasValue)
                    FechaHasta = DateTime.Today;

                ViewBag.Inmuebles = GetInmueblesDisponibles(FechaDesde.Value, FechaHasta.Value);
                ContratoAltaEditarViewModel model = new ContratoAltaEditarViewModel();
                if (Id == 0)
                {
                    model.Fecha_Desde = FechaDesde.Value;
                    model.Fecha_Hasta = FechaHasta.Value;
                    model.Id_Inquilino = Id_Inquilino.HasValue ? Id_Inquilino.Value : 0;
                    model.Monto_Alquiler = Monto_Alquiler.HasValue ? Monto_Alquiler.Value : 0;
                }else {
                    var contrato = _repositorioContrato.GetContrato(Id, null);
                    model = new ContratoAltaEditarViewModel(contrato ?? new Contrato());
    
                }
                
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);
                TempData["ErrorMessage"] = "Error al cargar o editar un contrato. Por favor intente de nuevo más tarde.";
                return View();
            }
        }

        [HttpPost]
        public IActionResult CrearActualizar(ContratoAltaEditarViewModel Contrato)
        {
            try
            {
                var listInmueble = GetInmueblesDisponibles(Contrato.Fecha_Desde, Contrato.Fecha_Hasta);
                if (!ModelState.IsValid)
                {
                    return  View("AltaEditar", Contrato);
                }
                else if (Contrato.Id_Inmueble == 0)
                {
                    ModelState.AddModelError("", "Debe seleccionar un inmueble");
                    ViewBag.Inquilinos = new SelectList(GetInquilinos(), "Id", "NombreCompletoDNI");
                    ViewBag.Inmuebles = listInmueble;
                    return View("AltaEditar", Contrato);
                }

                var inmuebleDisponible = listInmueble.FirstOrDefault(x => x.Id == Contrato.Id_Inmueble);
                if (inmuebleDisponible == null)
                {
                    ModelState.AddModelError("", "Inmueble no disponible");
                    ViewBag.Inquilinos = new SelectList(GetInquilinos(), "Id", "NombreCompletoDNI");
                    ViewBag.Inmuebles = listInmueble;
                    return View("AltaEditar", Contrato);
                }
                Contrato contratoNew = new Contrato(Contrato);
                var idUsuarioCookie = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
                if (idUsuarioCookie == null)
                    contratoNew.Id_Usuario_Creacion = 1;
                else
                    contratoNew.Id_Usuario_Creacion = int.Parse(idUsuarioCookie);

                contratoNew.Id = _repositorioContrato.InsertarContrato(contratoNew);

                if (contratoNew.Id > 0){
                    TempData["SuccessMessage"] = "Contrato agregado correctamente.";
                }
                else{
                    TempData["ErrorMessage"] = "Contrato no se pudo crear.";
                }
                

                return Redirect("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);

                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Policy = "administrador")]
        public IActionResult Baja(int id)
        {
            try
            {
                bool result = _repositorioContrato.BajaContrato(id);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error al eliminar el contrato");
                    return View();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
        
        public IActionResult TerminarContrato(int Id)
        {
            try
            {
                var contrato = _repositorioContrato.GetContrato(Id, null);
                if (contrato == null)
                {
                    return NotFound("El contrato solicitado no existe.");
                }

                return View("TerminarContrato", contrato);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [Authorize(Policy = "administrador")]
        public IActionResult FinalizarContrato(Contrato contrato)
        {
            try
            {
                var contratoDb = _repositorioContrato.GetContrato(contrato.Id, null);
                if (contratoDb == null)
                {
                    return NotFound("El contrato solicitado no existe.");
                }

                contratoDb.Fecha_Finalizacion_Anticipada = contrato.Fecha_Finalizacion_Anticipada;
                contratoDb.MultaCalculada();
                contratoDb.Estado = EstadoContrato.Finalizado;
                var IdUser = User.FindFirst("Id");
                contratoDb.Id_Usuario_Finalizacion = IdUser != null ? int.Parse(IdUser.Value) : 0;
                _repositorioContrato.ActualizarContrato(contratoDb);
                
                //Insertar en pagos
                var pago = new Pago();
                pago.Contrato_Id = contratoDb.Id;
                pago.Numero_Pago = NroRandomPago();
                pago.Fecha_Pago = DateTime.Now;
                pago.Detalle = "Pago con Multa";
                pago.Importe = contratoDb.Monto_Alquiler + (contratoDb.Multa ?? 0);
                pago.Estado = EstadoPago.Anulado;
                pago.Fecha_Anulacion = contratoDb.Fecha_Finalizacion_Anticipada ?? DateTime.Now;

                _repositorioPago.InsertarPago(pago, null);

                TempData["SuccessMessage"] = "Contrato Finalizado correctamente.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);

                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult RenovarContrato(int Id) {
            try {
                var contrato = _repositorioContrato.GetContrato(Id, null);
                if (contrato == null) {
                    return NotFound("El contrato solicitado no existe.");
                }

                return View("RenovarContrato", contrato);
            } catch (Exception ex) {
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        [HttpPost]
        [Authorize(Policy = "administrador")]
        public IActionResult RenovarContrato(Contrato contrato) {
            try {
                var contratoDb = _repositorioContrato.GetContrato(contrato.Id, null);
                var IdUser = User.FindFirst("Id");

                if (contratoDb == null) {
                    return NotFound("El contrato solicitado no existe.");
                }
                //--Renoovacion del contrato, con el ID del contrato viejo
                contratoDb.Fecha_Desde = contrato.Fecha_Desde;
                contratoDb.Fecha_Hasta = contrato.Fecha_Hasta;
                contratoDb.Monto_Alquiler = contrato.Monto_Alquiler;
                contratoDb.Estado = EstadoContrato.Vigente;
                contratoDb.Fecha_Finalizacion_Anticipada= null;
                contratoDb.Multa = null;
                contratoDb.Pagado = false;
                contratoDb.Cantidad_Cuotas = contratoDb.CantidadCuotas();
                contratoDb.Cuotas_Pagas = 0;
                contratoDb.Id_Usuario_Finalizacion = null;               
                contratoDb.Id_Usuario_Creacion = IdUser != null ? int.Parse(IdUser.Value) : 0;

                _repositorioContrato.InsertarContrato(contratoDb);

                TempData["SuccessMessage"] = "Contrato Renovado correctamente.";
                return RedirectToAction("Index");
            } catch (Exception ex) {
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);

                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
        
        private IEnumerable<Inquilino> GetInquilinos()
        {
            return _repositorioInquilino.GetInquilinos();
        }

        private SelectList GetInmueblesSelectList(bool sinUso)
        {
            var inmuebles = sinUso ? _repositorioInmueble.GetInmueblesSinUso() : _repositorioInmueble.GetInmuebles();
            ViewBag.InmueblesData = inmuebles.ToDictionary(i => i.Id.ToString(), i => i.Precio.ToString("0.##", CultureInfo.InvariantCulture));
            return new SelectList(inmuebles, "Id", "NombreInmueble");
        }

        private IEnumerable<Inmueble> GetInmueblesDisponibles(DateTime fechaDesde, DateTime fechaHasta)
        {
            return _repositorioInmueble.GetInmueblesDisponiblesPorFecha(fechaDesde, fechaHasta);
        }

        private static int NroRandomPago() 
        {
            Random random = new Random();
            int numeroPago = random.Next(100000, 999999);
            return numeroPago;
        }
    
    }
}
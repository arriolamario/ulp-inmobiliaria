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

        public IActionResult Index(ContratoFilter filters) {
            try {
                var viewModel = new ContratoViewModel {
                    Contratos = _repositorioContrato.GetContratosFiltrados(filters),
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
                Random random = new Random();
                int numeroPago = random.Next(100000, 999999);
                ViewBag.NumeroPago = numeroPago;

                var contrato = _repositorioContrato.GetContrato(Id);
                if (contrato == null)
                {
                    return NotFound();
                }

                return View(contrato);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred while getting contract: {Error}", ex.Message);

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        public IActionResult AltaEditar(int Id, [FromQuery] DateTime? FechaDesde, [FromQuery] DateTime? FechaHasta, [FromQuery] int? Id_Inquilino, [FromQuery] decimal? Monto_Alquiler)
        {
            try
            {
                ViewBag.Inquilinos = new SelectList(GetInquilinos(), "Id", "NombreCompletoDNI");
                if (!FechaDesde.HasValue)
                    FechaDesde = DateTime.Today;
                if (!FechaHasta.HasValue)
                    FechaHasta = DateTime.Today;

                ViewBag.Inmuebles = GetInmueblesDisponibles(FechaDesde.Value, FechaHasta.Value);
                if (Id == 0)
                {
                    var contratoViewModerl = new ContratoAltaEditarViewModel();
                    contratoViewModerl.Fecha_Desde = FechaDesde.Value;
                    contratoViewModerl.Fecha_Hasta = FechaHasta.Value;
                    contratoViewModerl.Id_Inquilino = Id_Inquilino.HasValue ? Id_Inquilino.Value : 0;
                    contratoViewModerl.Monto_Alquiler = Monto_Alquiler.HasValue ? Monto_Alquiler.Value : 0;
                    return View(contratoViewModerl);
                }

                var contrato = _repositorioContrato.GetContrato(Id);
                return View(contrato);
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
                var contrato = _repositorioContrato.GetContrato(Id);
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
        public IActionResult FinalizarContrato(Contrato contrato)
        {
            try
            {
                var contratoDb = _repositorioContrato.GetContrato(contrato.Id);
                if (contratoDb == null)
                {
                    return NotFound("El contrato solicitado no existe.");
                }

                contratoDb.Fecha_Finalizacion_Anticipada = contrato.Fecha_Finalizacion_Anticipada;
                contratoDb.MultaCalculada();
                contratoDb.Estado = EstadoContrato.Finalizado;
                contratoDb.Id_Usuario_Finalizacion = 2;

                _repositorioContrato.ActualizarContrato(contratoDb);

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
    }
}
using InmobiliariaCA.Models;
using InmobiliariaCA.Models.ContratoModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaCA.Api;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class ContratoController : ControllerBase
{
    private readonly IConfiguration config;
    private readonly DataContext contexto;

    private readonly IWebHostEnvironment environment;

    public ContratoController(DataContext contexto, IConfiguration config, IWebHostEnvironment environment)
    {
        this.config = config;
        this.contexto = contexto;
        this.environment = environment;
    }

    [HttpGet]
    [Route("{idContrato}")]
    public async Task<ActionResult> Inmuebles(int idContrato)
    {
        var Id = User.FindFirst("Id")?.Value;

        ContratoApi contrato = await contexto.Contrato
                                        .Include(x => x.Inquilino)
                                        .Include(x => x.Inmueble)
                                        .FirstAsync(x => x.Id == idContrato && x.Inmueble.Id_Propietario == int.Parse(Id));
        
        return Ok(new
        {
            status = "exito",
            message = "Detalle de contrato",
            data = new {
                contrato.Id,
                fechaDesde =contrato.Fecha_Desde,
                fechaHasta = contrato.Fecha_Hasta,
                montoAlquiler = contrato.Monto_Alquiler,
                inquilino = new {
                    id = contrato.Id_Inquilino,
                    contrato.Inquilino.Nombre,
                    contrato.Inquilino.Apellido
                },
                inmueble = new {
                    contrato.Inmueble.Direccion
                }
            }
        });
    }

    [HttpGet]
    [Route("{idContrato}/pagos")]
    public async Task<ActionResult> Pagos(int idContrato)
    {
        var Id = User.FindFirst("Id")?.Value;

        List<PagoApi> listaPagos = await contexto.Pago
                                        .Where(x => x.Contrato_Id == idContrato)
                                        .ToListAsync();
        
        return Ok(new
        {
            status = "exito",
            message = "Lista de pagos",
            data = listaPagos.Select(x => new {
                x.Id,
                contratoId = x.Contrato_Id,
                numeroPago = x.Numero_Pago,
                fechaPago = x.Fecha_Pago,
                x.Importe,
                x.Detalle
            })
        });
    }
}
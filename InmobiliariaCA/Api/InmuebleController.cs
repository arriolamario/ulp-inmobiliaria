using InmobiliariaCA.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaCA.Api;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class InmuebleController : ControllerBase
{
    private readonly IConfiguration config;
    private readonly DataContext contexto;

    private readonly IWebHostEnvironment environment;

    public InmuebleController(DataContext contexto, IConfiguration config, IWebHostEnvironment environment)
    {
        this.config = config;
        this.contexto = contexto;
        this.environment = environment;
    }

    [HttpGet]
    public async Task<ActionResult<Propietario?>> Inmuebles()
    {
        var Id = User.FindFirst("Id")?.Value;

        List<Inmueble> inmuebles = await contexto.Inmueble.Where(x => x.Id_Propietario == int.Parse(Id))
                                .Include(x => x.Propietario).Include(x => x.Tipo_Uso).Include(x => x.Tipo).ToListAsync();

        return Ok(new
        {
            status = "exito",
            message = "Listado de inmuebles",
            data = inmuebles.Select(x => new
            {
                x.Id,
                x.Direccion,
                idTipoUso = x.Id_Tipo_Inmueble_Uso,
                uso = new
                {
                    x.Tipo_Uso.Id,
                    x.Tipo_Uso.Descripcion
                },
                idTipo = x.Id_Tipo_Inmueble,
                tipo = new
                {
                    x.Tipo.Id,
                    x.Tipo.Descripcion
                }
            })
        });
    }
}
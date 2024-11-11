using InmobiliariaCA.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InmobiliariaCA.Api;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class TiposController : ControllerBase
{
    private readonly IConfiguration config;
    private readonly DataContext contexto;

    private readonly IWebHostEnvironment environment;

    public TiposController(DataContext contexto, IConfiguration config, IWebHostEnvironment environment)
    {
        this.config = config;
        this.contexto = contexto;
        this.environment = environment;
    }

    [HttpGet]
    public async Task<ActionResult<Propietario?>> tipos()
    {
        var tipos = contexto.TipoInmueble.ToList();
        var tiposUso = contexto.TipoInmuebleUso.ToList();

        return Ok(new
        {
            status = "exito",
            message = "Listado de inmuebles",
            data = new {
                tipo = tipos.Select(x => new {
                    x.Id,
                    x.Descripcion
                }),
                uso = tiposUso.Select(x => new {
                    x.Id,
                    x.Descripcion
                })
            }
        });
    }

    [HttpGet]
    [Route("{idInmueble}")]
    public async Task<ActionResult<Propietario?>> Inmuebles(int idInmueble){
        var Id = User.FindFirst("Id")?.Value;

        List<Inmueble> inmuebles = await contexto.Inmueble.Where(x => x.Id_Propietario == int.Parse(Id) && x.Id == idInmueble)
                                .Include(x => x.Propietario).Include(x => x.Tipo_Uso).Include(x => x.Tipo).ToListAsync();

        return Ok(new
        {
            status = "exito",
            message = "Listado de inmuebles",
            data = inmuebles.Select(x => new
            {
                x.Id,
                x.Direccion,
                x.Precio,
                x.Avatar_Url,
                x.Ambientes,
                x.Activo,
                tipo = new {
                    x.Tipo.Id,
                    x.Tipo.Descripcion
                },
                uso = new {
                    x.Tipo_Uso.Id,
                    x.Tipo_Uso.Descripcion
                }       
            })
        });
    }
}
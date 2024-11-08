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
    public async Task<ActionResult> Inmuebles()
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
                x.Precio,
                x.Avatar_Url
            })
        });
    }

    [HttpGet("alquilados")]
    public async Task<ActionResult> InmueblesAlquilados()
    {
        var Id = User.FindFirst("Id")?.Value;
        // var contrato =    contexto.Contrato.Include(x => x.Inmueble).First();
        // return Ok(contrato);
        var inmueblesAlquiladosHoy = await contexto.Contrato
            .Where(c => c.Fecha_Desde <= DateTime.Today && c.Fecha_Hasta >= DateTime.Today && c.EstadoString == "Vigente")
            .Include(c => c.Inmueble)
            .Where(x => x.Inmueble.Id_Propietario == int.Parse(Id)) // Asegúrate de tener la propiedad de navegación configurada
            .Select(c => new { c.Inmueble, c.Id_Inquilino, Id_Contrato = c.Id})
            .ToListAsync();

        return Ok(new
        {
            status = "exito",
            message = "Listado de inmuebles alquilados",
            data = inmueblesAlquiladosHoy.Select(x => new
            {
                x.Inmueble.Id,
                x.Inmueble.Direccion,
                x.Inmueble.Precio,
                x.Inmueble.Avatar_Url,
                idInquilino = x.Id_Inquilino,
                idContrato = x.Id_Contrato
            })
        });
    }

    [HttpGet]
    [Route("{idInmueble}")]
    public async Task<ActionResult> Inmuebles(int idInmueble)
    {
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
                tipo = new
                {
                    x.Tipo.Id,
                    x.Tipo.Descripcion
                },
                uso = new
                {
                    x.Tipo_Uso.Id,
                    x.Tipo_Uso.Descripcion
                }
            }).First()
        });
    }

    [HttpPut]
    public async Task<ActionResult> Post([FromForm] InmuebleApi inmuebleApi, [FromForm] IFormFile imagen)
    {
        var Id = User.FindFirst("Id")?.Value;



        Inmueble inmueble = new Inmueble
        {
            Id_Propietario = Id == null ? 0 : int.Parse(Id),
            Id_Tipo_Inmueble = inmuebleApi.IdTipo,
            Id_Tipo_Inmueble_Uso = inmuebleApi.IdUso,
            Direccion = inmuebleApi.Direccion,
            Precio = inmuebleApi.Precio,
            Ambientes = inmuebleApi.Ambientes,
            Activo = inmuebleApi.Activo,
            Coordenada_Lat = "0",
            Coordenada_Lon = "0",
            Avatar_Url = "",
            Fecha_Actualizacion = DateTime.Now,
            Fecha_Creacion = DateTime.Now
        };
        try
        {
            contexto.Inmueble.Add(inmueble);
            await contexto.SaveChangesAsync();

            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "inmueble");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = "inmueble_" + inmueble.Id + Path.GetExtension(imagen.FileName);
            string pathCompleto = Path.Combine(path, fileName);
            inmueble.Avatar_Url = Path.Combine("\\inmueble", fileName);
            await contexto.SaveChangesAsync();

            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
            {
                imagen.CopyTo(stream);
            }
        }
        catch (Exception ex)
        {

        }
        // contexto.Add(inmueble);

        return Ok(new
        {
            status = "exito",
            message = "Inmueble creado"
        });
    }

    [HttpPatch("{idInmueble}")]
    public async Task<ActionResult> Patch([FromForm] Boolean activo, [FromForm] IFormFile imagen, int idInmueble)
    {
        var Id = User.FindFirst("Id")?.Value;
        Inmueble inmueblebd =contexto.Inmueble.First(x => x.Id == idInmueble && x.Id_Propietario == int.Parse(Id));

        inmueblebd.Activo = activo;

        
        try
        {
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "inmueble");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = "inmueble_" + inmueblebd.Id + Path.GetExtension(imagen.FileName);
            string pathCompleto = Path.Combine(path, fileName);
            inmueblebd.Avatar_Url = Path.Combine("\\inmueble", fileName);
            await contexto.SaveChangesAsync();

            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
            {
                imagen.CopyTo(stream);
            }
        }
        catch (Exception ex)
        {

        }
        // contexto.Add(inmueble);

        return Ok(new
        {
            status = "exito",
            message = "Inmueble creado"
        });
    }
}
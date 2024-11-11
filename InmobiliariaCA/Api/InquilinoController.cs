using InmobiliariaCA.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InmobiliariaCA.Api;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class InquilinoController : ControllerBase
{
    private readonly IConfiguration config;
    private readonly DataContext contexto;

    private readonly IWebHostEnvironment environment;

    public InquilinoController(DataContext contexto, IConfiguration config, IWebHostEnvironment environment)
    {
        this.config = config;
        this.contexto = contexto;
        this.environment = environment;
    }

    [HttpGet("{idInquilino}")]
    public ActionResult InquilinoByInmueble(int idInquilino)
    {
        Inquilino inquilino = contexto.Inquilino
            .First(x => x.Id == idInquilino);
        string[] telefonoSplit = inquilino.Telefono.Split('-');
        return Ok(new
        {
            status = "exito",
            message = "Inquilino encontrado",
            data = new
            {
                inquilino.Id,
                inquilino.Dni,
                inquilino.Nombre,
                inquilino.Apellido,
                inquilino.Email,
                inquilino.Direccion,
                TelefonoArea = telefonoSplit[0],
                TelefonoNumero = telefonoSplit[1]
            }
        });
    }
}
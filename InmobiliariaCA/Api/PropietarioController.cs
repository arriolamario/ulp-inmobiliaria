using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using InmobiliariaCA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using MimeKit;
using MailKit.Net.Smtp;

namespace InmobiliariaCA.Api;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
public class PropietariosController : ControllerBase
{
    private readonly IConfiguration config;
    private readonly DataContext contexto;

    private readonly IWebHostEnvironment environment;

    public PropietariosController(DataContext contexto, IConfiguration config, IWebHostEnvironment environment)
    {
        this.config = config;
        this.contexto = contexto;
        this.environment = environment;
    }

    [HttpGet]
    public async Task<ActionResult<Propietario?>> Get()
    {
        var usuario = User.Identity?.Name;

        Propietario? propietario = await contexto.Propietario.SingleOrDefaultAsync(x => x.Usuario == usuario);
        string[] telefonoSplit = propietario.Telefono.Split('-');
        return Ok(new
        {
            propietario.Id,
            documento = propietario.Dni,
            propietario.Nombre,
            propietario.Apellido,
            telefonoArea = telefonoSplit[0],
            telefonoNumero = telefonoSplit[1],
            propietario.Email,
            propietario.Direccion,
            propietario.Avatar_Url,
            usuario
        });
    }

    [HttpPatch]
    public async Task<ActionResult> Patch([FromBody] PropietarioApi propietario)
    {
        Propietario propietarioBd = await contexto.Propietario.FirstAsync(x => x.Id == propietario.Id);
        propietarioBd.Dni = propietario.Documento;
        propietarioBd.Nombre = propietario.Nombre;
        propietarioBd.Apellido = propietario.Apellido;
        propietarioBd.Telefono = $"{propietario.TelefonoArea}-{propietario.TelefonoNumero}";
        propietarioBd.Email = propietario.Email;
        int result = await contexto.SaveChangesAsync();

        if (result == 0)
        {
            return BadRequest(new
            {
                status = "error",
                message = "No se pudo actualizar el propietario"
            });
        }

        return Ok(new
        {
            status = "exito",
            message = "Propietario actualizado correctamente"
        });
    }

    // POST api/<controller>/login
    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] PropietarioLoginView loginView)
    {
        string hashed = PasswordHash(loginView.Clave);

        var propietarioBd = contexto.Propietario.FirstOrDefault(x => x.Usuario == loginView.Usuario && x.Password_Hash == hashed);
        if (propietarioBd == null)
        {
            return Unauthorized(new
            {
                mensaje = "Nombre de usuario o clave incorrecta"
            }
        );
        }

        var token = GeneracionToken(propietarioBd, out string jwtToken);

        return Ok(new
        {
            token = jwtToken,
            expiracion = token.ValidTo,
            usuario = new
            {
                propietarioBd.Email,
                propietarioBd.Nombre,
                propietarioBd.Apellido,
                propietarioBd.Avatar_Url
            }
        });
    }


    [HttpPatch("cambiarclave")]
    public async Task<ActionResult> CambiarClave([FromBody] string clave)
    {
        var Id = User.FindFirst("Id")?.Value;

        Propietario? propietario = await contexto.Propietario.FirstOrDefaultAsync(x => x.Id == int.Parse(Id));
        string hashed = PasswordHash(clave);
        propietario.Password_Hash = hashed;
        int result = await contexto.SaveChangesAsync();

        if (result == 0)
        {
            return BadRequest(new
            {
                status = "error",
                message = "No se pudo cambiar la clave"
            });
        }


        return Ok(new
        {
            status = "exito",
            message = "Clave cambiada correctamente"
        });
    }

    [HttpPost("usuario")]
    [AllowAnonymous]
    public async Task<IActionResult> Usuario([FromForm] string usuario)
    {
        var propietario = await contexto.Propietario.FirstOrDefaultAsync(x => x.Usuario == usuario);

        if (propietario != null)
        {

            // string dominio;
            // dominio = environment.IsDevelopment() ? HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() : "www.misitio.com";

            // // Obtener el nombre de host
            // string hostName = Dns.GetHostName();

            // // Obtener las direcciones IP del host
            // IPAddress[] addresses = Dns.GetHostAddresses(hostName);

            // foreach (IPAddress ip in addresses)
            // {
            //     // Solo mostramos las direcciones IPv4 (AddressFamily.InterNetwork)
            //     if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //     {   
            //         dominio = ip.ToString();
            //         Console.WriteLine("Dirección IPv4: " + ip);
            //     }
            // }
            GeneracionToken(propietario, out string jwtToken);

             //string link = $"https://6aa7-190-0-109-106.ngrok-free.app/api/propietarios/token?access_token={jwtToken}";
             string link = $"http://192.168.1.135:5182/api/propietarios/token?access_token={jwtToken}";
             //string link = $"https://www.clarin.com";

            await EnviarCorreoToken(link, propietario.Email);

            return Ok(new
            {
                status = "exito",
                message = "Email enviado"
            });

        }
        else
        {
            return BadRequest(new
            {
                status = "error",
                message = "No se encontro el email"
            });
        }
    }

    [HttpGet("token")]
    public async Task<IActionResult> Token()
    {
        var Id = User.FindFirst("Id")?.Value;
        var propietario = await contexto.Propietario.FirstOrDefaultAsync(x => x.Id == int.Parse(Id));


        Random rand = new Random(Environment.TickCount);
        string randomChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
        string nuevaClave = "";
        for (int i = 0; i < 8; i++)
        {
            nuevaClave += randomChars[rand.Next(0, randomChars.Length)];
        }

        string hashed = PasswordHash(nuevaClave);
        propietario.Password_Hash = hashed;
        int result = await contexto.SaveChangesAsync();

        if (result == 0)
        {
            return BadRequest(new
            {
                status = "error",
                message = "No se pudo cambiar la clave"
            });
        }
        else
        {
            await EnviarCorreoNuevaClave(nuevaClave, propietario.Email);
            return Ok(new
            {
                status = "exito",
                message = "Email enviado con su nueva password"
            });
        }

    }

    [HttpPatch("cambiaravatar")]
    public async Task<ActionResult> CambiarAvatar([FromForm] IFormFile avatar)
    {
        var Id = User.FindFirst("Id")?.Value;
        Propietario? propietario = contexto.Propietario.SingleOrDefault(x => x.Id == int.Parse(Id));


        string wwwPath = environment.WebRootPath;
        string path = Path.Combine(wwwPath, "propietario");
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        string fileName = "avatar_" + propietario.Id + Path.GetExtension(avatar.FileName);
        string pathCompleto = Path.Combine(path, fileName);
        propietario.Avatar_Url = Path.Combine("\\propietario", fileName);

        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
        {
            avatar.CopyTo(stream);
        }
    
        await contexto.SaveChangesAsync();

        return new JsonResult(new
        {
            status = "exito",
            message = "Se actualizo correctamente"
        });
    }

    private string PasswordHash(string password)
    {
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: password,
                                salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"] ?? ""),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
        return hashed;
    }

    private JwtSecurityToken GeneracionToken(Propietario propietarioBd, out string jwtToken)
    {
        var key = new SymmetricSecurityKey(
            System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
        var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, propietarioBd.Usuario),
                new Claim("Id", propietarioBd.Id.ToString()),
                new Claim("FullName", propietarioBd.Nombre + " " + propietarioBd.Apellido)
            };
        var expiracion = DateTime.Now.AddMinutes(60);
        
        var token = new JwtSecurityToken(
            issuer: config["TokenAuthentication:Issuer"],
            audience: config["TokenAuthentication:Audience"],
            claims: claims,
            expires: expiracion,
            signingCredentials: credenciales
        );
        jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        
        
        return token;
    }

    public async Task EnviarCorreoToken(string link, string email)
    {

        // Configuración del mensaje
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Inmobiliaria CA", "no-replay@sandbox89447560ebb34b72b11aa754b8457bf9.mailgun.org")); // Correo que envía (configurado en Mailgun)
        message.To.Add(new MailboxAddress("Destinatario", "arriola.mario.90@gmail.com")); // Correo del destinatario
        message.To.Add(new MailboxAddress("Destinatario", email)); // Correo del destinatario
        message.Subject = "Asunto del correo";

        // Cuerpo del correo
        message.Body = new TextPart("html")
        {
            Text = $@"<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Recuperar contraseña</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .email-container {{
            background-color: #ffffff;
            padding: 20px;
            margin: 30px auto;
            max-width: 600px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }}
        h2 {{
            color: #333333;
        }}
        p {{
            color: #555555;
        }}
        a {{
            display: inline-block;
            background-color: #007bff;
            color: #ffffff;
            padding: 10px 20px;
            text-decoration: none;
            border-radius: 5px;
        }}
        a:hover {{
            background-color: #0056b3;
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <h2>Recupera tu contraseña</h2>
        <p>Hola,</p>
        <p>Parece que has solicitado recuperar tu contraseña. Haz clic en el siguiente enlace para restablecerla:</p>
        <p><a href=""{link}"" target=""_blank"">Recuperar contraseña</a></p>
        <p>Si no solicitaste este cambio, puedes ignorar este mensaje.</p>
        <p>Saludos,</p>
        <p>El equipo de soporte</p>
    </div>
</body>
</html>
"
        };

        // Configuración del cliente SMTP
        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.CheckCertificateRevocation = false; // Opción para evitar problemas de certificación en entornos de desarrollo

            await client.ConnectAsync("sandbox.smtp.mailtrap.io", 587, MailKit.Security.SecureSocketOptions.StartTls);
            // await client.AuthenticateAsync("postmaster@sandbox89447560ebb34b72b11aa754b8457bf9.mailgun.org", "935ae8e2b1deb164d8bfb62f5735e877-d010bdaf-4e0ec541");
            await client.AuthenticateAsync("13dee342798801", "7bdcc0bb130c55");

            // Enviar correo
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }

    public async Task EnviarCorreoNuevaClave(string clave, string email)
    {

        // Configuración del mensaje
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Inmobiliaria CA", "no-replay@sandbox89447560ebb34b72b11aa754b8457bf9.mailgun.org")); // Correo que envía (configurado en Mailgun)
        message.To.Add(new MailboxAddress("Destinatario", "arriola.mario.90@gmail.com")); // Correo del destinatario
        message.To.Add(new MailboxAddress("Destinatario", email)); // Correo del destinatario
        message.Subject = "Asunto del correo";

        // Cuerpo del correo
        message.Body = new TextPart("html")
        {
            Text = $@"<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Nueva contraseña generada</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .email-container {{
            background-color: #ffffff;
            padding: 20px;
            margin: 30px auto;
            max-width: 600px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }}
        h2 {{
            color: #333333;
        }}
        p {{
            color: #555555;
        }}
        .password-box {{
            background-color: #f8f9fa;
            border: 1px solid #ced4da;
            padding: 10px;
            font-size: 18px;
            font-weight: bold;
            text-align: center;
            color: #333333;
            border-radius: 4px;
        }}
        .note {{
            font-size: 14px;
            color: #888888;
            margin-top: 20px;
        }}
    </style>
</head>
<body>
    <div class=""email-container"">
        <h2>Tu nueva contraseña</h2>
        <p>Hola,</p>
        <p>Se ha generado una nueva contraseña para tu cuenta. Utiliza la siguiente contraseña para iniciar sesión:</p>
        <div class=""password-box"">
            {clave}
        </div>
        <p class=""note"">Te recomendamos que cambies esta contraseña por una de tu preferencia una vez que hayas iniciado sesión.</p>
        <p>Si no solicitaste este cambio, por favor contacta con el soporte.</p>
        <p>Saludos,</p>
        <p>El equipo de soporte</p>
    </div>
</body>
</html>
"
        };

        // Configuración del cliente SMTP
        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.CheckCertificateRevocation = false; // Opción para evitar problemas de certificación en entornos de desarrollo

            await client.ConnectAsync("sandbox.smtp.mailtrap.io", 587, MailKit.Security.SecureSocketOptions.StartTls);
            // await client.AuthenticateAsync("postmaster@sandbox89447560ebb34b72b11aa754b8457bf9.mailgun.org", "935ae8e2b1deb164d8bfb62f5735e877-d010bdaf-4e0ec541");
            await client.AuthenticateAsync("13dee342798801", "7bdcc0bb130c55");
            // Enviar correo
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
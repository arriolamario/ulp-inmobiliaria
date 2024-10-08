using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Repositorio;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using InmobiliariaCA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Runtime.InteropServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaCA.Controllers;

public class UsuarioController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IRepositorioUsuario _repositorioUsuario;
    private IConfiguration _configuration;
    private IWebHostEnvironment environment;

    public UsuarioController(ILogger<HomeController> logger,
                        IWebHostEnvironment environment,
                        IRepositorioUsuario repositorioUsuario,
                        IConfiguration configuration)
    {
        _logger = logger;
        _repositorioUsuario = repositorioUsuario;
        _configuration = configuration;
        this.environment = environment;
    }

    [Authorize(Policy = "administrador")]
    public IActionResult Index()
    {
        return View(_repositorioUsuario.GetUsuarios());
    }
    [Authorize]
    public IActionResult Detalle(int Id)
    {
        if (!LogicaEmpleado(Id))
        {
            return RedirectToAction("AccesoDenegado", "Usuario");
        }

        var usuario = _repositorioUsuario.GetUsuario(Id);
        if (usuario == null)
        {
            return NotFound();
        }
        return View(usuario);
    }

    [HttpPost]
    [Authorize]
    public ActionResult UploadAvatar(int Id, IFormFile avatar)
    {
        if (!LogicaEmpleado(Id))
        {
            return RedirectToAction("AccesoDenegado", "Usuario");
        }
        bool resultSuccess = false;
        Usuario? u = _repositorioUsuario.GetUsuario(Id);

        if (u != null)
        {
            string wwwPath = environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = "avatar_" + u.Id + Path.GetExtension(avatar.FileName);
            string pathCompleto = Path.Combine(path, fileName);
            u.Avatar_Url = Path.Combine("\\Uploads", fileName);

            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
            {
                avatar.CopyTo(stream);
            }
            _repositorioUsuario.ActualizarUsuario(u);
            resultSuccess = true;
        }

        return new JsonResult(new { success = resultSuccess });
    }

    [Authorize]
    public IActionResult AltaEditar(int Id)
    {
        if (!LogicaEmpleado(Id))
        {
            return RedirectToAction("AccesoDenegado", "Usuario");
        }
        List<KeyValuePair<string, string>> roles = new List<KeyValuePair<string, string>>(){
            new KeyValuePair<string, string>("administrador", "Administrador"),
            new KeyValuePair<string, string>("empleado", "Empleado"),
        };

        ViewBag.Roles = new SelectList(roles, "Key", "Value");

        if (Id != 0)
        {
            var usuario = _repositorioUsuario.GetUsuario(Id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(new UsuarioAltaEditarViewModel(usuario));
        }

        return View(new UsuarioAltaEditarViewModel());
    }

    [HttpPost]
    [Authorize]
    public IActionResult AltaEditar(UsuarioAltaEditarViewModel usuario)
    {
        if (!LogicaEmpleado(usuario.Id))
        {
            return RedirectToAction("AccesoDenegado", "Usuario");
        }
        if (!ModelState.IsValid)
        {
            return RedirectToAction("AltaEditar", usuario);// Vuelva a mostrar el formulario con los errores(usuario);
        }
        if (usuario.Id == 0)
        {

            if (_repositorioUsuario.GetPorEmail(usuario.Email) == null)
            {
                Usuario u = new Usuario(usuario);
                u.Password_Hash = PasswordHash("123456");
                int result = _repositorioUsuario.InsertarUsuario(u);
                if (result > 0)
                    TempData["SuccessMessage"] = "Usuario insertado correctamente";
                else
                    TempData["ErrorMessage"] = "No se inserto el usuario";
            }
            else
            {
                ModelState.AddModelError("Email", "Email ya existente");
                List<KeyValuePair<string, string>> roles = new List<KeyValuePair<string, string>>(){
                    new KeyValuePair<string, string>("administrador", "Administrador"),
                    new KeyValuePair<string, string>("empleado", "Empleado"),
                };

                ViewBag.Roles = new SelectList(roles, "Key", "Value");
                
                return View("AltaEditar", usuario);
            }
        }
        else
        {
            Usuario? userBase = _repositorioUsuario.GetUsuario(usuario.Id);
            if(!(userBase != null && userBase.Email == usuario.Email) && _repositorioUsuario.GetPorEmail(usuario.Email) != null){
                ModelState.AddModelError("Email", "Email ya existente");
                List<KeyValuePair<string, string>> roles = new List<KeyValuePair<string, string>>(){
                    new KeyValuePair<string, string>("administrador", "Administrador"),
                    new KeyValuePair<string, string>("empleado", "Empleado"),
                };

                ViewBag.Roles = new SelectList(roles, "Key", "Value");
                
                return View("AltaEditar", usuario);
            }

            if(usuario.Id == 1){
                usuario.Rol = "administrador";
            }
            int result = _repositorioUsuario.ActualizarUsuario(new Usuario(usuario));
            if (result > 0)
                TempData["SuccessMessage"] = "Usuario actualizado correctamente";
            else
                TempData["ErrorMessage"] = "No se actualizo el usuario";
            return RedirectToAction("Detalle", "Usuario", new { Id = usuario.Id });
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize(Policy = "administrador")]
    public IActionResult Baja(int Id)
    {
        Usuario? userDb = _repositorioUsuario.GetUsuario(Id);
        if(userDb == null){
            TempData["ErrorMessage"] = "No se puede eliminar el usuario";
        } else if (userDb.Rol == "administrador" && !_repositorioUsuario.BorrarAdministrador()){
            TempData["ErrorMessage"] = "No se puede eliminar el usuario";
        }
        else
        {
            if (_repositorioUsuario.BajaUsuario(Id))
            {
                TempData["SuccessMessage"] = "Se elimino correctamente el usuario";
            }
            else
            {
                TempData["ErrorMessage"] = "No se puede eliminar el usuario";
            }
        }
        return RedirectToAction("Index");
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UsuarioLoginViewModel usuario)
    {
        if (ModelState.IsValid)
        {
            string hashed = PasswordHash(usuario.Password);

            var e = _repositorioUsuario.GetPorEmail(usuario.Email);
            if (e == null || e.Password_Hash != hashed)
            {
                ModelState.AddModelError("", "El email o el password no son correctos");
                return View();
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, e.Email),
                new Claim("Id", e.Id.ToString()),
                new Claim("FullName", e.Nombre + " " + e.Apellido),
                new Claim("AvatarUrl", e.Avatar_Url),
                new Claim(ClaimTypes.Role, e.Rol),
            };

            var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );
        return RedirectToAction("Login", "Usuario");
    }

    [Authorize]
    public IActionResult ResetPassword(int Id)
    {
        if (!LogicaEmpleado(Id))
        {
            return RedirectToAction("AccesoDenegado", "Usuario");
        }
        if (Id == 0)
        {
            return NotFound();
        }

        Usuario? u = _repositorioUsuario.GetUsuario(Id);
        if (u == null)
        {
            return NotFound();
        }

        return View(new UsuarioResetPasswordViewModel(u));
    }
    [HttpPost]
    [Authorize]
    public IActionResult ResetPassword(UsuarioResetPasswordViewModel model)
    {

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Lógica para verificar el token y cambiar la contraseña
        Usuario? usuario = _repositorioUsuario.GetPorEmail(model.Email);
        if (usuario != null && !LogicaEmpleado(usuario.Id))
        {
            return RedirectToAction("AccesoDenegado", "Usuario");
        }
        if (usuario == null)
        {
            ModelState.AddModelError("", "El email no existe");
            return View(model);
        }

        usuario.Password_Hash = PasswordHash(model.NewPassword);
        int result = _repositorioUsuario.ActualizarUsuario(usuario);
        if (result > 0)
        {
            TempData["SuccessMessage"] = "Se ha cambiado la contraseña correctamente";
            return RedirectToAction("Detalle", "Usuario", new { Id = usuario.Id });
        }

        ModelState.AddModelError("", "No se pudo cambiar la contraseña");

        return View(model);
    }

    [HttpPost]
    public IActionResult RemoveAvatar(int Id)
    {
        var usuario = _repositorioUsuario.GetUsuario(Id);
        if (usuario != null)
        {
            
            string pathCompleto = Path.Combine(environment.WebRootPath, usuario.Avatar_Url.Remove(0,1));
            System.IO.File.Delete(pathCompleto);
            usuario.Avatar_Url = "";
            _repositorioUsuario.ActualizarUsuario(usuario);
        }

        return RedirectToAction("Detalle", "Usuario", new { Id = Id });
    }

    private string PasswordHash(string password)
    {
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                                password: password,
                                salt: System.Text.Encoding.ASCII.GetBytes(_configuration["Salt"] ?? ""),
                                prf: KeyDerivationPrf.HMACSHA1,
                                iterationCount: 1000,
                                numBytesRequested: 256 / 8));
        return hashed;
    }

    private bool LogicaEmpleado(int Id)
    {
        if (!HttpContext.User.IsInRole("administrador"))
        {
            var idUsuarioCookie = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
            if (idUsuarioCookie != null && int.Parse(idUsuarioCookie) != Id)
            {
                return false;
            }
        }

        return true;
    }

    [AllowAnonymous]
    public IActionResult AccesoDenegado()
    {
        return View();
    }
}

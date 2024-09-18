using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Repositorio;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using InmobiliariaCA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Runtime.InteropServices;

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

    public IActionResult Index()
    {
        return View(_repositorioUsuario.GetUsuarios());
    }

    public IActionResult Detalle(int Id)
    {
        var usuario = _repositorioUsuario.GetUsuario(Id);
        if (usuario == null)
        {
            return NotFound();
        }
        return View(usuario);
    }

    [HttpPost]
    public ActionResult UploadAvatar(int Id, IFormFile avatar)
    {
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
            u.Avatar_Url = Path.Combine("/Uploads", fileName);

            using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
            {
                avatar.CopyTo(stream);
            }
            _repositorioUsuario.ActualizarUsuario(u);
            resultSuccess = true;
        }

        return new JsonResult(new { success = resultSuccess });
    }

    public IActionResult AltaEditar(int Id)
    {
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
    public IActionResult AltaEditar(UsuarioAltaEditarViewModel usuario)
    {
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
                //return View();
                return View("AltaEditar", usuario);
            }
        }
        else
        {
            int result = _repositorioUsuario.ActualizarUsuario(new Usuario(usuario));
            if (result > 0)
                TempData["SuccessMessage"] = "Usuario actualizado correctamente";
            else
                TempData["ErrorMessage"] = "No se actualizo el usuario";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Baja(int Id)
    {
        if (Id == 0)
        {
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

    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Login(UsuarioLoginViewModel usuario)
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
            // var claims = new List<Claim>
            // {
            // 	new Claim(ClaimTypes.Name, e.Email),
            // 	new Claim("FullName", e.Nombre + " " + e.Apellido),
            // 	new Claim(ClaimTypes.Role, e.RolNombre),
            // };

            // var claimsIdentity = new ClaimsIdentity(
            // 		claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // await HttpContext.SignInAsync(
            // 		CookieAuthenticationDefaults.AuthenticationScheme,
            // 		new ClaimsPrincipal(claimsIdentity));
            // TempData.Remove("returnUrl");
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    public IActionResult ResetPassword(int Id)
    {
        if (Id == 0)
        {
            return NotFound();
        }

        Usuario? u = _repositorioUsuario.GetUsuario(Id);
        if (u == null){
            return NotFound();
        }

        return View(new UsuarioResetPasswordViewModel(u));
    }
    [HttpPost]
    public IActionResult ResetPassword(UsuarioResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Lógica para verificar el token y cambiar la contraseña
        Usuario? usuario = _repositorioUsuario.GetPorEmail(model.Email);

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
}

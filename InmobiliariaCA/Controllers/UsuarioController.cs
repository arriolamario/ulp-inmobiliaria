using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Repositorio;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using InmobiliariaCA.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InmobiliariaCA.Controllers;

public class CropData
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
}

public class UsuarioController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IRepositorioUsuario _repositorioUsuario;

    public UsuarioController(ILogger<HomeController> logger,
                        IRepositorioUsuario repositorioUsuario)
    {
        _logger = logger;
        _repositorioUsuario = repositorioUsuario;
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
    public IActionResult UploadAvatar(IFormFile avatar, string CropData)
    {
        // if (avatar != null && avatar.Length > 0)
        // {
        //     var cropData = Newtonsoft.Json.JsonConvert.DeserializeObject<CropData>(CropData);
        //     if(cropData == null) {
        //         return BadRequest("CropData no válido");
        //     }


        //     using (var stream = avatar.OpenReadStream())
        //     using (var image = Image.Load(stream))
        //     {
        //         // Recorta la imagen según los datos de Cropper.js
        //         var rec = new Rectangle((int)cropData.X, (int)cropData.Y, (int)cropData.Width, (int)cropData.Height);
        //         image.Mutate(x => x.Crop(rec));

        //         // Redimensiona si es necesario
        //         image.Mutate(x => x.Resize(100, 100)); // Tamaño final del avatar

        //         // Guardar la imagen en el servidor
        //         var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatars", "avatar.jpg");
        //         image.Save(path);
        //     }

        //     return RedirectToAction("Success");
        // }

        return Redirect("Index");
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

            int result = _repositorioUsuario.InsertarUsuario(new Usuario(usuario));
            if (result > 0)
                TempData["SuccessMessage"] = "Usuario insertado correctamente";
            else
                TempData["ErrorMessage"] = "No se inserto el usuario";
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
}

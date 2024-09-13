using Microsoft.AspNetCore.Mvc;
using InmobiliariaCA.Repositorio;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using InmobiliariaCA.Models;

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
        if(usuario == null) {
            return NotFound();
        }
        return View(usuario);
    }

    [HttpPost]
    public IActionResult UploadAvatar(IFormFile avatar, string CropData)
    {
        if (avatar != null && avatar.Length > 0)
        {
            var cropData = Newtonsoft.Json.JsonConvert.DeserializeObject<CropData>(CropData);
            if(cropData == null) {
                return BadRequest("CropData no válido");
            }


            using (var stream = avatar.OpenReadStream())
            using (var image = Image.Load(stream))
            {
                // Recorta la imagen según los datos de Cropper.js
                var rec = new Rectangle((int)cropData.X, (int)cropData.Y, (int)cropData.Width, (int)cropData.Height);
                image.Mutate(x => x.Crop(rec));
                
                // Redimensiona si es necesario
                image.Mutate(x => x.Resize(100, 100)); // Tamaño final del avatar

                // Guardar la imagen en el servidor
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/avatars", "avatar.jpg");
                image.Save(path);
            }

            return RedirectToAction("Success");
        }

        return Redirect("Index");
    }

    public IActionResult AltaEditar(int Id, int idPropietario)
    {
        // var tiposInmuebles = _repositorioInmueble.GetTipoInmuebles();
        // var tiposInmueblesUsos = _repositorioInmueble.GetTipoInmueblesUsos();
        // ViewBag.TipoInmuebles = new SelectList(tiposInmuebles ?? new List<TipoInmueble>(), "Id", "Descripcion");
        // ViewBag.TipoInmueblesUsos = new SelectList(tiposInmueblesUsos ?? new List<TipoInmuebleUso>(), "Id", "Descripcion");
        // if (Id == 0)
        // {
        //     ViewBag.idPropietario = idPropietario;
        //     return View(new Inmueble());
        // }
        // var inmueble = _repositorioInmueble.GetInmueble(Id);
        // ViewBag.idPropietario = inmueble?.Id_Propietario;
        // return View(inmueble);
        return View(new Usuario());
    }

    // [HttpPost]
    // public IActionResult CrearActualizar(Inmueble inmueble)
    // {
    //     if (inmueble.Id == 0)
    //     {
    //         _repositorioInmueble.AltaInmueble(inmueble);
    //         TempData["SuccessMessage"] = "Inmueble insertado correctamente";
    //     }
    //     else
    //     {
    //         _repositorioInmueble.ActualizarInmueble(inmueble);
    //         TempData["SuccessMessage"] = "Inmueble actualizado correctamente";
    //     }
    //     return RedirectToAction("Index");
    // }

    // [HttpPost]
    // public IActionResult BajaLogica(int Id)
    // {
    //     if (Id == 0)
    //     {
    //     }
    //     else
    //     {
    //         if (_repositorioInmueble.BajaInmueble(Id))
    //         {
    //             TempData["SuccessMessage"] = "Se elimino correctamente el inmueble";
    //         }
    //         else
    //         {
    //             TempData["ErrorMessage"] = "No se puede eliminar el inmueble";
    //         }
    //     }
    //     return RedirectToAction("Index");
    // }
}

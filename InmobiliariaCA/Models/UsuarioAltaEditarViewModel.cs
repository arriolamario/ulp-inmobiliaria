using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InmobiliariaCA.Models;

namespace InmobiliariaCA.Models;

public class UsuarioAltaEditarViewModel
{
    public UsuarioAltaEditarViewModel()
    {
        
    }

    public UsuarioAltaEditarViewModel(Usuario usuario)
    {
        Id = usuario.Id;
        Email = usuario.Email;
        Nombre = usuario.Nombre;
        Apellido = usuario.Apellido;
        Rol = usuario.Rol;
    }

    public int Id { get; set; }
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    public string Email { get; set; } = "";
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = "";
    [Required(ErrorMessage = "El apellido es obligatorio.")]
    public string Apellido { get; set; } = "";
    [Required(ErrorMessage = "El rol es obligatorio.")]
    public string Rol { get; set; } = "";
}

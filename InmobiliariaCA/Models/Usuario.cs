using System;
using System.Collections.Generic;
using InmobiliariaCA.Models;

namespace InmobiliariaCA.Models;

public class Usuario
{
    public Usuario()
    {
        
    }
    public Usuario(UsuarioAltaEditarViewModel usuario)
    {
        this.Id = usuario.Id;
        this.Apellido = usuario.Apellido;
        this.Email = usuario.Email;
        this.Nombre = usuario.Nombre;
        this.Rol = usuario.Rol;
    }
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public string Password_Hash { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Apellido { get; set; } = "";
    public string Avatar_Url { get; set; } = "";
    public string Rol { get; set; } = "";
    public DateTime Fecha_Creacion { get; set; } = DateTime.Now;
    public DateTime Fecha_Actualizacion { get; set; } = DateTime.Now;
    public string NombreCompleto => $"{Apellido}, {Nombre}";
}

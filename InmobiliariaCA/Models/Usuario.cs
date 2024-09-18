using System;
using System.Collections.Generic;
using InmobiliariaCA.Models;
using InmobiliariaCA.Models.ContratoModels;

public class Usuario
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Apellido { get; set; } = "";
    public string Telefono { get; set; } = "";
    public string AvatarUrl { get; set; } = "";
    public string Rol { get; set; } = "";
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public DateTime FechaActualizacion { get; set; } = DateTime.Now;

    public virtual ICollection<Pago> PagosCreados { get; set; } = new List<Pago>();
    public virtual ICollection<Pago> PagosAnulados { get; set; } = new List<Pago>();
    public virtual ICollection<Contrato> ContratosCreados { get; set; } = new List<Contrato>();
    public string NombreCompleto => $"{Apellido}, {Nombre}";
}

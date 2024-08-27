namespace InmobiliariaCA.Models;
using System.ComponentModel.DataAnnotations;

public class Inmueble
{
    public int Id { get; set; }
    public string Direccion { get; set; } = "";
    
    public int Id_Tipo_Inmueble_Uso { get; set; }
    public TipoInmuebleUso? Tipo_Uso { get; set; }
    public int Id_Tipo_Inmueble { get; set; }
    public TipoInmueble? Tipo { get; set; }
    public int Ambientes { get; set; }
    public double Coordenada_Lat { get; set; }
    public double Coordenada_Lon { get; set; }
    public decimal Precio { get; set; }
    public int Estado { get; set; }
    public int Id_Propietario { get; set; }
    public Propietario? Propietario { get; set; }
    public DateTime Fecha_Creacion { get; set; }
    public DateTime Fecha_Actualizacion { get; set; }
}

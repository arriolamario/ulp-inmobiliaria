namespace InmobiliariaCA.Models;

public class TipoInmuebleUso
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = "";
    public DateTime Fecha_Creacion { get; set; }
    public DateTime Fecha_Actualizacion { get; set; }
}
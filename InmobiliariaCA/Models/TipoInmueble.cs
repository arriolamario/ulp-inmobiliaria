namespace InmobiliariaCA.Models;
public class TipoInmueble
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = "";
    public DateTime FechaCreacion { get; set; }
    public DateTime FechaActualizacion { get; set; }
}
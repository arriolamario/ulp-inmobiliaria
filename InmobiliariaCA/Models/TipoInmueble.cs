using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaCA.Models;
[Table("Tipo_Inmueble")]
public class TipoInmueble
{
    [Key]
    public int Id { get; set; }
    public string Descripcion { get; set; } = "";
    public DateTime Fecha_Creacion { get; set; }
    public DateTime Fecha_Actualizacion { get; set; }
}
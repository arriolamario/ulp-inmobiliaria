using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaCA.Models;
public class Contrato {
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El inmueble es obligatorio.")]
    [ForeignKey("Inmueble")]
    public int Id_Inmueble { get; set; }

    [Required(ErrorMessage = "El inquilino es obligatorio.")]
    [ForeignKey("Inquilino")]
    public int Id_Inquilino { get; set; }

    [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime Fecha_Desde { get; set; }

    [Required(ErrorMessage = "La fecha de finalización es obligatoria.")]
    [DataType(DataType.Date)]
       public DateTime Fecha_Hasta { get; set; }

    [Required(ErrorMessage = "El monto del alquiler es obligatorio.")]
    [Column("monto_alquiler", TypeName = "decimal(10, 2)")]
    [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser un valor positivo.")]
    public decimal Monto_Alquiler { get; set; }

    [DataType(DataType.Date)]
    public DateTime? Fecha_Finalizacion_Anticipada { get; set; }

    [Column("multa", TypeName = "decimal(10, 2)")]
    [Range(0, double.MaxValue, ErrorMessage = "La multa debe ser un valor positivo.")]
    public decimal? Multa { get; set; }

    [Required]
    [Column("estado")]
    public bool Estado { get; set; } = true;

    [Required(ErrorMessage = "El usuario de creación es obligatorio.")]
    [ForeignKey("UsuarioCreacion")]
    public int Id_Usuario_Creacion { get; set; }

    [ForeignKey("UsuarioFinalizacion")]
    public int? Id_Usuario_Finalizacion { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime Fecha_Creacion { get; set; } = DateTime.Now;

    [DataType(DataType.DateTime)]
    public DateTime Fecha_Actualizacion { get; set; } = DateTime.Now;

    public virtual Inmueble Inmueble { get; set; }
    public virtual Inquilino Inquilino { get; set; }
    public virtual Usuario Usuario_Creacion { get; set; }
    public virtual Usuario Usuario_Finalizacion { get; set; }
}

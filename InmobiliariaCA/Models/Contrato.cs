using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

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
    public DateTime Fecha_Desde { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "La fecha de finalización es obligatoria.")]
    [DataType(DataType.Date)]
    public DateTime Fecha_Hasta { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "El monto del alquiler es obligatorio.")]
    [Column("monto_alquiler", TypeName = "decimal(10, 2)")]
    [Range(0, double.MaxValue, ErrorMessage = "El monto debe ser un valor positivo.")]
    public decimal Monto_Alquiler { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? Fecha_Finalizacion_Anticipada { get; set; }

    [Column("multa", TypeName = "decimal(10, 2)")]
    [Range(0, double.MaxValue, ErrorMessage = "La multa debe ser un valor positivo.")]
    public decimal? Multa { get; set; }

    [Required(ErrorMessage = "El usuario de creación es obligatorio.")]
    [ForeignKey("UsuarioCreacion")]
    public int Id_Usuario_Creacion { get; set; }

    [ForeignKey("UsuarioFinalizacion")]
    public int? Id_Usuario_Finalizacion { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime Fecha_Creacion { get; set; } = DateTime.Now;

    [DataType(DataType.DateTime)]
    public DateTime Fecha_Actualizacion { get; set; } = DateTime.Now;

    public int Cantidad_Cuotas { get; set; }

    public int Cuotas_Pagas { get; set; }
    public bool Pagado { get; set; } = false;

    public virtual Inmueble? Inmueble { get; set; }
    public virtual Inquilino? Inquilino { get; set; }
    public virtual Usuario? Usuario_Creacion { get; set; }
    public virtual Usuario? Usuario_Finalizacion { get; set; }
    
    public string MontoAlquilerString() => Monto_Alquiler.ToString("C", CultureInfo.CreateSpecificCulture("es-AR"));

    public string? MultaString() => Multa?.ToString("C", CultureInfo.CreateSpecificCulture("es-AR"));

    public void MultaCalculada() {
        if (Fecha_Finalizacion_Anticipada.HasValue) {
            TimeSpan tiempoTranscurrido = Fecha_Finalizacion_Anticipada.Value.Subtract(Fecha_Desde);
            TimeSpan tiempoTotal = Fecha_Hasta.Subtract(Fecha_Desde);

            if (tiempoTranscurrido.TotalDays < tiempoTotal.TotalDays / 2) {
                Multa = Monto_Alquiler * 2;
            } else {
                Multa = Monto_Alquiler;
            }
        }
    }
}

namespace InmobiliariaCA.Models;
using System;
using System.ComponentModel.DataAnnotations;

public class Pago {

    public int Id { get; set; }

    [Required(ErrorMessage = "El ID del contrato es obligatorio.")]
    public int Contrato_Id { get; set; }

    [Required(ErrorMessage = "El número de pago es obligatorio.")]
    public int Numero_Pago { get; set; }

    [Required(ErrorMessage = "La fecha de pago es obligatoria.")]
    [DataType(DataType.Date, ErrorMessage = "Formato de fecha no válido.")]
    public DateTime Fecha_Pago { get; set; }

    [StringLength(255, ErrorMessage = "El detalle no puede exceder los 255 caracteres.")]
    public string Detalle { get; set; } = "";

    [Required(ErrorMessage = "El importe es obligatorio.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El importe debe ser mayor que cero.")]
    public decimal Importe { get; set; }

    public EstadoPago Estado { get; set; } = EstadoPago.Pagado;
    public int Creado_Por_Id { get; set; }
    public int? Anulado_Por_Id { get; set; }
    public DateTime? Fecha_Anulacion { get; set; }

    public virtual Contrato? Contrato { get; set; }
    public virtual Usuario? CreadoPor { get; set; }
    public virtual Usuario? AnuladoPor { get; set; }
}

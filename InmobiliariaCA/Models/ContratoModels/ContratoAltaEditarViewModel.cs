using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace InmobiliariaCA.Models.ContratoModels;
public class ContratoAltaEditarViewModel {


    public ContratoAltaEditarViewModel() { }

    public ContratoAltaEditarViewModel(Contrato contrato) {
        Id = contrato.Id;
        Id_Inmueble = contrato.Id_Inmueble;
        Id_Inquilino = contrato.Id_Inquilino;
        Monto_Alquiler = contrato.Monto_Alquiler;
        Fecha_Desde = contrato.Fecha_Desde;
        Fecha_Hasta = contrato.Fecha_Hasta;
        Id_Usuario_Creacion = contrato.Id_Usuario_Creacion;
    }
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

    [Required(ErrorMessage = "El usuario de creación es obligatorio.")]
    [ForeignKey("UsuarioCreacion")]
    public int Id_Usuario_Creacion { get; set; }
}

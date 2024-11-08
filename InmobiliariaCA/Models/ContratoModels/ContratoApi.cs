using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace InmobiliariaCA.Models.ContratoModels;
[Table("Contrato")]
public class ContratoApi {

    public ContratoApi()
    {
        
    }
    public int Id { get; set; }
    [ForeignKey("Inmueble")]
    public int Id_Inmueble { get; set; }
    public virtual Inmueble Inmueble { get; set; }
    [ForeignKey("Inquilino")]
    public int Id_Inquilino { get; set; }
    public virtual Inquilino Inquilino { get; set; }
    [DataType(DataType.Date)]
    public DateTime Fecha_Desde { get; set; } = DateTime.Today;
    [DataType(DataType.Date)]
    public DateTime Fecha_Hasta { get; set; } = DateTime.Today;
    //[Column("monto_alquiler", TypeName = "decimal(10, 2)")]
    public decimal Monto_Alquiler { get; set; }
    [NotMapped]
    public EstadoContrato Estado { 
        get => Enum.Parse<EstadoContrato>(EstadoString); 
        set => EstadoString = value.ToString(); 
    }

    [Column("estado")]
    public string EstadoString { get; set; }

    /*
    
    [DataType(DataType.Date)]
    public DateTime Fecha_Desde { get; set; } = DateTime.Today;
    [DataType(DataType.Date)]
    public DateTime Fecha_Hasta { get; set; } = DateTime.Today.AddDays(30);
    

    [DataType(DataType.Date)]
    public DateTime? Fecha_Finalizacion_Anticipada { get; set; }

    [Column("multa", TypeName = "decimal(10, 2)")]
    public decimal? Multa { get; set; }

    [ForeignKey("UsuarioCreacion")]
    public int Id_Usuario_Creacion { get; set; }
    public int? Id_Usuario_Finalizacion { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime Fecha_Creacion { get; set; } = DateTime.Now;

    [DataType(DataType.DateTime)]
    public DateTime Fecha_Actualizacion { get; set; } = DateTime.Now;

    public int Cantidad_Cuotas { get; set; }

    public int Cuotas_Pagas { get; set; }
    public bool Pagado { get; set; } = false;

    
    
    [ForeignKey(nameof(Id_Usuario_Creacion))]
    public virtual Usuario? Usuario_Creacion { get; set; }
    [ForeignKey(nameof(Id_Usuario_Finalizacion))]
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

    public bool PagosCompletos() => Cantidad_Cuotas == Cuotas_Pagas;

    public bool EsFinalizado() => this.PagosCompletos() || EstadoContrato.Finalizado == Estado;

    public int CantidadCuotas() {
        int meses = ((Fecha_Hasta.Year - Fecha_Desde.Year) * 12) + Fecha_Hasta.Month - Fecha_Desde.Month;
        return Math.Max(meses, 1);
    }

    */

}

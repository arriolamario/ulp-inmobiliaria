using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace InmobiliariaCA.Models.ContratoModels;
public class ContratoDetalleViewModel {

    public ContratoDetalleViewModel()
    {
        
    }

    public ContratoDetalleViewModel(Contrato contrato)
    {
        Id = contrato.Id;
        Fecha_Desde = contrato.Fecha_Desde;
        Fecha_Hasta = contrato.Fecha_Hasta;
        Monto_Alquiler = contrato.Monto_Alquiler;
        Fecha_Finalizacion_Anticipada = contrato.Fecha_Finalizacion_Anticipada;
        Multa = contrato.Multa;
        Cantidad_Cuotas = contrato.Cantidad_Cuotas;
        Cuotas_Pagas = contrato.Cuotas_Pagas;
        Inmueble = contrato.Inmueble;
        Inquilino = contrato.Inquilino;
        Estado = contrato.Estado;
    }

    public int Id { get; set; }
    public DateTime Fecha_Desde { get; set; } = DateTime.Today;
    public DateTime Fecha_Hasta { get; set; } = DateTime.Today.AddDays(30);
    public decimal Monto_Alquiler { get; set; }
    public DateTime? Fecha_Finalizacion_Anticipada { get; set; }
    public decimal? Multa { get; set; }
    public int Cantidad_Cuotas { get; set; }
    public int Cuotas_Pagas { get; set; }
    public virtual Inmueble? Inmueble { get; set; }
    public virtual Inquilino? Inquilino { get; set; }
    public EstadoContrato Estado { get; set; } = EstadoContrato.Vigente;
    public bool PagosCompletos() => Cantidad_Cuotas == Cuotas_Pagas;
    public bool EsFinalizado() => EstadoContrato.Finalizado == Estado;
    public List<Pago> Pagos { get; set; } = new List<Pago>();

}

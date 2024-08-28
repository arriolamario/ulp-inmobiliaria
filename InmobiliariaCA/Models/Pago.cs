using System;
using InmobiliariaCA.Models;

public class Pago
{
    public int Id { get; set; }
    public int ContratoId { get; set; }
    public int NumeroPago { get; set; }
    public DateTime FechaPago { get; set; }
    public string Detalle { get; set; }
    public decimal Importe { get; set; }
    public string Estado { get; set; }
    public int CreadoPorId { get; set; }
    public int? AnuladoPorId { get; set; }
    public DateTime? FechaAnulacion { get; set; }

    public virtual Contrato Contrato { get; set; }
    public virtual Usuario CreadoPor { get; set; }
    public virtual Usuario AnuladoPor { get; set; }
}

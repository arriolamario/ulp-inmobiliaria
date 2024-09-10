namespace InmobiliariaCA.Models;
using System;
public class Pago {
    public int Id { get; set; }
    public int Contrato_Id { get; set; }
    public int Numero_Pago { get; set; }
    public DateTime Fecha_Pago { get; set; }
    public string Detalle { get; set; } = "";
    public decimal Importe { get; set; }
    public string Estado { get; set; } = "";
    public int Creado_Por_Id { get; set; }
    public int? Anulado_Por_Id { get; set; }
    public DateTime? Fecha_Anulacion { get; set; }

    public virtual Contrato? Contrato { get; set; }
    public virtual Usuario? CreadoPor { get; set; }
    public virtual Usuario? AnuladoPor { get; set; }
}

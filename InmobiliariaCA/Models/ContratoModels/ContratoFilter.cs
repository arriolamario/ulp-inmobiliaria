namespace InmobiliariaCA.Models.ContratoModels;

    public class ContratoFilter {
        public int? ContratoId { get; set; }
        public int? InquilinoId { get; set; }
        public int? InmuebleId { get; set; }
        public EstadoContrato? Estado { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    } 
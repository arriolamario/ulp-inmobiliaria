namespace InmobiliariaCA.Models.ContratoModels;

    public class ContratoFilter {
        public int? ContratoId { get; set; }
        public int? InquilinoId { get; set; }
        public int? InmuebleId { get; set; }
        public EstadoContrato? Estado { get; set; }
        public DateTime? FechaDesde_Inicio { get; set; }
        public DateTime? FechaDesde_Fin { get; set; }
        public DateTime? FechaHasta_Inicio { get; set; }
        public DateTime? FechaHasta_Fin { get; set; }
    } 
namespace InmobiliariaCA.Models;

public class ContratoViewModel {
    public IEnumerable<Contrato> Contratos { get; set; } = new List<Contrato>();
    public ContratoFilter Filters { get; set; } = new ContratoFilter();
}
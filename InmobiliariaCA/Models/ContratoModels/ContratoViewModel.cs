namespace InmobiliariaCA.Models.ContratoModels;

public class ContratoViewModel {
    public IEnumerable<Contrato> Contratos { get; set; } = new List<Contrato>();
    public ContratoFilter Filters { get; set; } = new ContratoFilter();
    public ContratoAltaEditarViewModel ContratoAltaEditarViewModel { get; set; } = new ContratoAltaEditarViewModel();
    public Contrato Contrato { get; set; } = new Contrato();
    
}
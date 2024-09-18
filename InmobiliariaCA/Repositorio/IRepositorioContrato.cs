namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models.ContratoModels;
using System.Collections.Generic;

public interface IRepositorioContrato {
    List<Contrato> GetContratos();
    Contrato? GetContrato(int Id);
    int InsertarContrato(Contrato Contrato);
    int ActualizarContrato(Contrato Contrato);
    bool BajaContrato(int Id);   
    int ActualizarContratoPagado(int Id, int Pagado);
    List<Contrato> GetContratosFiltrados(ContratoFilter filter);
}


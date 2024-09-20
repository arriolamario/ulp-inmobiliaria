namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models.ContratoModels;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

public interface IRepositorioContrato {
    List<Contrato> GetContratos();
    Contrato? GetContrato(int Id);
    int InsertarContrato(Contrato Contrato);
    int ActualizarContrato(Contrato Contrato);
    bool BajaContrato(int Id);   
    int ActualizarContratoPagado(int Id, MySqlTransaction? transaction);
    List<Contrato> GetContratosFiltrados(ContratoFilter filter);
}


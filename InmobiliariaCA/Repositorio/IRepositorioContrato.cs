namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

public interface IRepositorioContrato
{
    List<Contrato> GetContratos();
    Contrato? GetContrato(int Id);
    int InsertarContrato(Contrato Contrato);
    int ActualizarContrato(Contrato Contrato);
    bool BajaContrato(int Id);

    int ActualizarContratoPagado(int Id, int Pagado);
}


namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

public interface IRepositorioContrato
{
    List<Contrato> GetContratos();
    Contrato? GetContrato(int id);
    int InsertarContrato(Contrato contrato);
    int ActualizarContrato(Contrato contrato);
    bool BajaContrato(int id);

    int ActualizarContratoPagado(int Id);
}


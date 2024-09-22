using InmobiliariaCA.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaCA.Repositorio;

public interface IRepositorioPropietario
{
    List<Propietario> GetPropietarios();
    Propietario? GetPropietario(int Id);
    bool ExistePropietarioPorDni(string dni);
    bool ActualizarPropietario(Propietario propietario);
    int InsertarPropietario(Propietario propietario);
    bool BajaPropietario(int Id);
    List<Propietario> GetPropietarios(List<int> propietariosIds);
}
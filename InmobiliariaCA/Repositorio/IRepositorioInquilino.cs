namespace InmobiliariaCA.Repositorio;

using InmobiliariaCA.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

public interface IRepositorioInquilino
{
    List<Inquilino> GetInquilinos();
    Inquilino? GetInquilino(int Id, MySqlTransaction? transaction);
    int InsertarInquilino(Inquilino inquilino);
    bool ActualizarInquilino(Inquilino inquilino);
    bool ExisteInquilinoPorDni(string dni);
    bool BajaInquilino(int id);
}
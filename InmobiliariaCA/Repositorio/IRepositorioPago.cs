namespace InmobiliariaCA.Repositorio;

using InmobiliariaCA.Models;
using MySql.Data.MySqlClient;

public interface IRepositorioPago
{
    List<Pago> GetPagos();
    Pago? GetPago(int Id);
    int InsertarPago(Pago Pago, MySqlTransaction? transaction);
    bool ActualizarPago(Pago Pago);
    bool AnularPago(int Id, int IdAnulador, int IdContrato);
}
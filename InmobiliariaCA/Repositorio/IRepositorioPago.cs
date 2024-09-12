namespace InmobiliariaCA.Repositorio;

using InmobiliariaCA.Models;

public interface IRepositorioPago
{
    List<Pago> GetPagos();
    Pago? GetPago(int Id);
    int InsertarPago(Pago Pago);
    bool ActualizarPago(Pago Pago);

    bool AnularPago(int Id, int IdAnulador);
}
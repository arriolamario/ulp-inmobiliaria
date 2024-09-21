namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;
using MySql.Data.MySqlClient;

public interface IRepositorioInmueble
{
    int AltaInmueble(Inmueble inmueble);
    
    List<Inmueble> GetInmuebles();
    List<Inmueble> GetInmuebles(int IdPropietario);
    List<Inmueble> GetInmueblesSinUso();
    Inmueble? GetInmueble(int Id, MySqlTransaction? transaction);
    bool BajaInmueble(int Id);
    bool ActualizarInmueble(Inmueble Inmueble);
    bool EsInmuebleDisponible(int IdInmueble, DateTime FechaDesde, DateTime FechaHasta);
    List<TipoInmueble> GetTipoInmuebles();
    List<TipoInmuebleUso> GetTipoInmueblesUsos();
}
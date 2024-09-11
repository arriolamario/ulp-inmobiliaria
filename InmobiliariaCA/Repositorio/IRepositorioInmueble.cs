namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;

public interface IRepositorioInmueble
{
    int AltaInmueble(Inmueble inmueble);
    
    List<Inmueble> GetInmuebles();
    List<Inmueble> GetInmuebles(int idPropietario);
    List<Inmueble> GetInmueblesSinUso();
    Inmueble? GetInmueble(int Id);
    bool BajaInmueble(int id);
    bool ActualizarInmueble(Inmueble inmueble);
    List<TipoInmueble> GetTipoInmuebles();
    List<TipoInmuebleUso> GetTipoInmueblesUsos();
}
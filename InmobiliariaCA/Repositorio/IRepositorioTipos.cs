namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;

public interface IRepositorioTipos
{
    int AltaTipoInmueble(TipoInmueble tipoInmueble);
    List<TipoInmueble> GetTipoInmuebles();
    TipoInmueble? GetTipoInmueble(int Id);
    bool ExisteRelacionTipoInmueble(int Id);
    bool BajaTipoInmueble(int Id);
    bool UpdateTipoInmueble(TipoInmueble tipo);
    int AltaTipoInmuebleUso(TipoInmuebleUso tipo);
    List<TipoInmuebleUso> GetTipoInmueblesUsos();
    TipoInmuebleUso? GetTipoInmuebleUso(int Id);
    bool BajaTipoInmuebleUso(int Id);
    bool ExisteRelacionTipoInmuebleUso(int Id);
    bool UpdateTipoInmuebleUso(TipoInmuebleUso tipo);
}
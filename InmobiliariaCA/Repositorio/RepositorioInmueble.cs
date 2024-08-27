namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;

public class RepositorioInmueble : RepositorioBase
{
    private RepositorioPropietario _repositorioPropietario;
    private RepositorioTipos _repositorioTipos;
    public RepositorioInmueble(IConfiguration configuration) : base(configuration)
    {
        _repositorioPropietario = new RepositorioPropietario(configuration);
        _repositorioTipos = new RepositorioTipos(configuration);
    }
    public int AltaInmueble(Inmueble inmueble)
    {
        int result = 0;
        string query = @$"INSERT INTO inmueble(
                            {nameof(Inmueble.Direccion)}, 
                            {nameof(Inmueble.Id_Tipo_Inmueble_Uso)},
                            {nameof(Inmueble.Id_Tipo_Inmueble)},
                            {nameof(Inmueble.Ambientes)},
                            {nameof(Inmueble.Coordenada_Lat)}, 
                            {nameof(Inmueble.Coordenada_Lon)}, 
                            {nameof(Inmueble.Precio)},
                            {nameof(Inmueble.Estado)},
                            {nameof(Inmueble.Id_Propietario)})
                            VALUES (@{nameof(Inmueble.Direccion)},
                            @{nameof(Inmueble.Id_Tipo_Inmueble_Uso)},
                            @{nameof(Inmueble.Id_Tipo_Inmueble)},
                            @{nameof(Inmueble.Ambientes)},
                            @{nameof(Inmueble.Coordenada_Lat)}, 
                            @{nameof(Inmueble.Coordenada_Lon)}, 
                            @{nameof(Inmueble.Precio)},
                            @{nameof(Inmueble.Estado)},
                            @{nameof(Inmueble.Id_Propietario)});
                            SELECT LAST_INSERT_ID();";
        
        result = this.ExecuteScalar(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Inmueble.Direccion)}", inmueble.Direccion);
            parameters.AddWithValue($"@{nameof(Inmueble.Id_Tipo_Inmueble_Uso)}", inmueble.Id_Tipo_Inmueble_Uso);
            parameters.AddWithValue($"@{nameof(Inmueble.Id_Tipo_Inmueble)}", inmueble.Id_Tipo_Inmueble);
            parameters.AddWithValue($"@{nameof(Inmueble.Ambientes)}", inmueble.Ambientes);
            parameters.AddWithValue($"@{nameof(Inmueble.Coordenada_Lat)}", inmueble.Coordenada_Lat);
            parameters.AddWithValue($"@{nameof(Inmueble.Coordenada_Lon)}", inmueble.Coordenada_Lon);
            parameters.AddWithValue($"@{nameof(Inmueble.Precio)}", inmueble.Precio);
            parameters.AddWithValue($"@{nameof(Inmueble.Estado)}", inmueble.Estado);
            parameters.AddWithValue($"@{nameof(Inmueble.Id_Propietario)}", inmueble.Id_Propietario);
        });

        return result;
    }
    public List<Inmueble> GetInmuebles()
    {
        List<Inmueble> resultInmuebles = new List<Inmueble>();

        string query = @$"select {nameof(Inmueble.Id)}, 
                                {nameof(Inmueble.Direccion)},
                                {nameof(Inmueble.Id_Tipo_Inmueble_Uso)},
                                {nameof(Inmueble.Id_Tipo_Inmueble)},
                                {nameof(Inmueble.Ambientes)},
                                {nameof(Inmueble.Coordenada_Lat)}, 
                                {nameof(Inmueble.Coordenada_Lon)}, 
                                {nameof(Inmueble.Precio)},
                                {nameof(Inmueble.Estado)},
                                {nameof(Inmueble.Id_Propietario)},
                                {nameof(Inmueble.Fecha_Creacion)},
                                {nameof(Inmueble.Fecha_Actualizacion)}		
                                from inmueble where Estado = 1;";

        resultInmuebles = this.ExecuteReaderList<Inmueble>(query, (reader) =>  {
            return new Inmueble()
            {
                Id = int.Parse(reader[nameof(Inmueble.Id)].ToString() ?? "0"),
                Direccion = reader["direccion"].ToString() ?? "",
                Id_Tipo_Inmueble_Uso = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble_Uso)].ToString() ?? "0"),
                Id_Tipo_Inmueble = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble)].ToString() ?? "0"),
                Ambientes = int.Parse(reader[nameof(Inmueble.Ambientes)].ToString() ?? "0"),
                Coordenada_Lat = double.Parse(reader[nameof(Inmueble.Coordenada_Lat)].ToString() ?? "0"),
                Coordenada_Lon = double.Parse(reader[nameof(Inmueble.Coordenada_Lon)].ToString() ?? "0"),
                Precio = decimal.Parse(reader[nameof(Inmueble.Precio)].ToString() ?? "0"),
                Estado = int.Parse(reader[nameof(Inmueble.Estado)].ToString() ?? "0"),
                Id_Propietario = int.Parse(reader[nameof(Inmueble.Id_Propietario)].ToString() ?? "0"),
                Fecha_Creacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Creacion)].ToString() ?? "0"),
                Fecha_Actualizacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Actualizacion)].ToString() ?? "0")
            };
        }); 

        List<int> propietariosIds = resultInmuebles.Select(x => x.Id_Propietario).ToList();

        var propietarios = _repositorioPropietario.GetPropietarios(propietariosIds);
        var tiposInmuebles = _repositorioTipos.GetTipoInmuebles();
        var tiposInmueblesUsos = _repositorioTipos.GetTipoInmueblesUsos();

        resultInmuebles.ForEach(x => {
            x.Propietario = propietarios.FirstOrDefault(y => y.Id == x.Id_Propietario);
            x.Tipo = tiposInmuebles.FirstOrDefault(y => y.Id == x.Id_Tipo_Inmueble);
            x.Tipo_Uso = tiposInmueblesUsos.FirstOrDefault(y => y.Id == x.Id_Tipo_Inmueble_Uso);
        });

        return resultInmuebles;
    }

    public Inmueble? GetInmueble(int Id)
    {
        Inmueble? result = null;
        return result;
    }

    public bool BajaLogicaInmueble(int id)
    {
        bool result = false;
        return result;
    }

    public bool ActualizarInmueble(Inmueble inmueble){
        throw new Exception("Funcionalidad no implementada");
    }

    public List<TipoInmueble> GetTipoInmuebles()
    {
        var resultList = _repositorioTipos.GetTipoInmuebles();
        return resultList;
    }

    public List<TipoInmuebleUso> GetTipoInmueblesUsos()
    {
        var resultList = _repositorioTipos.GetTipoInmueblesUsos();
        return resultList;
    }
}
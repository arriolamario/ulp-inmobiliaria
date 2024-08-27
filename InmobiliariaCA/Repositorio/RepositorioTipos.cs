namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;

public class RepositorioTipos : RepositorioBase
{
    public RepositorioTipos(IConfiguration configuration) : base(configuration)
    {
    }
    #region TipoInmueble
    public int AltaTipoInmueble(TipoInmueble tipoInmueble)
    {
        int result = 0;
        string query = @$"INSERT INTO tipo_inmueble(
                            {nameof(tipoInmueble.Descripcion)}) 
                            VALUES (@{nameof(tipoInmueble.Descripcion)});,
                            SELECT LAST_INSERT_ID();";
        
        result = this.ExecuteScalar(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(TipoInmueble.Descripcion)}", tipoInmueble.Descripcion);
        });

        return result;
    }
    public List<TipoInmueble> GetTipoInmueble()
    {
        List<TipoInmueble> resultInmuebles = new List<TipoInmueble>();

        string query = @$"select {nameof(TipoInmueble.Id)}, 
                                {nameof(TipoInmueble.Descripcion)},
                                {nameof(TipoInmueble.Fecha_Creacion)},
                                {nameof(TipoInmueble.Fecha_Actualizacion)}
                                from tipo_inmueble;";

        resultInmuebles = this.ExecuteReaderList<TipoInmueble>(query, (reader) =>  {
            return new TipoInmueble()
            {
                Id = reader.GetInt32(nameof(TipoInmueble.Id)),
                Descripcion = reader.GetString(nameof(TipoInmueble.Descripcion)),
                Fecha_Creacion = reader.GetDateTime(nameof(TipoInmueble.Fecha_Creacion)),
                Fecha_Actualizacion = reader.GetDateTime(nameof(TipoInmueble.Fecha_Actualizacion))
            };
        }); 

        return resultInmuebles;
    }

    public TipoInmueble? GetTipoInmueble(int Id)
    {
        TipoInmueble? result = null;

        string query = $@"select {nameof(TipoInmueble.Id)},
                                {nameof(TipoInmueble.Descripcion)},
                                {nameof(TipoInmueble.Fecha_Creacion)},
                                {nameof(TipoInmueble.Fecha_Actualizacion)}
                                from tipo_inmueble
                                where {nameof(TipoInmueble.Id)} = @Id;";

        result = this.ExecuteReader(query, 
        (parameters) => {
            parameters.AddWithValue("@Id", Id);
        },
        (reader) =>  {
            return new TipoInmueble()
            {
                Id = reader.GetInt32(nameof(TipoInmueble.Id)),
                Descripcion = reader.GetString(nameof(TipoInmueble.Descripcion)),
                Fecha_Creacion = reader.GetDateTime(nameof(TipoInmueble.Fecha_Creacion)),
                Fecha_Actualizacion = reader.GetDateTime(nameof(TipoInmueble.Fecha_Actualizacion))
            };
        });

        return result;
    }
    #endregion

    #region TipoInmuebleUso
    public int AltaTipoInmuebleUso(Inmueble inmueble)
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
    public List<TipoInmuebleUso> GetTipoInmuebleUso()
    {
        List<TipoInmuebleUso> result = new List<TipoInmuebleUso>();

        string query = @$"select {nameof(TipoInmuebleUso.Id)},
                                {nameof(TipoInmuebleUso.Descripcion)},
                                {nameof(TipoInmuebleUso.Fecha_Creacion)},
                                {nameof(TipoInmuebleUso.Fecha_Actualizacion)}
                                from tipo_inmueble_uso;";

        result = this.ExecuteReaderList<TipoInmuebleUso>(query, (reader) =>  {
            return new TipoInmuebleUso()
            {
                Id = reader.GetInt32(nameof(TipoInmuebleUso.Id)),
                Descripcion = reader.GetString(nameof(TipoInmuebleUso.Descripcion)),
                Fecha_Creacion = reader.GetDateTime(nameof(TipoInmuebleUso.Fecha_Creacion)),
                Fecha_Actualizacion = reader.GetDateTime(nameof(TipoInmuebleUso.Fecha_Actualizacion))
            };
        }); 

        return result;
    }

    public TipoInmuebleUso? GetTipoInmuebleUso(int Id)
    {
        TipoInmuebleUso? result = null;

        string query = $@"select {nameof(TipoInmuebleUso.Id)},
                                {nameof(TipoInmuebleUso.Descripcion)},
                                {nameof(TipoInmuebleUso.Fecha_Creacion)},
                                {nameof(TipoInmuebleUso.Fecha_Actualizacion)}
                                from tipo_inmueble_uso
                                where {nameof(TipoInmueble.Id)} = @Id;";

        result = this.ExecuteReader(query, 
        (parameters) => {
            parameters.AddWithValue("@Id", Id);
        },
        (reader) =>  {
            return new TipoInmuebleUso()
            {
                Id = reader.GetInt32(nameof(TipoInmuebleUso.Id)),
                Descripcion = reader.GetString(nameof(TipoInmuebleUso.Descripcion)),
                Fecha_Creacion = reader.GetDateTime(nameof(TipoInmuebleUso.Fecha_Creacion)),
                Fecha_Actualizacion = reader.GetDateTime(nameof(TipoInmuebleUso.Fecha_Actualizacion))
            };
        });

        return result;
    }
    #endregion
}
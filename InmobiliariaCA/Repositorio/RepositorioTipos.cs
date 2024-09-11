namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;

public class RepositorioTipos : RepositorioBase, IRepositorioTipos
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
                            VALUES (@{nameof(tipoInmueble.Descripcion)});
                            SELECT LAST_INSERT_ID();";
        
        result = this.ExecuteScalar(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(TipoInmueble.Descripcion)}", tipoInmueble.Descripcion);
        });

        return result;
    }
    public List<TipoInmueble> GetTipoInmuebles()
    {
        List<TipoInmueble> resultInmuebles = new List<TipoInmueble>();

        string query = @$"select {nameof(TipoInmueble.Id)}, 
                                {nameof(TipoInmueble.Descripcion)},
                                {nameof(TipoInmueble.Estado)},
                                {nameof(TipoInmueble.Fecha_Creacion)},
                                {nameof(TipoInmueble.Fecha_Actualizacion)}
                                from tipo_inmueble
                                where {nameof(TipoInmueble.Estado)} = 1;";

        resultInmuebles = this.ExecuteReaderList<TipoInmueble>(query, 
            (parameters) => {},
            (reader) =>  {
            return new TipoInmueble()
            {
                Id = reader.GetInt32(nameof(TipoInmueble.Id)),
                Descripcion = reader.GetString(nameof(TipoInmueble.Descripcion)),
                Estado = reader.GetInt32(nameof(TipoInmueble.Estado)),
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
                                {nameof(TipoInmueble.Estado)},
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
                Estado = reader.GetInt32(nameof(TipoInmueble.Estado)),
                Fecha_Creacion = reader.GetDateTime(nameof(TipoInmueble.Fecha_Creacion)),
                Fecha_Actualizacion = reader.GetDateTime(nameof(TipoInmueble.Fecha_Actualizacion))
            };
        });

        return result;
    }
    
    public bool ExisteRelacionTipoInmueble(int Id){
        bool result = false;
        string query = $@"select count(*) from inmueble i join tipo_inmueble ti on (i.id_tipo_inmueble = ti.id) where ti.id = @Id;";

        result = this.ExecuteScalar(query, (parameters) => {
            parameters.AddWithValue("@Id", Id);
        }) > 0;
        return result;
    }

    public bool BajaTipoInmueble(int Id)
    {
        bool result = false;
        string query = @$"UPDATE tipo_inmueble SET {nameof(TipoInmueble.Estado)} = 0 WHERE {nameof(TipoInmueble.Id)} = @{nameof(TipoInmueble.Id)};";

        result = 0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(TipoInmueble.Id)}", Id);
        });

        return result;
    }
    public bool UpdateTipoInmueble(TipoInmueble tipo)
    {
        bool result = false;
        string query = @$"UPDATE tipo_inmueble SET {nameof(TipoInmueble.Descripcion)} = @{nameof(TipoInmueble.Descripcion)} WHERE {nameof(TipoInmueble.Id)} = @{nameof(TipoInmueble.Id)};";

        result = 0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(TipoInmueble.Id)}", tipo.Id);
            parameters.AddWithValue($"@{nameof(TipoInmueble.Descripcion)}", tipo.Descripcion);
        });

        return result;
    }
    #endregion

    #region TipoInmuebleUso
    public int AltaTipoInmuebleUso(TipoInmuebleUso tipo)
    {
        int result = 0;
        string query = @$"INSERT INTO tipo_inmueble_uso(
                            {nameof(TipoInmuebleUso.Descripcion)}) 
                            VALUES (@{nameof(TipoInmuebleUso.Descripcion)});
                            SELECT LAST_INSERT_ID();";
        
        result = this.ExecuteScalar(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(TipoInmuebleUso.Descripcion)}", tipo.Descripcion);
        });

        return result;
    }
    public List<TipoInmuebleUso> GetTipoInmueblesUsos()
    {
        List<TipoInmuebleUso> result = new List<TipoInmuebleUso>();

        string query = @$"select {nameof(TipoInmuebleUso.Id)},
                                {nameof(TipoInmuebleUso.Descripcion)},
                                {nameof(TipoInmuebleUso.Estado)},
                                {nameof(TipoInmuebleUso.Fecha_Creacion)},
                                {nameof(TipoInmuebleUso.Fecha_Actualizacion)}
                                from tipo_inmueble_uso
                                where {nameof(TipoInmuebleUso.Estado)} = 1;";

        result = this.ExecuteReaderList<TipoInmuebleUso>(query, (parameters) => {}, (reader) =>  {
            return new TipoInmuebleUso()
            {
                Id = reader.GetInt32(nameof(TipoInmuebleUso.Id)),
                Descripcion = reader.GetString(nameof(TipoInmuebleUso.Descripcion)),
                Estado = reader.GetInt32(nameof(TipoInmuebleUso.Estado)),
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
                                {nameof(TipoInmuebleUso.Estado)},
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
                Estado = reader.GetInt32(nameof(TipoInmuebleUso.Estado)),
                Fecha_Creacion = reader.GetDateTime(nameof(TipoInmuebleUso.Fecha_Creacion)),
                Fecha_Actualizacion = reader.GetDateTime(nameof(TipoInmuebleUso.Fecha_Actualizacion))
            };
        });

        return result;
    }

    public bool BajaTipoInmuebleUso(int Id)
    {
        bool result = false;
        string query = @$"UPDATE tipo_inmueble_uso SET {nameof(TipoInmuebleUso.Estado)} = 0 WHERE {nameof(TipoInmuebleUso.Id)} = @{nameof(TipoInmuebleUso.Id)};";

        result = 0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Propietario.Id)}", Id);
        });

        return result;
    }

    public bool ExisteRelacionTipoInmuebleUso(int Id){
        bool result = false;
        string query = $@"select count(*) from inmueble i join tipo_inmueble_uso ti on (i.id_tipo_inmueble_uso = ti.id) where ti.id = @Id;";

        result = this.ExecuteScalar(query, (parameters) => {
            parameters.AddWithValue("@Id", Id);
        }) > 0;
        return result;
    }

    public bool UpdateTipoInmuebleUso(TipoInmuebleUso tipo)
    {
        bool result = false;
        string query = @$"UPDATE tipo_inmueble_uso SET {nameof(TipoInmuebleUso.Descripcion)} = @{nameof(TipoInmuebleUso.Descripcion)} WHERE {nameof(TipoInmuebleUso.Id)} = @{nameof(TipoInmuebleUso.Id)};";

        result = 0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(TipoInmuebleUso.Id)}", tipo.Id);
            parameters.AddWithValue($"@{nameof(TipoInmuebleUso.Descripcion)}", tipo.Descripcion);
        });

        return result;
    }
    #endregion
}
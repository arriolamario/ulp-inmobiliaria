namespace InmobiliariaCA.Repositorio;

using System.Data;
using InmobiliariaCA.Models;
using MySql.Data.MySqlClient;

public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
{
    private IRepositorioPropietario _repositorioPropietario;
    private IRepositorioTipos _repositorioTipos;
    private readonly ILogger<RepositorioInmueble> _logger;
    public RepositorioInmueble(
        IConfiguration configuration, 
        IRepositorioPropietario repositorioPropietario, 
        IRepositorioTipos repositorioTipos,
        ILogger<RepositorioInmueble> logger) : base(configuration)
    {
        _logger = logger;
        _repositorioPropietario = repositorioPropietario;
        _repositorioTipos = repositorioTipos;
    }
    public int AltaInmueble(Inmueble inmueble) {
        int result = 0;
        string query = @$"INSERT INTO inmueble(
                            {nameof(Inmueble.Direccion)}, 
                            {nameof(Inmueble.Id_Tipo_Inmueble_Uso)},
                            {nameof(Inmueble.Id_Tipo_Inmueble)},
                            {nameof(Inmueble.Ambientes)},
                            {nameof(Inmueble.Coordenada_Lat)}, 
                            {nameof(Inmueble.Coordenada_Lon)}, 
                            {nameof(Inmueble.Precio)},
                            {nameof(Inmueble.Id_Propietario)},
                            {nameof(Inmueble.Activo)})
                            VALUES (@{nameof(Inmueble.Direccion)},
                            @{nameof(Inmueble.Id_Tipo_Inmueble_Uso)},
                            @{nameof(Inmueble.Id_Tipo_Inmueble)},
                            @{nameof(Inmueble.Ambientes)},
                            @{nameof(Inmueble.Coordenada_Lat)}, 
                            @{nameof(Inmueble.Coordenada_Lon)}, 
                            @{nameof(Inmueble.Precio)},
                            @{nameof(Inmueble.Id_Propietario)},
                            @{nameof(Inmueble.Activo)});
                            SELECT LAST_INSERT_ID();";
        
        result = this.ExecuteScalar(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Inmueble.Direccion)}", inmueble.Direccion);
            parameters.AddWithValue($"@{nameof(Inmueble.Id_Tipo_Inmueble_Uso)}", inmueble.Id_Tipo_Inmueble_Uso);
            parameters.AddWithValue($"@{nameof(Inmueble.Id_Tipo_Inmueble)}", inmueble.Id_Tipo_Inmueble);
            parameters.AddWithValue($"@{nameof(Inmueble.Ambientes)}", inmueble.Ambientes);
            parameters.AddWithValue($"@{nameof(Inmueble.Coordenada_Lat)}", inmueble.Coordenada_Lat);
            parameters.AddWithValue($"@{nameof(Inmueble.Coordenada_Lon)}", inmueble.Coordenada_Lon);
            parameters.AddWithValue($"@{nameof(Inmueble.Precio)}", inmueble.Precio);
            parameters.AddWithValue($"@{nameof(Inmueble.Id_Propietario)}", inmueble.Id_Propietario);
            parameters.AddWithValue($"@{nameof(Inmueble.Activo)}", inmueble.Activo);
        });

        return result;
    }
    public List<Inmueble> GetInmuebles() {
        List<Inmueble> resultInmuebles = new List<Inmueble>();

        string query = @$"select {nameof(Inmueble.Id)}, 
                                {nameof(Inmueble.Direccion)},
                                {nameof(Inmueble.Id_Tipo_Inmueble_Uso)},
                                {nameof(Inmueble.Id_Tipo_Inmueble)},
                                {nameof(Inmueble.Ambientes)},
                                {nameof(Inmueble.Coordenada_Lat)}, 
                                {nameof(Inmueble.Coordenada_Lon)}, 
                                {nameof(Inmueble.Precio)},
                                {nameof(Inmueble.Id_Propietario)},
                                {nameof(Inmueble.Fecha_Creacion)},
                                {nameof(Inmueble.Fecha_Actualizacion)},
                                {nameof(Inmueble.Activo)}		
                                from inmueble;";

        resultInmuebles = this.ExecuteReaderList<Inmueble>(query, (parameters) => {}, (reader) =>  {
            return new Inmueble()
            {
                Id = int.Parse(reader[nameof(Inmueble.Id)].ToString() ?? "0"),
                Direccion = reader["direccion"].ToString() ?? "",
                Id_Tipo_Inmueble_Uso = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble_Uso)].ToString() ?? "0"),
                Id_Tipo_Inmueble = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble)].ToString() ?? "0"),
                Ambientes = int.Parse(reader[nameof(Inmueble.Ambientes)].ToString() ?? "0"),
                Coordenada_Lat = reader[nameof(Inmueble.Coordenada_Lat)].ToString() ?? "",
                Coordenada_Lon = reader[nameof(Inmueble.Coordenada_Lon)].ToString() ?? "",
                Precio = decimal.Parse(reader[nameof(Inmueble.Precio)].ToString() ?? "0"),
                Id_Propietario = int.Parse(reader[nameof(Inmueble.Id_Propietario)].ToString() ?? "0"),
                Fecha_Creacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Creacion)].ToString() ?? "0"),
                Fecha_Actualizacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Actualizacion)].ToString() ?? "0"),
                Activo = bool.Parse(reader[nameof(Inmueble.Activo)].ToString() ?? "0")
            };
        }); 

        List<int> propietariosIds = resultInmuebles.Select(x => x.Id_Propietario).ToList();

        var propietarios = _repositorioPropietario.GetPropietarios(propietariosIds);
        var tiposInmuebles = _repositorioTipos.GetTipoInmuebles();
        var tiposInmueblesUsos = _repositorioTipos.GetTipoInmueblesUsos();

        resultInmuebles.ForEach(x => {
            x.Propietario = propietarios.FirstOrDefault(y => y.Id == x.Id_Propietario)?? new Propietario();
            x.Tipo = tiposInmuebles.FirstOrDefault(y => y.Id == x.Id_Tipo_Inmueble);
            x.Tipo_Uso = tiposInmueblesUsos.FirstOrDefault(y => y.Id == x.Id_Tipo_Inmueble_Uso);
        });

        return resultInmuebles;
    }

    public List<Inmueble> GetInmuebles(bool Activo) {
        List<Inmueble> resultInmuebles = new List<Inmueble>();
        
        string query = @$"select {nameof(Inmueble.Id)}, 
                                {nameof(Inmueble.Direccion)},
                                {nameof(Inmueble.Id_Tipo_Inmueble_Uso)},
                                {nameof(Inmueble.Id_Tipo_Inmueble)},
                                {nameof(Inmueble.Ambientes)},
                                {nameof(Inmueble.Coordenada_Lat)}, 
                                {nameof(Inmueble.Coordenada_Lon)}, 
                                {nameof(Inmueble.Precio)},
                                {nameof(Inmueble.Id_Propietario)},
                                {nameof(Inmueble.Fecha_Creacion)},
                                {nameof(Inmueble.Fecha_Actualizacion)},
                                {nameof(Inmueble.Activo)}		
                                from inmueble
                                where {nameof(Inmueble.Activo)} = @{nameof(Activo)};";

        resultInmuebles = this.ExecuteReaderList<Inmueble>(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Activo)}", Activo);
        }, (reader) =>  {
            return new Inmueble()
            {
                Id = int.Parse(reader[nameof(Inmueble.Id)].ToString() ?? "0"),
                Direccion = reader["direccion"].ToString() ?? "",
                Id_Tipo_Inmueble_Uso = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble_Uso)].ToString() ?? "0"),
                Id_Tipo_Inmueble = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble)].ToString() ?? "0"),
                Ambientes = int.Parse(reader[nameof(Inmueble.Ambientes)].ToString() ?? "0"),
                Coordenada_Lat = reader[nameof(Inmueble.Coordenada_Lat)].ToString() ?? "",
                Coordenada_Lon = reader[nameof(Inmueble.Coordenada_Lon)].ToString() ?? "",
                Precio = decimal.Parse(reader[nameof(Inmueble.Precio)].ToString() ?? "0"),
                Id_Propietario = int.Parse(reader[nameof(Inmueble.Id_Propietario)].ToString() ?? "0"),
                Fecha_Creacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Creacion)].ToString() ?? "0"),
                Fecha_Actualizacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Actualizacion)].ToString() ?? "0"),
                Activo = bool.Parse(reader[nameof(Inmueble.Activo)].ToString() ?? "0")
            };
        }); 

        List<int> propietariosIds = resultInmuebles.Select(x => x.Id_Propietario).ToList();

        var propietarios = _repositorioPropietario.GetPropietarios(propietariosIds);
        var tiposInmuebles = _repositorioTipos.GetTipoInmuebles();
        var tiposInmueblesUsos = _repositorioTipos.GetTipoInmueblesUsos();

        resultInmuebles.ForEach(x => {
            x.Propietario = propietarios.FirstOrDefault(y => y.Id == x.Id_Propietario)?? new Propietario();
            x.Tipo = tiposInmuebles.FirstOrDefault(y => y.Id == x.Id_Tipo_Inmueble);
            x.Tipo_Uso = tiposInmueblesUsos.FirstOrDefault(y => y.Id == x.Id_Tipo_Inmueble_Uso);
        });

        return resultInmuebles;
    }

    public List<Inmueble> GetInmuebles(int idPropietario) {
        List<Inmueble> resultInmuebles = new List<Inmueble>();

        string query = @$"select {nameof(Inmueble.Id)}, 
                                {nameof(Inmueble.Direccion)},
                                {nameof(Inmueble.Id_Tipo_Inmueble_Uso)},
                                {nameof(Inmueble.Id_Tipo_Inmueble)},
                                {nameof(Inmueble.Ambientes)},
                                {nameof(Inmueble.Coordenada_Lat)}, 
                                {nameof(Inmueble.Coordenada_Lon)}, 
                                {nameof(Inmueble.Precio)},
                                {nameof(Inmueble.Id_Propietario)},
                                {nameof(Inmueble.Fecha_Creacion)},
                                {nameof(Inmueble.Fecha_Actualizacion)},
                                {nameof(Inmueble.Activo)}		
                                from inmueble
                                where {nameof(Inmueble.Id_Propietario)} = @{nameof(Inmueble.Id_Propietario)};";

        resultInmuebles = this.ExecuteReaderList<Inmueble>(query, (parameters) => {}, (reader) =>  {
            return new Inmueble()
            {
                Id = int.Parse(reader[nameof(Inmueble.Id)].ToString() ?? "0"),
                Direccion = reader["direccion"].ToString() ?? "",
                Id_Tipo_Inmueble_Uso = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble_Uso)].ToString() ?? "0"),
                Id_Tipo_Inmueble = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble)].ToString() ?? "0"),
                Ambientes = int.Parse(reader[nameof(Inmueble.Ambientes)].ToString() ?? "0"),
                Coordenada_Lat = reader[nameof(Inmueble.Coordenada_Lat)].ToString() ?? "",
                Coordenada_Lon = reader[nameof(Inmueble.Coordenada_Lon)].ToString() ?? "",
                Precio = decimal.Parse(reader[nameof(Inmueble.Precio)].ToString() ?? "0"),
                Id_Propietario = int.Parse(reader[nameof(Inmueble.Id_Propietario)].ToString() ?? "0"),
                Fecha_Creacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Creacion)].ToString() ?? "0"),
                Fecha_Actualizacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Actualizacion)].ToString() ?? "0"),
                Activo = bool.Parse(reader[nameof(Inmueble.Activo)].ToString() ?? "0")
            };
        }); 

        List<int> propietariosIds = resultInmuebles.Select(x => x.Id_Propietario).ToList();

        var propietarios = _repositorioPropietario.GetPropietarios(propietariosIds);
        var tiposInmuebles = _repositorioTipos.GetTipoInmuebles();
        var tiposInmueblesUsos = _repositorioTipos.GetTipoInmueblesUsos();

        resultInmuebles.ForEach(x => {
            x.Propietario = propietarios.FirstOrDefault(y => y.Id == x.Id_Propietario)?? new Propietario();
            x.Tipo = tiposInmuebles.FirstOrDefault(y => y.Id == x.Id_Tipo_Inmueble);
            x.Tipo_Uso = tiposInmueblesUsos.FirstOrDefault(y => y.Id == x.Id_Tipo_Inmueble_Uso);
        });

        return resultInmuebles;
    }
    public List<Inmueble> GetInmueblesSinUso() {
        List<Inmueble> resultInmuebles = new List<Inmueble>();
    
           string query = @$"SELECT i.{nameof(Inmueble.Id)}, 
                            i.{nameof(Inmueble.Direccion)},
                            i.{nameof(Inmueble.Id_Tipo_Inmueble_Uso)},
                            i.{nameof(Inmueble.Id_Tipo_Inmueble)},
                            i.{nameof(Inmueble.Precio)}
                      FROM inmueble i
                      LEFT JOIN contrato c ON c.id_inmueble = i.id 
                          AND c.fecha_hasta > CURDATE()
                      WHERE c.id IS NULL and i.activo = 1;";

           resultInmuebles = this.ExecuteReaderList<Inmueble>(query, (parameters) => {}, (reader) => {
           return new Inmueble() {
                    Id = int.Parse(reader[nameof(Inmueble.Id)].ToString() ?? "0"),
                    Direccion = reader["direccion"].ToString() ?? "",
                    Id_Tipo_Inmueble = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble)].ToString() ?? "0"),
                    Precio = decimal.Parse(reader[nameof(Inmueble.Precio)].ToString() ?? "0")
                };
            });

            var tiposInmuebles = _repositorioTipos.GetTipoInmuebles();
            var tiposInmueblesUsos = _repositorioTipos.GetTipoInmueblesUsos();

            resultInmuebles.ForEach(x => {
                x.Tipo = tiposInmuebles.FirstOrDefault(y => y.Id == x.Id_Tipo_Inmueble);
                x.Tipo_Uso = tiposInmueblesUsos.FirstOrDefault(y => y.Id == x.Id_Tipo_Inmueble_Uso);
            });

            return resultInmuebles;
    }

    public Inmueble? GetInmueble(int Id, MySqlTransaction? transaction) {
        Inmueble? result = null;
        using var connection = transaction != null ? transaction.Connection : GetConnection();

        if(transaction == null){        
            using var transactionNew = BeginTransaction(connection);
            transaction = transactionNew;
        }

        try {
            string query = @$"select {nameof(Inmueble.Id)}, 
                                    {nameof(Inmueble.Direccion)},
                                    {nameof(Inmueble.Id_Tipo_Inmueble_Uso)},
                                    {nameof(Inmueble.Id_Tipo_Inmueble)},
                                    {nameof(Inmueble.Ambientes)},
                                    {nameof(Inmueble.Coordenada_Lat)}, 
                                    {nameof(Inmueble.Coordenada_Lon)}, 
                                    {nameof(Inmueble.Precio)},
                                    {nameof(Inmueble.Id_Propietario)},
                                    {nameof(Inmueble.Fecha_Creacion)},
                                    {nameof(Inmueble.Fecha_Actualizacion)},
                                    {nameof(Inmueble.Activo)}		
                                    from inmueble where {nameof(Inmueble.Id)} = @{nameof(Inmueble.Id)};";

            result = this.ExecuteReader<Inmueble>(query, (parameters) => {
                parameters.AddWithValue($"@{nameof(Inmueble.Id)}", Id);
            },  (reader) =>  {
                return new Inmueble()
                {
                    Id = int.Parse(reader[nameof(Inmueble.Id)].ToString() ?? "0"),
                    Direccion = reader["direccion"].ToString() ?? "",
                    Id_Tipo_Inmueble_Uso = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble_Uso)].ToString() ?? "0"),
                    Id_Tipo_Inmueble = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble)].ToString() ?? "0"),
                    Ambientes = int.Parse(reader[nameof(Inmueble.Ambientes)].ToString() ?? "0"),
                    Coordenada_Lat = reader[nameof(Inmueble.Coordenada_Lat)].ToString() ?? "0",
                    Coordenada_Lon = reader[nameof(Inmueble.Coordenada_Lon)].ToString() ?? "0",
                    Precio = decimal.Parse(reader[nameof(Inmueble.Precio)].ToString() ?? "0"),
                    Id_Propietario = int.Parse(reader[nameof(Inmueble.Id_Propietario)].ToString() ?? "0"),
                    Fecha_Creacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Creacion)].ToString() ?? "0"),
                    Fecha_Actualizacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Actualizacion)].ToString() ?? "0"),
                    Activo = bool.Parse(reader[nameof(Inmueble.Activo)].ToString() ?? "0")
                };
            }); 

            List<int> propietariosIds = new List<int>();
            if (result != null) propietariosIds.Add(result.Id_Propietario);

            var propietarios = _repositorioPropietario.GetPropietarios(propietariosIds);
            var tiposInmuebles = _repositorioTipos.GetTipoInmuebles();
            var tiposInmueblesUsos = _repositorioTipos.GetTipoInmueblesUsos();
            if(result != null){
                result.Propietario = propietarios.FirstOrDefault(y => y.Id == result.Id_Propietario) ?? new Propietario();
                result.Tipo = tiposInmuebles.FirstOrDefault(y => y.Id == result.Id_Tipo_Inmueble);
                result.Tipo_Uso = tiposInmueblesUsos.FirstOrDefault(y => y.Id == result.Id_Tipo_Inmueble_Uso);

            }
            return result;
        } catch (Exception ex) {
            _logger.LogError("Error: {Error}", ex.Message);
            if (connection.State != ConnectionState.Open) {
                _logger.LogError("La conexión se ha cerrado inesperadamente.");
            } else {
                transaction.Rollback();
            }
            throw;      
        }
    }

    public bool BajaInmueble(int id) {
        bool result = false;
        string query = @$"delete from inmueble where {nameof(Inmueble.Id)} = @{nameof(Inmueble.Id)};";
        result = this.ExecuteNonQuery(query, (parameters) => {
                    parameters.AddWithValue($"@{nameof(Inmueble.Id)}", id);
                }) > 0;
        return result;
    }

    public bool ActualizarInmueble(Inmueble inmueble){
        bool result = false;
        string query = @$"UPDATE inmueble SET 
                                {nameof(Inmueble.Direccion)} = @{nameof(Inmueble.Direccion)}, 
                                {nameof(Inmueble.Id_Propietario)} = @{nameof(Inmueble.Id_Propietario)}, 
                                {nameof(Inmueble.Id_Tipo_Inmueble)} = @{nameof(Inmueble.Id_Tipo_Inmueble)}, 
                                {nameof(Inmueble.Id_Tipo_Inmueble_Uso)} = @{nameof(Inmueble.Id_Tipo_Inmueble_Uso)}, 
                                {nameof(Inmueble.Ambientes)} = @{nameof(Inmueble.Ambientes)},
                                {nameof(Inmueble.Coordenada_Lat)} = @{nameof(Inmueble.Coordenada_Lat)},
                                {nameof(Inmueble.Coordenada_Lon)} = @{nameof(Inmueble.Coordenada_Lon)},
                                {nameof(Inmueble.Precio)} = @{nameof(Inmueble.Precio)},
                                {nameof(Inmueble.Activo)} = @{nameof(Inmueble.Activo)}
                        where {nameof(Inmueble.Id)} = @{nameof(Inmueble.Id)};";

        result = 0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Inmueble.Id)}", inmueble.Id);
            parameters.AddWithValue($"@{nameof(Inmueble.Direccion)}", inmueble.Direccion);
            parameters.AddWithValue($"@{nameof(Inmueble.Id_Propietario)}", inmueble.Id_Propietario);
            parameters.AddWithValue($"@{nameof(Inmueble.Id_Tipo_Inmueble)}", inmueble.Id_Tipo_Inmueble);
            parameters.AddWithValue($"@{nameof(Inmueble.Id_Tipo_Inmueble_Uso)}", inmueble.Id_Tipo_Inmueble_Uso);
            parameters.AddWithValue($"@{nameof(Inmueble.Ambientes)}", inmueble.Ambientes);
            parameters.AddWithValue($"@{nameof(Inmueble.Coordenada_Lat)}", inmueble.Coordenada_Lat);
            parameters.AddWithValue($"@{nameof(Inmueble.Coordenada_Lon)}", inmueble.Coordenada_Lon);
            parameters.AddWithValue($"@{nameof(Inmueble.Precio)}", inmueble.Precio);
            parameters.AddWithValue($"@{nameof(Inmueble.Activo)}", inmueble.Activo);
        });
        return result;
    }

    public List<TipoInmueble> GetTipoInmuebles() {
        var resultList = _repositorioTipos.GetTipoInmuebles();
        return resultList;
    }

    public List<TipoInmuebleUso> GetTipoInmueblesUsos() {
        var resultList = _repositorioTipos.GetTipoInmueblesUsos();
        return resultList;
    }

    public List<Inmueble> GetInmueblesDisponiblesPorFecha(DateTime fechaDesde, DateTime fechaHasta, int Id_Contrato) {
        List<Inmueble> inmuebles = new List<Inmueble>();

        string query = @$"SELECT i.{nameof(Inmueble.Id)}, 
                                i.{nameof(Inmueble.Direccion)},
                                i.{nameof(Inmueble.Id_Tipo_Inmueble_Uso)},
                                i.{nameof(Inmueble.Id_Tipo_Inmueble)},
                                i.{nameof(Inmueble.Ambientes)},
                                i.{nameof(Inmueble.Coordenada_Lat)},
                                i.{nameof(Inmueble.Coordenada_Lon)},
                                i.{nameof(Inmueble.Precio)},
                                i.{nameof(Inmueble.Id_Propietario)},
                                i.{nameof(Inmueble.Fecha_Creacion)},
                                i.{nameof(Inmueble.Fecha_Actualizacion)},
                                i.{nameof(Inmueble.Activo)}
                        FROM inmueble i
                        LEFT JOIN contrato c 
                            ON i.id = c.id_inmueble
                            {(Id_Contrato > 0 ? $"AND c.id != {Id_Contrato}" : "")}
                            AND c.estado = 'Vigente'
                            AND 
                                (c.fecha_desde <= @FechaFin AND c.fecha_hasta >= @FechaInicio) -- Contrato que cubre el rango solicitado
                        WHERE i.activo = 1
                        AND c.id IS NULL;";

        inmuebles = this.ExecuteReaderList<Inmueble>(query,(param) => {
            param.AddWithValue($"@FechaInicio", fechaDesde);
            param.AddWithValue($"@FechaFin", fechaHasta);
        },
        (reader) => {
            return new Inmueble(){
                Id = int.Parse(reader[nameof(Inmueble.Id)].ToString() ?? "0"),
                Direccion = reader["direccion"].ToString() ?? "",
                Id_Tipo_Inmueble_Uso = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble_Uso)].ToString() ?? "0"),
                Id_Tipo_Inmueble = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble)].ToString() ?? "0"),
                Ambientes = int.Parse(reader[nameof(Inmueble.Ambientes)].ToString() ?? "0"),
                Coordenada_Lat = reader[nameof(Inmueble.Coordenada_Lat)].ToString() ?? "",
                Coordenada_Lon = reader[nameof(Inmueble.Coordenada_Lon)].ToString() ?? "",
                Precio = decimal.Parse(reader[nameof(Inmueble.Precio)].ToString() ?? "0"),
                Id_Propietario = int.Parse(reader[nameof(Inmueble.Id_Propietario)].ToString() ?? "0"),
                Fecha_Creacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Creacion)].ToString() ?? "0"),
                Fecha_Actualizacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Actualizacion)].ToString() ?? "0"),
                Activo = bool.Parse(reader[nameof(Inmueble.Activo)].ToString() ?? "0")
            };
        });

        return inmuebles;
    }
    public bool EsInmuebleDisponible(int IdInmueble, DateTime fechaDesde, DateTime fechaHasta) {
        try {
            // string query = @$"SELECT COUNT(1)
            //             FROM inmueble AS i
            //             LEFT JOIN contrato AS c ON i.id = c.id_inmueble
            //             WHERE i.id = @IdInmueble
            //             AND i.activo = 1
            //             AND c.estado != 'Finalizado'
            //             AND (
            //                 (c.id IS NULL) 
            //                 OR 
            //                 (c.fecha_desde > @FechaHasta OR c.fecha_hasta < @FechaDesde)
            //             );";

            string query = @$"SELECT COUNT(*) AS contratos_invalidos
                    FROM contrato c
                    JOIN inmueble i ON i.id = c.id_inmueble
                    WHERE i.id = @IdInmueble
                    AND i.activo = 1
                    AND (
                        c.estado != 'Finalizado' 
                        AND (
                        (@FechaDesde BETWEEN c.fecha_desde AND c.fecha_hasta)
                        OR (@FechaHasta BETWEEN c.fecha_desde AND c.fecha_hasta)
                        OR (c.fecha_desde BETWEEN @FechaDesde AND @FechaHasta)
                        )
                    );";

            int count = this.ExecuteScalar(query, (parameters) => {
                parameters.AddWithValue("@IdInmueble", IdInmueble);
                parameters.AddWithValue("@FechaDesde", fechaDesde.Date);
                parameters.AddWithValue("@FechaHasta", fechaHasta.Date);

            });

            return count > 0; //DISPONIBLE
        } catch (Exception ex) {
            throw new Exception("Error al validar el inmueble en las fechas: " + ex.Message, ex);
        }

    }
}
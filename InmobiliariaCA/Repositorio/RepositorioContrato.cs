namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Text;
using InmobiliariaCA.Models.ContratoModels;
using System.Data;
using MySql.Data.MySqlClient;
using System.Transactions;

public class RepositorioContrato : RepositorioBase, IRepositorioContrato {

    private IRepositorioInmueble _repositorioInmueble;
    
    private readonly ILogger<RepositorioContrato> _logger;
    private IRepositorioInquilino _repositorioInquilino;

    public RepositorioContrato(IConfiguration configuration, 
        ILogger<RepositorioContrato> logger,
        IRepositorioInmueble repositorioInmueble, 
        IRepositorioInquilino repositorioInquilino) : base(configuration)
    {
         _logger = logger;
        _repositorioInmueble = repositorioInmueble;
        _repositorioInquilino = repositorioInquilino;
    }

    public List<Contrato> GetContratos() {
        List<Contrato> resultContratos = new List<Contrato>();

        string query = @$"select {nameof(Contrato.Id)},
                                    {nameof(Contrato.Id_Inmueble)},
                                    {nameof(Contrato.Id_Inquilino)},
                                    {nameof(Contrato.Fecha_Desde)},
                                    {nameof(Contrato.Fecha_Hasta)},
                                    {nameof(Contrato.Monto_Alquiler)},
                                    {nameof(Contrato.Fecha_Finalizacion_Anticipada)},
                                    {nameof(Contrato.Multa)},
                                    {nameof(Contrato.Fecha_Creacion)},
                                    {nameof(Contrato.Fecha_Actualizacion)},                    
                                    {nameof(Contrato.Pagado)},
                                    {nameof(Contrato.Cantidad_Cuotas)},
                                    {nameof(Contrato.Cuotas_Pagas)},
                                    {nameof(Contrato.Estado)}
                            from contrato;";

        resultContratos = this.ExecuteReaderList<Contrato>(query, (parameters) => { }, (reader) => {
            var contrato = new Contrato() {
                Id = int.Parse(reader[nameof(Contrato.Id)].ToString() ?? "0"),
                Id_Inmueble = int.Parse(reader[nameof(Contrato.Id_Inmueble)].ToString() ?? "0"),
                Id_Inquilino = int.Parse(reader[nameof(Contrato.Id_Inquilino)].ToString() ?? "0"),
                Fecha_Desde = DateTime.Parse(reader[nameof(Contrato.Fecha_Desde)].ToString() ?? "0"),
                Fecha_Hasta = DateTime.Parse(reader[nameof(Contrato.Fecha_Hasta)].ToString() ?? "0"),
                Monto_Alquiler = decimal.Parse(reader[nameof(Contrato.Monto_Alquiler)].ToString() ?? "0"),
                Fecha_Finalizacion_Anticipada = reader[nameof(Contrato.Fecha_Finalizacion_Anticipada)] != DBNull.Value ? DateTime.Parse(reader["fecha_finalizacion_anticipada"].ToString() ?? "0") : (DateTime?)null,
                Multa = reader[nameof(Contrato.Multa)] != DBNull.Value ? decimal.Parse(reader[nameof(Contrato.Multa)].ToString() ?? "0") : (decimal?)null,
                Fecha_Creacion = DateTime.Parse(reader[nameof(Contrato.Fecha_Creacion)].ToString() ?? "0"),
                Fecha_Actualizacion = DateTime.Parse(reader[nameof(Contrato.Fecha_Actualizacion)].ToString() ?? "0"),
                Pagado = reader[nameof(Contrato.Pagado)].ToString() == "1",
                Cantidad_Cuotas = int.Parse(reader[nameof(Contrato.Cantidad_Cuotas)].ToString() ?? "0"),
                Cuotas_Pagas = int.Parse(reader[nameof(Contrato.Cuotas_Pagas)].ToString() ?? "0"),
                Estado = Enum.TryParse(reader[nameof(Contrato.Estado)].ToString(), out EstadoContrato estado) ? estado : EstadoContrato.Vigente
            };

            // Cargar los objetos Inmueble e Inquilino usando sus IDs
            contrato.Inmueble = _repositorioInmueble.GetInmueble(contrato.Id_Inmueble,null) ?? throw new InvalidOperationException("Inmueble no se encuentra");
            contrato.Inquilino = _repositorioInquilino.GetInquilino(contrato.Id_Inquilino,null) ?? throw new InvalidOperationException("Inquilino no se encuentra");
            contrato.Estado = contrato.PagosCompletos() ? EstadoContrato.Finalizado : contrato.Estado;

            return contrato;
        });

        return resultContratos;
    }

    public List<Contrato> GetContratos(int Id_Inmueble) {
        List<Contrato> resultContratos = new List<Contrato>();

        string query = @$"select {nameof(Contrato.Id)},
                                    {nameof(Contrato.Id_Inmueble)},
                                    {nameof(Contrato.Id_Inquilino)},
                                    {nameof(Contrato.Fecha_Desde)},
                                    {nameof(Contrato.Fecha_Hasta)},
                                    {nameof(Contrato.Monto_Alquiler)},
                                    {nameof(Contrato.Fecha_Finalizacion_Anticipada)},
                                    {nameof(Contrato.Multa)},
                                    {nameof(Contrato.Fecha_Creacion)},
                                    {nameof(Contrato.Fecha_Actualizacion)},                    
                                    {nameof(Contrato.Pagado)},
                                    {nameof(Contrato.Cantidad_Cuotas)},
                                    {nameof(Contrato.Cuotas_Pagas)},
                                    {nameof(Contrato.Estado)}
                            from contrato
                                where {nameof(Contrato.Id_Inmueble)} = {Id_Inmueble};";

        resultContratos = this.ExecuteReaderList<Contrato>(query, (parameters) => { }, (reader) => {
            var contrato = new Contrato() {
                Id = int.Parse(reader[nameof(Contrato.Id)].ToString() ?? "0"),
                Id_Inmueble = int.Parse(reader[nameof(Contrato.Id_Inmueble)].ToString() ?? "0"),
                Id_Inquilino = int.Parse(reader[nameof(Contrato.Id_Inquilino)].ToString() ?? "0"),
                Fecha_Desde = DateTime.Parse(reader[nameof(Contrato.Fecha_Desde)].ToString() ?? "0"),
                Fecha_Hasta = DateTime.Parse(reader[nameof(Contrato.Fecha_Hasta)].ToString() ?? "0"),
                Monto_Alquiler = decimal.Parse(reader[nameof(Contrato.Monto_Alquiler)].ToString() ?? "0"),
                Fecha_Finalizacion_Anticipada = reader[nameof(Contrato.Fecha_Finalizacion_Anticipada)] != DBNull.Value ? DateTime.Parse(reader["fecha_finalizacion_anticipada"].ToString() ?? "0") : (DateTime?)null,
                Multa = reader[nameof(Contrato.Multa)] != DBNull.Value ? decimal.Parse(reader[nameof(Contrato.Multa)].ToString() ?? "0") : (decimal?)null,
                Fecha_Creacion = DateTime.Parse(reader[nameof(Contrato.Fecha_Creacion)].ToString() ?? "0"),
                Fecha_Actualizacion = DateTime.Parse(reader[nameof(Contrato.Fecha_Actualizacion)].ToString() ?? "0"),
                Pagado = reader[nameof(Contrato.Pagado)].ToString() == "1",
                Cantidad_Cuotas = int.Parse(reader[nameof(Contrato.Cantidad_Cuotas)].ToString() ?? "0"),
                Cuotas_Pagas = int.Parse(reader[nameof(Contrato.Cuotas_Pagas)].ToString() ?? "0"),
                Estado = Enum.TryParse(reader[nameof(Contrato.Estado)].ToString(), out EstadoContrato estado) ? estado : EstadoContrato.Vigente
            };

            // Cargar los objetos Inmueble e Inquilino usando sus IDs
            contrato.Inmueble = _repositorioInmueble.GetInmueble(Id_Inmueble, null) ?? throw new InvalidOperationException("Inmueble no se encuentra");
            contrato.Inquilino = _repositorioInquilino.GetInquilino(contrato.Id_Inquilino, null) ?? throw new InvalidOperationException("Inquilino no se encuentra");
            contrato.Estado = contrato.PagosCompletos() ? EstadoContrato.Finalizado : contrato.Estado;

            return contrato;
        });

        return resultContratos;
    }

     public Contrato? GetContrato(int id, MySqlTransaction? transaction) {
        Contrato? result = null;
        using var connection = transaction != null ? transaction.Connection : GetConnection();

        if(transaction == null){        
            using var transactionNew = BeginTransaction(connection);
            transaction = transactionNew;
        }

        try {
            string query = @$"select  {nameof(Contrato.Id)},
                                        {nameof(Contrato.Id_Inmueble)},
                                        {nameof(Contrato.Id_Inquilino)},
                                        {nameof(Contrato.Fecha_Desde)},
                                        {nameof(Contrato.Fecha_Hasta)},
                                        {nameof(Contrato.Monto_Alquiler)},
                                        {nameof(Contrato.Fecha_Finalizacion_Anticipada)},
                                        {nameof(Contrato.Multa)},
                                        {nameof(Contrato.Fecha_Creacion)},
                                        {nameof(Contrato.Fecha_Actualizacion)},                    
                                        {nameof(Contrato.Pagado)},
                                        {nameof(Contrato.Cantidad_Cuotas)},
                                        {nameof(Contrato.Cuotas_Pagas)},
                                        {nameof(Contrato.Estado)}
                                from contrato
                                    where {nameof(Contrato.Id)} = {id};";

            result = this.ExecuteReader<Contrato>(query,(reader) => {
                Contrato contrato = new Contrato() {
                    Id = int.Parse(reader[nameof(Contrato.Id)].ToString() ?? "0"),
                    Id_Inmueble = int.Parse(reader[nameof(Contrato.Id_Inmueble)].ToString() ?? "0"),
                    Id_Inquilino = int.Parse(reader[nameof(Contrato.Id_Inquilino)].ToString() ?? "0"),
                    Fecha_Desde = DateTime.Parse(reader[nameof(Contrato.Fecha_Desde)].ToString() ?? "0"),
                    Fecha_Hasta = DateTime.Parse(reader[nameof(Contrato.Fecha_Hasta)].ToString() ?? "0"),
                    Monto_Alquiler = decimal.Parse(reader[nameof(Contrato.Monto_Alquiler)].ToString() ?? "0"),
                    Fecha_Finalizacion_Anticipada = reader[nameof(Contrato.Fecha_Finalizacion_Anticipada)] != DBNull.Value ? DateTime.Parse(reader["fecha_finalizacion_anticipada"].ToString() ?? "0") : (DateTime?)null,
                    Multa = reader[nameof(Contrato.Multa)] != DBNull.Value ? decimal.Parse(reader[nameof(Contrato.Multa)].ToString() ?? "0") : (decimal?)null,
                    Fecha_Creacion = DateTime.Parse(reader[nameof(Contrato.Fecha_Creacion)].ToString() ?? "0"),
                    Fecha_Actualizacion = DateTime.Parse(reader[nameof(Contrato.Fecha_Actualizacion)].ToString() ?? "0"),
                    Pagado = reader[nameof(Contrato.Pagado)].ToString() == "1",
                    Cantidad_Cuotas = int.Parse(reader[nameof(Contrato.Cantidad_Cuotas)].ToString() ?? "0"),
                    Cuotas_Pagas = int.Parse(reader[nameof(Contrato.Cuotas_Pagas)].ToString() ?? "0"),
                    Estado = Enum.TryParse(reader[nameof(Contrato.Estado)].ToString(), out EstadoContrato estado) ? estado : EstadoContrato.Vigente
                };
                // Cargar los objetos Inmueble e Inquilino usando sus IDs
                contrato.Inmueble  = _repositorioInmueble.GetInmueble(contrato.Id_Inmueble, transaction) ?? throw new InvalidOperationException("Inmueble no se encuentra");
                contrato.Inquilino = _repositorioInquilino.GetInquilino(contrato.Id_Inquilino, transaction) ?? throw new InvalidOperationException("Inquilino no se encuentra");
                contrato.Estado = contrato.PagosCompletos() ? EstadoContrato.Finalizado : contrato.Estado;

                return contrato;
            });

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

    public int InsertarContrato(Contrato contrato) {
        string query = @$"INSERT INTO contrato (
                                {nameof(Contrato.Id_Inmueble)},
                                {nameof(Contrato.Id_Inquilino)},
                                {nameof(Contrato.Fecha_Desde)},
                                {nameof(Contrato.Fecha_Hasta)},
                                {nameof(Contrato.Monto_Alquiler)},
                                {nameof(Contrato.Fecha_Finalizacion_Anticipada)},
                                {nameof(Contrato.Multa)},
                                {nameof(Contrato.Id_Usuario_Creacion)},
                                {nameof(Contrato.Id_Usuario_Finalizacion)},
                                {nameof(Contrato.Fecha_Creacion)},
                                {nameof(Contrato.Fecha_Actualizacion)},
                                {nameof(Contrato.Cantidad_Cuotas)})
                            VALUES(
                                @{nameof(Contrato.Id_Inmueble)},
                                @{nameof(Contrato.Id_Inquilino)},
                                @{nameof(Contrato.Fecha_Desde)},
                                @{nameof(Contrato.Fecha_Hasta)},
                                @{nameof(Contrato.Monto_Alquiler)},
                                @{nameof(Contrato.Fecha_Finalizacion_Anticipada)},
                                @{nameof(Contrato.Multa)},
                                @{nameof(Contrato.Id_Usuario_Creacion)},
                                @{nameof(Contrato.Id_Usuario_Finalizacion)},
                                @{nameof(Contrato.Fecha_Creacion)},
                                @{nameof(Contrato.Fecha_Actualizacion)},
                                @{nameof(Contrato.Cantidad_Cuotas)});
                            SELECT LAST_INSERT_ID();";

            int result = this.ExecuteNonQuery(query, (parameters) => {
                parameters.AddWithValue($"@{nameof(Contrato.Id_Inmueble)}", contrato.Id_Inmueble);
                parameters.AddWithValue($"@{nameof(Contrato.Id_Inquilino)}", contrato.Id_Inquilino);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Desde)}", contrato.Fecha_Desde);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Hasta)}", contrato.Fecha_Hasta);
                parameters.AddWithValue($"@{nameof(Contrato.Monto_Alquiler)}", contrato.Monto_Alquiler);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Finalizacion_Anticipada)}", (object?)contrato.Fecha_Finalizacion_Anticipada ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Multa)}", (object?)contrato.Multa ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Id_Usuario_Creacion)}", contrato.Id_Usuario_Creacion);
                parameters.AddWithValue($"@{nameof(Contrato.Id_Usuario_Finalizacion)}", (object?)contrato.Id_Usuario_Finalizacion ?? DBNull.Value);// (object?)contrato.Id_Usuario_Finalizacion ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Creacion)}", contrato.Fecha_Creacion);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Actualizacion)}", contrato.Fecha_Actualizacion);
                parameters.AddWithValue($"@{nameof(Contrato.Cantidad_Cuotas)}", contrato.CantidadCuotas());
            });

        return result;
    }

    public int ActualizarContrato(Contrato contrato) {
        try{
            string query = @$"UPDATE contrato SET
                                    {nameof(Contrato.Id_Inmueble)} = @{nameof(Contrato.Id_Inmueble)},
                                    {nameof(Contrato.Id_Inquilino)} = @{nameof(Contrato.Id_Inquilino)},
                                    {nameof(Contrato.Fecha_Desde)} = @{nameof(Contrato.Fecha_Desde)},
                                    {nameof(Contrato.Fecha_Hasta)} = @{nameof(Contrato.Fecha_Hasta)},
                                    {nameof(Contrato.Monto_Alquiler)} = @{nameof(Contrato.Monto_Alquiler)},
                                    {nameof(Contrato.Fecha_Finalizacion_Anticipada)} = @{nameof(Contrato.Fecha_Finalizacion_Anticipada)},
                                    {nameof(Contrato.Multa)} = @{nameof(Contrato.Multa)},
                                    {nameof(Contrato.Id_Usuario_Finalizacion)} = @{nameof(Contrato.Id_Usuario_Finalizacion)},
                                    {nameof(Contrato.Fecha_Actualizacion)} = @{nameof(Contrato.Fecha_Actualizacion)},
                                    {nameof(Contrato.Estado)} = @{nameof(Contrato.Estado)}
                                WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)};";

            int result = this.ExecuteNonQuery(query, (parameters) => {
                parameters.AddWithValue($"@{nameof(Contrato.Id)}", contrato.Id);
                parameters.AddWithValue($"@{nameof(Contrato.Id_Inmueble)}", contrato.Id_Inmueble);
                parameters.AddWithValue($"@{nameof(Contrato.Id_Inquilino)}", contrato.Id_Inquilino);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Desde)}", contrato.Fecha_Desde);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Hasta)}", contrato.Fecha_Hasta);
                parameters.AddWithValue($"@{nameof(Contrato.Monto_Alquiler)}", contrato.Monto_Alquiler);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Finalizacion_Anticipada)}", (object?)contrato.Fecha_Finalizacion_Anticipada ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Multa)}", (object?)contrato.Multa ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Id_Usuario_Finalizacion)}", (object?)contrato.Id_Usuario_Finalizacion ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Actualizacion)}", contrato.Fecha_Actualizacion);
                parameters.AddWithValue($"@{nameof(Contrato.Estado)}", contrato.Estado.ToString());
            });

            return result;
        } catch (Exception ex) {
             _logger.LogError("Hubo un error al updatear contrato: {Error}", ex.Message);
            throw new Exception("Error al actualizar el contrato. Contacte con el administrador.");
        }
    }

    public int ActualizarContratoPagado(int Id, MySqlTransaction? transaction) {
        using var connection = transaction != null ? transaction.Connection : GetConnection();

        if(transaction == null){        
            using var transactionNew = BeginTransaction(connection);
            transaction = transactionNew;
        }        
        
        try{
            Contrato? contrato = this.GetContrato(Id, null) ?? throw new Exception("No se encontró el contrato.");

            if (contrato.Cantidad_Cuotas-contrato.Cuotas_Pagas == 0) {
                contrato.Estado = EstadoContrato.Finalizado;
            }

            string query = @$"UPDATE contrato SET
                                    {nameof(Contrato.Pagado)} = @{nameof(Contrato.Pagado)},
                                    {nameof(Contrato.Estado)} = @{nameof(Contrato.Estado)},
                                    {nameof(Contrato.Cuotas_Pagas)} = {nameof(Contrato.Cuotas_Pagas)} + 1                        
                            WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)}
                                AND {nameof(Contrato.Cuotas_Pagas)} < {nameof(Contrato.Cantidad_Cuotas)};";

            int result = this.ExecuteNonQuery(query, (parameters) => {
                parameters.AddWithValue($"@{nameof(Contrato.Pagado)}", contrato.EsFinalizado());
                parameters.AddWithValue($"@{nameof(Contrato.Estado)}", contrato.Estado.ToString());
                parameters.AddWithValue($"@{nameof(Contrato.Id)}", Id);
            }, transaction);

            // if(transaction.Connection.State == ConnectionState.Open){
            //     transaction.Commit();   
            // }
            return result;
        } catch (Exception ex) {
            _logger.LogError("Hubo un error al updatear contrato: {Error}", ex.Message);
            if (connection.State != ConnectionState.Open) {
                _logger.LogError("La conexión se ha cerrado inesperadamente.");
            } else {
                transaction.Rollback();
            }
            throw new Exception("Error al actualizar el contrato. Contacte con el administrador."); 
        }
    }

    public int ActualizarContratoPagoAnulado(int Id, MySqlTransaction? transaction) {
        using var connection = transaction != null ? transaction.Connection : GetConnection();

        if(transaction == null){        
            using var transactionNew = BeginTransaction(connection);
            transaction = transactionNew;
        }        
        
        try{
            Contrato? contrato = this.GetContrato(Id, null) ?? throw new Exception("No se encontró el contrato.");
            if(contrato.PagosCompletos() && EstadoContrato.Finalizado.Equals(contrato.Estado)){
                contrato.Estado = EstadoContrato.Vigente;
            }

            string query = @$"UPDATE contrato SET
                                    {nameof(Contrato.Estado)} = @{nameof(Contrato.Estado)},
                                    {nameof(Contrato.Cuotas_Pagas)} = {nameof(Contrato.Cuotas_Pagas)} - 1                        
                            WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)}
                                AND {nameof(Contrato.Cuotas_Pagas)} <= {nameof(Contrato.Cantidad_Cuotas)};";

            int result = this.ExecuteNonQuery(query, (parameters) => {
                parameters.AddWithValue($"@{nameof(Contrato.Estado)}", contrato.Estado.ToString());
                parameters.AddWithValue($"@{nameof(Contrato.Id)}", Id);
            }, transaction);

            // if(transaction.Connection.State == ConnectionState.Open){
            //     transaction.Commit();   
            // }
            return result;
        } catch (Exception ex) {
            _logger.LogError("Hubo un error al updatear contrato: {Error}", ex.Message);
            if (connection.State != ConnectionState.Open) {
                _logger.LogError("La conexión se ha cerrado inesperadamente.");
            } else {
                transaction.Rollback();
            }
            throw new Exception("Error al actualizar el contrato. Contacte con el administrador."); 
        }
    }

    public bool BajaContrato(int id) {
        string query = @$"delete from contrato 
                            where {nameof(Contrato.Id)} = @{nameof(Contrato.Id)};";

        bool result = 0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Contrato.Id)}", id);
        });

        return result;
    }

    
    public List<Contrato> GetContratosFiltrados(ContratoFilter filter, MySqlTransaction? transaction) {

        using var connection = transaction != null ? transaction.Connection : GetConnection();

        if(transaction == null){        
            using var transactionNew = BeginTransaction(connection);
            transaction = transactionNew;
        } 
        
        try {
            var query = new StringBuilder(@"select * from contrato where 1=1");

            if (filter.ContratoId.HasValue)
                query.Append($" AND Id = {filter.ContratoId}");

            if (filter.InquilinoId.HasValue)
                query.Append($" AND Id_Inquilino = {filter.InquilinoId}");

            if (filter.InmuebleId.HasValue)
                query.Append($" AND Id_Inmueble = {filter.InmuebleId}");

            if (filter.Estado.HasValue)
                query.Append($" AND Estado = '{filter.Estado.ToString()}'");

            if (filter.FechaDesde_Inicio.HasValue)
                query.Append($" AND Fecha_Desde >= '{filter.FechaDesde_Inicio.Value.ToString("yyyy-MM-dd")}'");

            if (filter.FechaDesde_Fin.HasValue)
                query.Append($" AND Fecha_Desde <= '{filter.FechaDesde_Fin.Value.ToString("yyyy-MM-dd")}'");

            if (filter.FechaHasta_Inicio.HasValue)
                query.Append($" AND Fecha_Hasta >= '{filter.FechaHasta_Inicio.Value.ToString("yyyy-MM-dd")}'");

            if (filter.FechaHasta_Fin.HasValue)
                query.Append($" AND Fecha_Hasta <= '{filter.FechaHasta_Fin.Value.ToString("yyyy-MM-dd")}'");


            List<Contrato> resultContratos = this.ExecuteReaderList<Contrato>(query.ToString(), (parameters) => { }, (reader) => {
                var contrato = new Contrato() {
                    Id = int.Parse(reader[nameof(Contrato.Id)].ToString() ?? "0"),
                    Id_Inmueble = int.Parse(reader[nameof(Contrato.Id_Inmueble)].ToString() ?? "0"),
                    Id_Inquilino = int.Parse(reader[nameof(Contrato.Id_Inquilino)].ToString() ?? "0"),
                    Fecha_Desde = DateTime.Parse(reader[nameof(Contrato.Fecha_Desde)].ToString() ?? "0"),
                    Fecha_Hasta = DateTime.Parse(reader[nameof(Contrato.Fecha_Hasta)].ToString() ?? "0"),
                    Monto_Alquiler = decimal.Parse(reader[nameof(Contrato.Monto_Alquiler)].ToString() ?? "0"),
                    Fecha_Finalizacion_Anticipada = reader[nameof(Contrato.Fecha_Finalizacion_Anticipada)] != DBNull.Value ? DateTime.Parse(reader["fecha_finalizacion_anticipada"].ToString() ?? "0") : (DateTime?)null,
                    Multa = reader[nameof(Contrato.Multa)] != DBNull.Value ? decimal.Parse(reader[nameof(Contrato.Multa)].ToString() ?? "0") : (decimal?)null,
                    Fecha_Creacion = DateTime.Parse(reader[nameof(Contrato.Fecha_Creacion)].ToString() ?? "0"),
                    Fecha_Actualizacion = DateTime.Parse(reader[nameof(Contrato.Fecha_Actualizacion)].ToString() ?? "0"),
                    Pagado = reader[nameof(Contrato.Pagado)].ToString() == "1",
                    Cantidad_Cuotas = int.Parse(reader[nameof(Contrato.Cantidad_Cuotas)].ToString() ?? "0"),
                    Cuotas_Pagas = int.Parse(reader[nameof(Contrato.Cuotas_Pagas)].ToString() ?? "0"),
                    Estado = Enum.TryParse(reader[nameof(Contrato.Estado)].ToString(), out EstadoContrato estado) ? estado : EstadoContrato.Vigente
                };

                // Cargar los objetos Inmueble e Inquilino usando sus IDs
                contrato.Inmueble = _repositorioInmueble.GetInmueble(contrato.Id_Inmueble, transaction) ?? throw new InvalidOperationException("Inmueble no se encuentra");
                contrato.Inquilino = _repositorioInquilino.GetInquilino(contrato.Id_Inquilino, transaction) ?? throw new InvalidOperationException("Inquilino no se encuentra");
                contrato.Estado = contrato.PagosCompletos() ? EstadoContrato.Finalizado : contrato.Estado;

                return contrato;
            });

            return resultContratos;
        } catch (Exception ex) {
            _logger.LogError("Hubo un error al updatear contrato: {Error}", ex.Message);
            if (connection.State != ConnectionState.Open) {
                _logger.LogError("La conexión se ha cerrado inesperadamente.");
            } else {
                transaction.Rollback();
            }
            throw new Exception("Error al actualizar el contrato. Contacte con el administrador.");
        }
    }    
}

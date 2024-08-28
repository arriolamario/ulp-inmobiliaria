namespace InmobiliariaCA.Repositorio
{
    using InmobiliariaCA.Models;
    using MySql.Data.MySqlClient;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System;

    public class RepositorioContrato : RepositorioBase {
        public RepositorioContrato(IConfiguration configuration) : base(configuration) {
        }

        public List<Contrato> GetContratos() {
            List<Contrato> resultContratos = new List<Contrato>();

            string query = @$"select {nameof(Contrato.Id)},
                                    {nameof(Contrato.IdInmueble)},
                                    {nameof(Contrato.IdInquilino)},
                                    {nameof(Contrato.FechaDesde)},
                                    {nameof(Contrato.FechaHasta)},
                                    {nameof(Contrato.MontoAlquiler)},
                                    {nameof(Contrato.FechaFinalizacionAnticipada)},
                                    {nameof(Contrato.Multa)},
                                    {nameof(Contrato.Estado)},
                                    {nameof(Contrato.FechaCreacion)},
                                    {nameof(Contrato.FechaActualizacion)}
                              from contrato
                              where estado = 1;";

            resultContratos = this.ExecuteReaderList<Contrato>(query, (reader) => {
                return new Contrato() {
                    Id = int.Parse(reader["id"].ToString() ?? "0"),
                    IdInmueble = int.Parse(reader["id_inmueble"].ToString() ?? "0"),
                    IdInquilino = int.Parse(reader["id_inquilino"].ToString() ?? "0"),
                    FechaDesde = DateTime.Parse(reader["fecha_desde"].ToString() ?? "0"),
                    FechaHasta = DateTime.Parse(reader["fecha_hasta"].ToString() ?? "0"),
                    MontoAlquiler = decimal.Parse(reader["monto_alquiler"].ToString() ?? "0"),
                    FechaFinalizacionAnticipada = reader["fecha_finalizacion_anticipada"] != DBNull.Value ? DateTime.Parse(reader["fecha_finalizacion_anticipada"].ToString() ?? "0") : (DateTime?)null,
                    Multa = reader["multa"] != DBNull.Value ? decimal.Parse(reader["multa"].ToString() ?? "0") : (decimal?)null,
                    Estado = reader["estado"].ToString() == "1",
                    FechaCreacion = DateTime.Parse(reader["fecha_creacion"].ToString() ?? "0"),
                    FechaActualizacion = DateTime.Parse(reader["fecha_actualizacion"].ToString() ?? "0")
                };
            });

            return resultContratos;
        }

        public Contrato? GetContrato(int id) {
            Contrato? result = null;

            string query = @$"select {nameof(Contrato.Id)},
                                    {nameof(Contrato.IdInmueble)},
                                    {nameof(Contrato.IdInquilino)},
                                    {nameof(Contrato.FechaDesde)},
                                    {nameof(Contrato.FechaHasta)},
                                    {nameof(Contrato.MontoAlquiler)},
                                    {nameof(Contrato.FechaFinalizacionAnticipada)},
                                    {nameof(Contrato.Multa)},
                                    {nameof(Contrato.Estado)},
                                    {nameof(Contrato.FechaCreacion)},
                                    {nameof(Contrato.FechaActualizacion)}
                              from contrato
                              where {nameof(Contrato.Id)} = {id} and estado = 1;";

            result = this.ExecuteReader<Contrato>(query, (reader) => {
                return new Contrato() {
                    Id = int.Parse(reader["id"].ToString() ?? "0"),
                    IdInmueble = int.Parse(reader["id_inmueble"].ToString() ?? "0"),
                    IdInquilino = int.Parse(reader["id_inquilino"].ToString() ?? "0"),
                    FechaDesde = DateTime.Parse(reader["fecha_desde"].ToString() ?? "0"),
                    FechaHasta = DateTime.Parse(reader["fecha_hasta"].ToString() ?? "0"),
                    MontoAlquiler = decimal.Parse(reader["monto_alquiler"].ToString() ?? "0"),
                    FechaFinalizacionAnticipada = reader["fecha_finalizacion_anticipada"] != DBNull.Value ? DateTime.Parse(reader["fecha_finalizacion_anticipada"].ToString() ?? "0") : (DateTime?)null,
                    Multa = reader["multa"] != DBNull.Value ? decimal.Parse(reader["multa"].ToString() ?? "0") : (decimal?)null,
                    Estado = reader["estado"].ToString() == "1",
                    FechaCreacion = DateTime.Parse(reader["fecha_creacion"].ToString() ?? "0"),
                    FechaActualizacion = DateTime.Parse(reader["fecha_actualizacion"].ToString() ?? "0")
                };
            });

            return result;
        }

        public int InsertarContrato(Contrato contrato) {
            string query = @$"INSERT INTO contrato (
                                {nameof(Contrato.IdInmueble)},
                                {nameof(Contrato.IdInquilino)},
                                {nameof(Contrato.FechaDesde)},
                                {nameof(Contrato.FechaHasta)},
                                {nameof(Contrato.MontoAlquiler)},
                                {nameof(Contrato.FechaFinalizacionAnticipada)},
                                {nameof(Contrato.Multa)},
                                {nameof(Contrato.Estado)})
                              VALUES(
                                @{nameof(Contrato.IdInmueble)},
                                @{nameof(Contrato.IdInquilino)},
                                @{nameof(Contrato.FechaDesde)},
                                @{nameof(Contrato.FechaHasta)},
                                @{nameof(Contrato.MontoAlquiler)},
                                @{nameof(Contrato.FechaFinalizacionAnticipada)},
                                @{nameof(Contrato.Multa)},
                                @{nameof(Contrato.Estado)});
                              SELECT LAST_INSERT_ID();";

            int result = this.ExecuteNonQuery(query, (parameters) => {
                parameters.AddWithValue($"@{nameof(Contrato.IdInmueble)}", contrato.IdInmueble);
                parameters.AddWithValue($"@{nameof(Contrato.IdInquilino)}", contrato.IdInquilino);
                parameters.AddWithValue($"@{nameof(Contrato.FechaDesde)}", contrato.FechaDesde);
                parameters.AddWithValue($"@{nameof(Contrato.FechaHasta)}", contrato.FechaHasta);
                parameters.AddWithValue($"@{nameof(Contrato.MontoAlquiler)}", contrato.MontoAlquiler);
                parameters.AddWithValue($"@{nameof(Contrato.FechaFinalizacionAnticipada)}", (object?)contrato.FechaFinalizacionAnticipada ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Multa)}", (object?)contrato.Multa ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Estado)}", contrato.Estado);
            });

            return result;
        }

        public bool ActualizarContrato(Contrato contrato) {
            string query = @$"UPDATE contrato SET 
                                {nameof(Contrato.IdInmueble)} = @{nameof(Contrato.IdInmueble)}, 
                                {nameof(Contrato.IdInquilino)} = @{nameof(Contrato.IdInquilino)}, 
                                {nameof(Contrato.FechaDesde)} = @{nameof(Contrato.FechaDesde)}, 
                                {nameof(Contrato.FechaHasta)} = @{nameof(Contrato.FechaHasta)}, 
                                {nameof(Contrato.MontoAlquiler)} = @{nameof(Contrato.MontoAlquiler)}, 
                                {nameof(Contrato.FechaFinalizacionAnticipada)} = @{nameof(Contrato.FechaFinalizacionAnticipada)}, 
                                {nameof(Contrato.Multa)} = @{nameof(Contrato.Multa)}, 
                                {nameof(Contrato.Estado)} = @{nameof(Contrato.Estado)}
                              WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)};";

            bool result = 0 < this.ExecuteNonQuery(query, (parameters) => {
                parameters.AddWithValue($"@{nameof(Contrato.Id)}", contrato.Id);
                parameters.AddWithValue($"@{nameof(Contrato.IdInmueble)}", contrato.IdInmueble);
                parameters.AddWithValue($"@{nameof(Contrato.IdInquilino)}", contrato.IdInquilino);
                parameters.AddWithValue($"@{nameof(Contrato.FechaDesde)}", contrato.FechaDesde);
                parameters.AddWithValue($"@{nameof(Contrato.FechaHasta)}", contrato.FechaHasta);
                parameters.AddWithValue($"@{nameof(Contrato.MontoAlquiler)}", contrato.MontoAlquiler);
                parameters.AddWithValue($"@{nameof(Contrato.FechaFinalizacionAnticipada)}", (object?)contrato.FechaFinalizacionAnticipada ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Multa)}", (object?)contrato.Multa ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Estado)}", contrato.Estado);
            });

            return result;
        }

        public bool BajaLogicaContrato(int id) {
            string query = @$"UPDATE contrato SET 
                                {nameof(Contrato.Estado)} = 0
                              WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)};";

            bool result = 0 < this.ExecuteNonQuery(query, (parameters) =>
            {
                parameters.AddWithValue($"@{nameof(Contrato.Id)}", id);
            });

            return result;
        }        
    }
}

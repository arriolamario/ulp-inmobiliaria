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
                                    {nameof(Contrato.Id_Inmueble)},
                                    {nameof(Contrato.Id_Inquilino)},
                                    {nameof(Contrato.Fecha_Desde)},
                                    {nameof(Contrato.Fecha_Hasta)},
                                    {nameof(Contrato.Monto_Alquiler)},
                                    {nameof(Contrato.Fecha_Finalizacion_Anticipada)},
                                    {nameof(Contrato.Multa)},
                                    {nameof(Contrato.Estado)},
                                    {nameof(Contrato.Fecha_Creacion)},
                                    {nameof(Contrato.Fecha_Actualizacion)}
                              from contrato
                              where estado = 1;";

            resultContratos = this.ExecuteReaderList<Contrato>(query, (reader) => {
                return new Contrato() {

                    Id_Inmueble = int.Parse(reader["id_inmueble"].ToString() ?? "0"),
                    Id_Inquilino = int.Parse(reader["id_inquilino"].ToString() ?? "0"),
                    Fecha_Desde = DateTime.Parse(reader["fecha_desde"].ToString() ?? "0"),
                    Fecha_Hasta = DateTime.Parse(reader["fecha_hasta"].ToString() ?? "0"),
                    Monto_Alquiler = decimal.Parse(reader["monto_alquiler"].ToString() ?? "0"),
                    Fecha_Finalizacion_Anticipada = reader["fecha_finalizacion_anticipada"] != DBNull.Value ? DateTime.Parse(reader["fecha_finalizacion_anticipada"].ToString() ?? "0") : (DateTime?)null,
                    Multa = reader["multa"] != DBNull.Value ? decimal.Parse(reader["multa"].ToString() ?? "0") : (decimal?)null,
                    Estado = reader["estado"].ToString() == "1",
                    Fecha_Creacion = DateTime.Parse(reader["fecha_creacion"].ToString() ?? "0"),
                    Fecha_Actualizacion = DateTime.Parse(reader["fecha_actualizacion"].ToString() ?? "0")
                };
            });

            return resultContratos;
        }

        public Contrato? GetContrato(int id) {
            Contrato? result = null;

            string query = @$"select {nameof(Contrato.Id)},
                                    {nameof(Contrato.Id_Inmueble)},
                                    {nameof(Contrato.Id_Inquilino)},
                                    {nameof(Contrato.Fecha_Desde)},
                                    {nameof(Contrato.Fecha_Hasta)},
                                    {nameof(Contrato.Monto_Alquiler)},
                                    {nameof(Contrato.Fecha_Finalizacion_Anticipada)},
                                    {nameof(Contrato.Multa)},
                                    {nameof(Contrato.Estado)},
                                    {nameof(Contrato.Fecha_Creacion)},
                                    {nameof(Contrato.Fecha_Actualizacion)}
                              from contrato
                              where {nameof(Contrato.Id)} = {id} and estado = 1;";

            result = this.ExecuteReader<Contrato>(query, (reader) => {
                return new Contrato() {
                    Id_Inmueble = int.Parse(reader["id_inmueble"].ToString() ?? "0"),
                    Id_Inquilino = int.Parse(reader["id_inquilino"].ToString() ?? "0"),
                    Fecha_Desde = DateTime.Parse(reader["fecha_desde"].ToString() ?? "0"),
                    Fecha_Hasta = DateTime.Parse(reader["fecha_hasta"].ToString() ?? "0"),
                    Monto_Alquiler = decimal.Parse(reader["monto_alquiler"].ToString() ?? "0"),
                    Fecha_Finalizacion_Anticipada = reader["fecha_finalizacion_anticipada"] != DBNull.Value ? DateTime.Parse(reader["fecha_finalizacion_anticipada"].ToString() ?? "0") : (DateTime?)null,
                    Multa = reader["multa"] != DBNull.Value ? decimal.Parse(reader["multa"].ToString() ?? "0") : (decimal?)null,
                    Estado = reader["estado"].ToString() == "1",
                    Fecha_Creacion = DateTime.Parse(reader["fecha_creacion"].ToString() ?? "0"),
                    Fecha_Actualizacion = DateTime.Parse(reader["fecha_actualizacion"].ToString() ?? "0")
                };
            });

            return result;
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
                                {nameof(Contrato.Estado)})
                              VALUES(
                                @{nameof(Contrato.Id_Inmueble)},
                                @{nameof(Contrato.Id_Inquilino)},
                                @{nameof(Contrato.Fecha_Desde)},
                                @{nameof(Contrato.Fecha_Hasta)},
                                @{nameof(Contrato.Monto_Alquiler)},
                                @{nameof(Contrato.Fecha_Finalizacion_Anticipada)},
                                @{nameof(Contrato.Multa)},
                                @{nameof(Contrato.Estado)});
                              SELECT LAST_INSERT_ID();";

            int result = this.ExecuteNonQuery(query, (parameters) => {
                parameters.AddWithValue($"@{nameof(Contrato.Id_Inmueble)}", contrato.Id_Inmueble);
                parameters.AddWithValue($"@{nameof(Contrato.Id_Inquilino)}", contrato.Id_Inquilino);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Desde)}", contrato.Fecha_Desde);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Hasta)}", contrato.Fecha_Hasta);
                parameters.AddWithValue($"@{nameof(Contrato.Monto_Alquiler)}", contrato.Monto_Alquiler);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Finalizacion_Anticipada)}", (object?)contrato.Fecha_Finalizacion_Anticipada ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Multa)}", (object?)contrato.Multa ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Estado)}", contrato.Estado);
            });

            return result;
        }

        public bool ActualizarContrato(Contrato contrato) {
            string query = @$"UPDATE contrato SET 
                                {nameof(Contrato.Id_Inmueble)} = @{nameof(Contrato.Id_Inmueble)}, 
                                {nameof(Contrato.Id_Inquilino)} = @{nameof(Contrato.Id_Inquilino)}, 
                                {nameof(Contrato.Fecha_Desde)} = @{nameof(Contrato.Fecha_Desde)}, 
                                {nameof(Contrato.Fecha_Hasta)} = @{nameof(Contrato.Fecha_Hasta)}, 
                                {nameof(Contrato.Monto_Alquiler)} = @{nameof(Contrato.Monto_Alquiler)}, 
                                {nameof(Contrato.Fecha_Finalizacion_Anticipada)} = @{nameof(Contrato.Fecha_Finalizacion_Anticipada)}, 
                                {nameof(Contrato.Multa)} = @{nameof(Contrato.Multa)}, 
                                {nameof(Contrato.Estado)} = @{nameof(Contrato.Estado)}
                              WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)};";

            bool result = 0 < this.ExecuteNonQuery(query, (parameters) => {
                parameters.AddWithValue($"@{nameof(Contrato.Id_Inmueble)}", contrato.Id_Inmueble);
                parameters.AddWithValue($"@{nameof(Contrato.Id_Inquilino)}", contrato.Id_Inquilino);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Desde)}", contrato.Fecha_Desde);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Hasta)}", contrato.Fecha_Hasta);
                parameters.AddWithValue($"@{nameof(Contrato.Monto_Alquiler)}", contrato.Monto_Alquiler);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Finalizacion_Anticipada)}", (object?)contrato.Fecha_Finalizacion_Anticipada ?? DBNull.Value);
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

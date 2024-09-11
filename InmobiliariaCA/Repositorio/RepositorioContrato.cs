namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

    public class RepositorioContrato : RepositorioBase, IRepositorioContrato
    {

        private IRepositorioInmueble _repositorioInmueble;
        private IRepositorioInquilino _repositorioInquilino;

        public RepositorioContrato(IConfiguration configuration, IRepositorioInmueble repositorioInmueble, IRepositorioInquilino repositorioInquilino) : base(configuration) {
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
                                    {nameof(Contrato.Estado)},
                                    {nameof(Contrato.Fecha_Creacion)},
                                    {nameof(Contrato.Fecha_Actualizacion)}
                            from contrato
                            where estado = 1;";

            resultContratos = this.ExecuteReaderList<Contrato>(query, (parameters) => {}, (reader) => {
                var contrato = new Contrato() {
                    Id = int.Parse(reader["id"].ToString() ?? "0"),
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

                // Cargar los objetos Inmueble e Inquilino usando sus IDs
                contrato.Inmueble =contrato.Inmueble = _repositorioInmueble.GetInmueble(contrato.Id_Inmueble) ?? throw new InvalidOperationException("Inmueble no se encuentra");
                contrato.Inquilino = _repositorioInquilino.GetInquilino(contrato.Id_Inquilino) ?? throw new InvalidOperationException("Inquilino no se encuentra");

                return contrato;
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
                Contrato contrato = new Contrato() {
                    Id = int.Parse(reader["id"].ToString() ?? "0"),
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
                // Cargar los objetos Inmueble e Inquilino usando sus IDs
                contrato.Inmueble =contrato.Inmueble = _repositorioInmueble.GetInmueble(contrato.Id_Inmueble) ?? throw new InvalidOperationException("Inmueble no se encuentra");
                contrato.Inquilino = _repositorioInquilino.GetInquilino(contrato.Id_Inquilino) ?? throw new InvalidOperationException("Inquilino no se encuentra");

                return contrato;
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
                                {nameof(Contrato.Id_Usuario_Creacion)},
                                {nameof(Contrato.Id_Usuario_Finalizacion)},
                                {nameof(Contrato.Fecha_Creacion)},
                                {nameof(Contrato.Fecha_Actualizacion)})
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
                                @{nameof(Contrato.Fecha_Actualizacion)});
                            SELECT LAST_INSERT_ID();";

            int result = this.ExecuteNonQuery(query, (parameters) => {
                parameters.AddWithValue($"@{nameof(Contrato.Id_Inmueble)}", contrato.Id_Inmueble);
                parameters.AddWithValue($"@{nameof(Contrato.Id_Inquilino)}", contrato.Id_Inquilino);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Desde)}", contrato.Fecha_Desde);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Hasta)}", contrato.Fecha_Hasta);
                parameters.AddWithValue($"@{nameof(Contrato.Monto_Alquiler)}", contrato.Monto_Alquiler);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Finalizacion_Anticipada)}", (object?)contrato.Fecha_Finalizacion_Anticipada ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Multa)}", (object?)contrato.Multa ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Id_Usuario_Creacion)}", 1);//contrato.Id_Usuario_Creacion);
                parameters.AddWithValue($"@{nameof(Contrato.Id_Usuario_Finalizacion)}", 2);// (object?)contrato.Id_Usuario_Finalizacion ?? DBNull.Value);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Creacion)}", contrato.Fecha_Creacion);
                parameters.AddWithValue($"@{nameof(Contrato.Fecha_Actualizacion)}", contrato.Fecha_Actualizacion);
            });

            return result;
        }

        public int ActualizarContrato(Contrato contrato) {
            string query = @$"UPDATE contrato SET
                                {nameof(Contrato.Id_Inmueble)} = @{nameof(Contrato.Id_Inmueble)},
                                {nameof(Contrato.Id_Inquilino)} = @{nameof(Contrato.Id_Inquilino)},
                                {nameof(Contrato.Fecha_Desde)} = @{nameof(Contrato.Fecha_Desde)},
                                {nameof(Contrato.Fecha_Hasta)} = @{nameof(Contrato.Fecha_Hasta)},
                                {nameof(Contrato.Monto_Alquiler)} = @{nameof(Contrato.Monto_Alquiler)},
                                {nameof(Contrato.Fecha_Finalizacion_Anticipada)} = @{nameof(Contrato.Fecha_Finalizacion_Anticipada)},
                                {nameof(Contrato.Multa)} = @{nameof(Contrato.Multa)},
                                {nameof(Contrato.Id_Usuario_Finalizacion)} = @{nameof(Contrato.Id_Usuario_Finalizacion)},
                                {nameof(Contrato.Fecha_Actualizacion)} = @{nameof(Contrato.Fecha_Actualizacion)}
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
            });

            return result;
        }

        public bool BajaLogicaContrato(int id) {
            string query = @$"UPDATE contrato SET 
                                {nameof(Contrato.Estado)} = 0
                              WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)};";

            bool result = 0 < this.ExecuteNonQuery(query, (parameters) => {
                parameters.AddWithValue($"@{nameof(Contrato.Id)}", id);
            });

            return result;
        }        
    }


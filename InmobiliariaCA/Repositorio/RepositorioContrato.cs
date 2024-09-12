namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

public class RepositorioContrato : RepositorioBase, IRepositorioContrato
{

    private IRepositorioInmueble _repositorioInmueble;
    private IRepositorioInquilino _repositorioInquilino;

    public RepositorioContrato(IConfiguration configuration, 
        IRepositorioInmueble repositorioInmueble, 
        IRepositorioInquilino repositorioInquilino) : base(configuration)
    {
        _repositorioInmueble = repositorioInmueble;
        _repositorioInquilino = repositorioInquilino;
    }

    public List<Contrato> GetContratos()
    {
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
                                    {nameof(Contrato.Pagado)}
                            from contrato;";

        resultContratos = this.ExecuteReaderList<Contrato>(query, (parameters) => { }, (reader) =>
        {
            var contrato = new Contrato()
            {
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
                Pagado = reader[nameof(Contrato.Pagado)].ToString() == "1"
            };

            // Cargar los objetos Inmueble e Inquilino usando sus IDs
            contrato.Inmueble = _repositorioInmueble.GetInmueble(contrato.Id_Inmueble) ?? throw new InvalidOperationException("Inmueble no se encuentra");
            contrato.Inquilino = _repositorioInquilino.GetInquilino(contrato.Id_Inquilino) ?? throw new InvalidOperationException("Inquilino no se encuentra");

            return contrato;
        });

        return resultContratos;
    }

    public Contrato? GetContrato(int id)
    {
        Console.WriteLine("GetContrato: " + id);
        Contrato? result = null;

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
                                    {nameof(Contrato.Pagado)}
                              from contrato
                                 where {nameof(Contrato.Id)} = {id};";

        result = this.ExecuteReader<Contrato>(query, (reader) =>
        {
            Contrato contrato = new Contrato()
            {
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
                Pagado = reader[nameof(Contrato.Pagado)].ToString() == "1"
            };
            // Cargar los objetos Inmueble e Inquilino usando sus IDs
            contrato.Inmueble  = _repositorioInmueble.GetInmueble(contrato.Id_Inmueble) ?? throw new InvalidOperationException("Inmueble no se encuentra");
            contrato.Inquilino = _repositorioInquilino.GetInquilino(contrato.Id_Inquilino) ?? throw new InvalidOperationException("Inquilino no se encuentra");

            return contrato;
        });
        return result;
    }

    public int InsertarContrato(Contrato contrato)
    {
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

        int result = this.ExecuteNonQuery(query, (parameters) =>
        {
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

    public int ActualizarContrato(Contrato contrato)
    {
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

        int result = this.ExecuteNonQuery(query, (parameters) =>
        {
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

    public int ActualizarContratoPagado(int Id)
    {
        Console.WriteLine("Id actualiuzar contrato pagado: " + Id);
        string query = @$"UPDATE contrato SET
                                {nameof(Contrato.Pagado)} = 1 
                            WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)};";

        int result = this.ExecuteNonQuery(query, (parameters) =>
        {
            parameters.AddWithValue($"@{nameof(Contrato.Id)}", Id);
        });

        return result;
    }

    public bool BajaContrato(int id)
    {
        string query = @$"delete from contrato 
                            where {nameof(Contrato.Id)} = @{nameof(Contrato.Id)};";

        bool result = 0 < this.ExecuteNonQuery(query, (parameters) =>
        {
            parameters.AddWithValue($"@{nameof(Contrato.Id)}", id);
        });

        return result;
    }
}


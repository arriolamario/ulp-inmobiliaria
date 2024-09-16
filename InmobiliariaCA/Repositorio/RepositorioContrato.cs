namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Text;

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
            contrato.Inmueble = _repositorioInmueble.GetInmueble(contrato.Id_Inmueble) ?? throw new InvalidOperationException("Inmueble no se encuentra");
            contrato.Inquilino = _repositorioInquilino.GetInquilino(contrato.Id_Inquilino) ?? throw new InvalidOperationException("Inquilino no se encuentra");
            contrato.Estado = contrato.PagosCompletos() ? EstadoContrato.Finalizado : contrato.Estado;

            return contrato;
        });

        return resultContratos;
    }

    public Contrato? GetContrato(int id) {
        Contrato? result = null;

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

        result = this.ExecuteReader<Contrato>(query, (reader) => {
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
            contrato.Inmueble  = _repositorioInmueble.GetInmueble(contrato.Id_Inmueble) ?? throw new InvalidOperationException("Inmueble no se encuentra");
            contrato.Inquilino = _repositorioInquilino.GetInquilino(contrato.Id_Inquilino) ?? throw new InvalidOperationException("Inquilino no se encuentra");
            contrato.Estado = contrato.PagosCompletos() ? EstadoContrato.Finalizado : contrato.Estado;

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
            parameters.AddWithValue($"@{nameof(Contrato.Id_Usuario_Creacion)}", 1);//contrato.Id_Usuario_Creacion);
            parameters.AddWithValue($"@{nameof(Contrato.Id_Usuario_Finalizacion)}", 2);// (object?)contrato.Id_Usuario_Finalizacion ?? DBNull.Value);
            parameters.AddWithValue($"@{nameof(Contrato.Fecha_Creacion)}", contrato.Fecha_Creacion);
            parameters.AddWithValue($"@{nameof(Contrato.Fecha_Actualizacion)}", contrato.Fecha_Actualizacion);
            parameters.AddWithValue($"@{nameof(Contrato.Cantidad_Cuotas)}", CantidadCuotas(contrato));
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
            parameters.AddWithValue($"@{nameof(Contrato.Estado)}", (object?)contrato.Estado ?? DBNull.Value);
        });

        return result;
    }

    public int ActualizarContratoPagado(int Id, int pagado) {
        Console.WriteLine("Id actualiuzar contrato pagado: " + Id);

        Contrato? contrato = this.GetContrato(Id);

        if(contrato!=null && contrato.Cantidad_Cuotas-contrato.Cuotas_Pagas == 0) {
            contrato.Estado = EstadoContrato.Finalizado;
        }        

        string query = @$"UPDATE contrato SET
                                {nameof(Contrato.Pagado)} = {pagado},
                                {nameof(Contrato.Estado)} = {contrato.Estado.ToString()}
                                {nameof(Contrato.Cuotas_Pagas)} = {nameof(Contrato.Cuotas_Pagas)} + 1                        
                            WHERE {nameof(Contrato.Id)} = @{nameof(Contrato.Id)} AND Cuotas_Pagas < Cantidad_Cuotas;";

        int result = this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Contrato.Id)}", Id);
        });

        return result;
    }

    public bool BajaContrato(int id) {
        string query = @$"delete from contrato 
                            where {nameof(Contrato.Id)} = @{nameof(Contrato.Id)};";

        bool result = 0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Contrato.Id)}", id);
        });

        return result;
    }

    private int CantidadCuotas(Contrato contrato) {

        int meses = ((contrato.Fecha_Hasta.Year - contrato.Fecha_Desde.Year) * 12) + contrato.Fecha_Hasta.Month - contrato.Fecha_Desde.Month;
        return Math.Max(meses, 1);
    }

    public List<Contrato> GetContratosFiltrados(Contrato.ContratoFilter filter) {

        Console.WriteLine("Repositorio Filtros: " + filter.ToString());
        //filter.Estado = EstadoContrato.Finalizado;
        var query = new StringBuilder(@"select * from contrato where 1=1");

        if (filter.InquilinoId.HasValue)
            query.Append($" AND Id_Inquilino = {filter.InquilinoId}");

        if (filter.InmuebleId.HasValue)
            query.Append($" AND Id_Inmueble = {filter.InmuebleId}");

        if (filter.Estado.HasValue)
            query.Append($" AND Estado = '{filter.Estado}'");

        if (filter.FechaDesde.HasValue)
            query.Append($" AND Fecha_Desde >= '{filter.FechaDesde.Value.ToString("yyyy-MM-dd")}'");

        if (filter.FechaHasta.HasValue)
            query.Append($" AND Fecha_Hasta <= '{filter.FechaHasta.Value.ToString("yyyy-MM-dd")}'");

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
            contrato.Inmueble = _repositorioInmueble.GetInmueble(contrato.Id_Inmueble) ?? throw new InvalidOperationException("Inmueble no se encuentra");
            contrato.Inquilino = _repositorioInquilino.GetInquilino(contrato.Id_Inquilino) ?? throw new InvalidOperationException("Inquilino no se encuentra");
            contrato.Estado = contrato.PagosCompletos() ? EstadoContrato.Finalizado : contrato.Estado;

            return contrato;
        });

        return resultContratos;
    }

}


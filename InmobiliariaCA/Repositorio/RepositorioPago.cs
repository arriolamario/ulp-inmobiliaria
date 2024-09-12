using InmobiliariaCA.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace InmobiliariaCA.Repositorio;

public class RepositorioPago : RepositorioBase, IRepositorioPago {
    private IRepositorioContrato _repositorioContrato;
    public RepositorioPago(IConfiguration configuration, IRepositorioContrato repositorioContrato) : base(configuration) {
        _repositorioContrato = repositorioContrato;
       //gi _repositorioUsuario = new RepositorioUsuario(configuration);
    }

    public List<Pago> GetPagos() {
        
            List<Pago> resultPagos = new List<Pago>();

                    string query = @$"SELECT {nameof(Pago.Id)},
                                    {nameof(Pago.Contrato_Id)},
                                    {nameof(Pago.Numero_Pago)},
                                    {nameof(Pago.Fecha_Pago)},
                                    {nameof(Pago.Detalle)},
                                    {nameof(Pago.Importe)},
                                    {nameof(Pago.Estado)},
                                    {nameof(Pago.Creado_Por_Id)}
                                FROM pago;";

                    resultPagos = this.ExecuteReaderList<Pago>(query, (parameters) => {}, (reader) =>
                    {
                        var pago = new Pago() {
                            Id = int.Parse(reader[nameof(Pago.Id)].ToString() ?? "0"),
                            Contrato_Id = int.Parse(reader[nameof(Pago.Contrato_Id)].ToString() ?? "0"),
                            Numero_Pago = int.Parse(reader[nameof(Pago.Numero_Pago)].ToString() ?? "0"),
                            Fecha_Pago = DateTime.Parse(reader[nameof(Pago.Fecha_Pago)].ToString() ?? "0"),
                            Detalle = reader[nameof(Pago.Detalle)].ToString() ?? "",
                            Importe = decimal.Parse(reader[nameof(Pago.Importe)].ToString() ?? "0"),
                            Estado = reader[nameof(Pago.Estado)].ToString() ?? "",
                            Creado_Por_Id = int.Parse(reader[nameof(Pago.Creado_Por_Id)].ToString() ?? "0")
                        };

                        // Cargar los objetos Contrato usando su ID
                        pago.Contrato = _repositorioContrato.GetContrato(pago.Contrato_Id) ?? throw new InvalidOperationException("Contrato no se encuentra");

                        return pago;
                    });

                    return resultPagos;        
    }

    public Pago? GetPago(int Id) {
        Pago? result = null;

        string query = @$"SELECT {nameof(Pago.Id)},
                          {nameof(Pago.Contrato_Id)},
                          {nameof(Pago.Numero_Pago)},
                          {nameof(Pago.Fecha_Pago)},
                          {nameof(Pago.Detalle)},
                          {nameof(Pago.Importe)},
                          {nameof(Pago.Estado)},
                          {nameof(Pago.Creado_Por_Id)}
                        FROM pago
                         WHERE {nameof(Pago.Id)} = {Id};";
        result = this.ExecuteReader<Pago>(query, (reader) => {
             var pago = new Pago() {
                            Id = int.Parse(reader[nameof(Pago.Id)].ToString() ?? "0"),
                            Contrato_Id = int.Parse(reader[nameof(Pago.Contrato_Id)].ToString() ?? "0"),
                            Numero_Pago = int.Parse(reader[nameof(Pago.Numero_Pago)].ToString() ?? "0"),
                            Fecha_Pago = DateTime.Parse(reader[nameof(Pago.Fecha_Pago)].ToString() ?? "0"),
                            Detalle = reader[nameof(Pago.Detalle)].ToString() ?? "",
                            Importe = decimal.Parse(reader[nameof(Pago.Importe)].ToString() ?? "0"),
                            Estado = reader[nameof(Pago.Estado)].ToString() ?? "",
                            Creado_Por_Id = int.Parse(reader[nameof(Pago.Creado_Por_Id)].ToString() ?? "0")
                        };

                        // Cargar los objetos Contrato usando su ID
                        pago.Contrato = _repositorioContrato.GetContrato(pago.Contrato_Id) ?? throw new InvalidOperationException("Contrato no se encuentra");

                        return pago;
        });

        return result;
    }

    public int InsertarPago(Pago pago) {
        
        string query = @$"INSERT INTO pago (
                          {nameof(Pago.Contrato_Id)},
                          {nameof(Pago.Numero_Pago)},
                          {nameof(Pago.Fecha_Pago)},
                          {nameof(Pago.Detalle)},
                          {nameof(Pago.Importe)},
                          {nameof(Pago.Estado)},
                          {nameof(Pago.Creado_Por_Id)}
                ) VALUES (
                          @{nameof(Pago.Contrato_Id)},
                          @{nameof(Pago.Numero_Pago)},
                          @{nameof(Pago.Fecha_Pago)},
                          @{nameof(Pago.Detalle)},
                          @{nameof(Pago.Importe)},
                          @{nameof(Pago.Estado)},
                          @{nameof(Pago.Creado_Por_Id)}
                );
        SELECT LAST_INSERT_ID();";

        int result = this.ExecuteScalar(query, (parameters) => {
            parameters.AddWithValue($"{nameof(Pago.Contrato_Id)}", pago.Contrato_Id);
            parameters.AddWithValue($"{nameof(Pago.Numero_Pago)}", pago.Numero_Pago);
            parameters.AddWithValue($"{nameof(Pago.Fecha_Pago)}", pago.Fecha_Pago);
            parameters.AddWithValue($"{nameof(Pago.Detalle)}", pago.Detalle);
            parameters.AddWithValue($"{nameof(Pago.Importe)}", pago.Importe);
            parameters.AddWithValue($"{nameof(Pago.Estado)}", 1);
            parameters.AddWithValue($"{nameof(Pago.Creado_Por_Id)}", 1);
        });

        //_repositorioContrato.ActualizarContratoPagado(pago.Contrato_Id);
         if (_repositorioContrato.ActualizarContratoPagado(pago.Contrato_Id) == 0) {
                throw new Exception("No se pudo actualizar el estado de pagado del contrato.");
        }

        return result;
    }

    public bool ActualizarPago(Pago pago) {
        string query = @$"UPDATE pago SET 
                          {nameof(Pago.Contrato_Id)},
                          {nameof(Pago.Numero_Pago)},
                          {nameof(Pago.Fecha_Pago)},
                          {nameof(Pago.Detalle)},
                          {nameof(Pago.Importe)},
                          {nameof(Pago.Estado)},
                          {nameof(Pago.Creado_Por_Id)},
                          {nameof(Pago.Anulado_Por_Id)},
                          {nameof(Pago.Fecha_Anulacion)}
        WHERE {nameof(Pago.Id)} = @{nameof(Pago.Id)};";

        bool result = 0 < this.ExecuteNonQuery(query, (parameters) => {
             parameters.AddWithValue($"{nameof(Pago.Contrato_Id)}", pago.Contrato_Id);
            parameters.AddWithValue($"{nameof(Pago.Numero_Pago)}", pago.Numero_Pago);
            parameters.AddWithValue($"{nameof(Pago.Fecha_Pago)}", pago.Fecha_Pago);
            parameters.AddWithValue($"{nameof(Pago.Detalle)}", pago.Detalle);
            parameters.AddWithValue($"{nameof(Pago.Importe)}", pago.Importe);
            parameters.AddWithValue($"{nameof(Pago.Estado)}", pago.Estado);
            parameters.AddWithValue($"{nameof(Pago.Creado_Por_Id)}", pago.Creado_Por_Id);
            parameters.AddWithValue($"{nameof(Pago.Anulado_Por_Id)}", pago.Anulado_Por_Id);
            parameters.AddWithValue($"{nameof(Pago.Fecha_Anulacion)}", pago.Fecha_Anulacion);
        });

        return result;
    }

    public bool AnularPago(int id, int anuladoPorId) {
        string query = @$"UPDATE pago SET 
            {nameof(Pago.Estado)} = 'Anulado', 
            {nameof(Pago.Anulado_Por_Id)} = @{nameof(Pago.Anulado_Por_Id)}, 
            {nameof(Pago.Fecha_Anulacion)} = CURRENT_TIMESTAMP
        WHERE {nameof(Pago.Id)} = @{nameof(Pago.Id)};";

        bool result = 0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"{nameof(Pago.Id)}", id);
            parameters.AddWithValue($"{nameof(Pago.Anulado_Por_Id)}", anuladoPorId);
        });

        return result;
    }

}

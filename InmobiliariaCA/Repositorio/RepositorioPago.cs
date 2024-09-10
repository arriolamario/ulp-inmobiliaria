using InmobiliariaCA.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace InmobiliariaCA.Repositorio;

public class RepositorioPago : RepositorioBase {
    public RepositorioPago(IConfiguration configuration) : base(configuration) {
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
                          {nameof(Pago.Creado_Por_Id)},
                          {nameof(Pago.Anulado_Por_Id)},
                          {nameof(Pago.Fecha_Anulacion)}
                          FROM pago;";

        resultPagos = this.ExecuteReaderList<Pago>(query, (reader) => {
            return new Pago() {
                Id = int.Parse(reader["Id"].ToString() ?? "0"),
                Contrato_Id = int.Parse(reader["Contrato_Id"].ToString() ?? "0"),
                Numero_Pago = int.Parse(reader["Numero_Pago"].ToString() ?? "0"),
                Fecha_Pago = DateTime.Parse(reader["Fecha_Pago"].ToString() ?? "0"),
                Detalle = reader["Detalle"].ToString() ?? "",
                Importe = decimal.Parse(reader["Importe"].ToString() ?? "0"),
                Estado = reader["Estado"].ToString() ?? "",
                Creado_Por_Id = int.Parse(reader["Creado_Por_Id"].ToString() ?? "0"),
                Anulado_Por_Id = reader.IsDBNull(reader.GetOrdinal("Anulado_Por_Id")) ? (int?)null : int.Parse(reader["AnuladoPorId"].ToString() ?? "0"),
                Fecha_Anulacion = reader.IsDBNull(reader.GetOrdinal("Fecha_Anulacion")) ? (DateTime?)null : DateTime.Parse(reader["FechaAnulacion"].ToString() ?? "0")
            };
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
                          {nameof(Pago.Creado_Por_Id)},
                          {nameof(Pago.Anulado_Por_Id)},
                          {nameof(Pago.Fecha_Anulacion)}
                        FROM pago
                         WHERE {nameof(Pago.Id)} = {Id};";
        result = this.ExecuteReader<Pago>(query, (reader) => {
            return new Pago() {
                Id = int.Parse(reader["Id"].ToString() ?? "0"),
                Contrato_Id = int.Parse(reader["Contrato_Id"].ToString() ?? "0"),
                Numero_Pago = int.Parse(reader["Numero_Pago"].ToString() ?? "0"),
                Fecha_Pago = DateTime.Parse(reader["Fecha_Pago"].ToString() ?? "0"),
                Detalle = reader["Detalle"].ToString() ?? "",
                Importe = decimal.Parse(reader["Importe"].ToString() ?? "0"),
                Estado = reader["Estado"].ToString() ?? "",
                Creado_Por_Id = int.Parse(reader["Creado_Por_Id"].ToString() ?? "0"),
                Anulado_Por_Id = reader.IsDBNull(reader.GetOrdinal("Anulado_Por_Id")) ? (int?)null : int.Parse(reader["AnuladoPorId"].ToString() ?? "0"),
                Fecha_Anulacion = reader.IsDBNull(reader.GetOrdinal("Fecha_Anulacion")) ? (DateTime?)null : DateTime.Parse(reader["FechaAnulacion"].ToString() ?? "0")
           };
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
        Console.WriteLine("query: " + query);

        int result = this.ExecuteScalar(query, (parameters) => {
            parameters.AddWithValue($"{nameof(Pago.Contrato_Id)}", pago.Contrato_Id);
            parameters.AddWithValue($"{nameof(Pago.Numero_Pago)}", pago.Numero_Pago);
            parameters.AddWithValue($"{nameof(Pago.Fecha_Pago)}", pago.Fecha_Pago);
            parameters.AddWithValue($"{nameof(Pago.Detalle)}", pago.Detalle);
            parameters.AddWithValue($"{nameof(Pago.Importe)}", pago.Importe);
            parameters.AddWithValue($"{nameof(Pago.Estado)}", 1);
            parameters.AddWithValue($"{nameof(Pago.Creado_Por_Id)}", 1);
        });

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

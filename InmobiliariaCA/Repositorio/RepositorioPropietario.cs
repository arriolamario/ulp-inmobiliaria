using InmobiliariaCA.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaCA.Repositorio;

public class RepositorioPropietario : RepositorioBase
{
    public RepositorioPropietario(IConfiguration configuration): base(configuration)
    {
    }

    public List<Propietario> GetPropietarios()
    {
        List<Propietario> resultPropietarios = new List<Propietario>();

        string query = @$"select {nameof(Propietario.Id)}, 
                                {nameof(Propietario.Dni)}, 
                                {nameof(Propietario.Nombre)}, 
                                {nameof(Propietario.Apellido)}, 
                                {nameof(Propietario.Telefono)}, 
                                {nameof(Propietario.Email)}, 
                                {nameof(Propietario.Direccion)}, 
                                {nameof(Propietario.Fecha_Creacion)}, 
                                {nameof(Propietario.Fecha_Actualizacion)},
                                {nameof(Propietario.Estado)}
                        from propietario
                        where Estado = 1;";

        resultPropietarios = this.ExecuteReaderList<Propietario>(query, (reader) => {
            return new Propietario()
                        {
                            Apellido = reader["apellido"].ToString() ?? "",
                            Dni = reader["dni"].ToString() ?? "",
                            Email = reader["email"].ToString() ?? "",
                            Nombre = reader["nombre"].ToString() ?? "",
                            TelefonoArea = reader["telefono"].ToString()?.Split('-')[0] ?? "",
                            TelefonoNumero = reader["telefono"].ToString()?.Split('-')[1] ?? "",
                            Direccion = reader["direccion"].ToString() ?? "",
                            Id = int.Parse(reader["id"].ToString() ?? "0"),
                            Estado = int.Parse(reader["estado"].ToString() ?? "0"),
                            Fecha_Creacion = DateTime.Parse(reader["fecha_creacion"].ToString() ?? "0"),
                            Fecha_Actualizacion = DateTime.Parse(reader["fecha_actualizacion"].ToString() ?? "0")
                        };
        });
        return resultPropietarios;
    }

    public Propietario? GetPropietario(int Id)
    {
        Propietario? result = default(Propietario);

        string query = @$"select {nameof(Propietario.Id)}, 
                                {nameof(Propietario.Dni)}, 
                                {nameof(Propietario.Nombre)}, 
                                {nameof(Propietario.Apellido)}, 
                                {nameof(Propietario.Telefono)}, 
                                {nameof(Propietario.Email)}, 
                                {nameof(Propietario.Direccion)}, 
                                {nameof(Propietario.Fecha_Creacion)}, 
                                {nameof(Propietario.Fecha_Actualizacion)}, 
                                {nameof(Propietario.Estado)}
                        from propietario
                        where {nameof(Propietario.Id)} = {Id};";

        result = this.ExecuteReader<Propietario>(query, (reader) => {
            return new Propietario()
                        {
                            Apellido = reader["apellido"].ToString() ?? "",
                            Dni = reader["dni"].ToString() ?? "",
                            Email = reader["email"].ToString() ?? "",
                            Nombre = reader["nombre"].ToString() ?? "",
                            TelefonoArea = reader["telefono"].ToString()?.Split('-')[0] ?? "",
                            TelefonoNumero = reader["telefono"].ToString()?.Split('-')[1] ?? "",
                            Direccion = reader["direccion"].ToString() ?? "",
                            Id = int.Parse(reader["id"].ToString() ?? "0"),
                            Estado = int.Parse(reader["estado"].ToString() ?? "0"),
                            Fecha_Creacion = DateTime.Parse(reader["fecha_creacion"].ToString() ?? "0"),
                            Fecha_Actualizacion = DateTime.Parse(reader["fecha_actualizacion"].ToString() ?? "0")
                        };
        });
        return result;
    }


    public bool ExistePropietarioPorDni(string dni)
    {
        bool existe = false;
        string query = @$"select count(*) from propietario where {nameof(Propietario.Dni)} = @{nameof(Propietario.Dni)};";

        existe = this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Propietario.Dni)}", dni);
        }) > 0;
        return existe;
    }


    public bool ActualizarPropietario(Propietario propietario)
    {
        bool result = false;
        string query = @$"UPDATE propietario SET 
                                {nameof(Propietario.Nombre)} = @{nameof(Propietario.Nombre)}, 
                                {nameof(Propietario.Apellido)} = @{nameof(Propietario.Apellido)}, 
                                {nameof(Propietario.Telefono)} = @{nameof(Propietario.Telefono)}, 
                                {nameof(Propietario.Email)} = @{nameof(Propietario.Email)}, 
                                {nameof(Propietario.Direccion)} = @{nameof(Propietario.Direccion)}
                        where {nameof(Propietario.Id)} = @{nameof(Propietario.Id)};";

        result = 0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Propietario.Id)}", propietario.Id);
            parameters.AddWithValue($"@{nameof(Propietario.Nombre)}", propietario.Nombre);
            parameters.AddWithValue($"@{nameof(Propietario.Apellido)}", propietario.Apellido);
            parameters.AddWithValue($"@{nameof(Propietario.Telefono)}", propietario.Telefono);
            parameters.AddWithValue($"@{nameof(Propietario.Email)}", propietario.Email);
            parameters.AddWithValue($"@{nameof(Propietario.Direccion)}", propietario.Direccion);
        });
        return result;
    }

    public int InsertarPropietario(Propietario propietario)
    {
        string query = @$"INSERT INTO propietario ({nameof(Propietario.Dni)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Telefono)}, {nameof(Propietario.Email)}, {nameof(Propietario.Direccion)})
                            VALUES(@{nameof(Propietario.Dni)}, @{nameof(Propietario.Nombre)}, @{nameof(Propietario.Apellido)}, @{nameof(Propietario.Telefono)}, @{nameof(Propietario.Email)}, @{nameof(Propietario.Direccion)});
                            SELECT LAST_INSERT_ID();";

        int result = this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Propietario.Dni)}", propietario.Dni);
            parameters.AddWithValue($"@{nameof(Propietario.Nombre)}", propietario.Nombre);
            parameters.AddWithValue($"@{nameof(Propietario.Apellido)}", propietario.Apellido);
            parameters.AddWithValue($"@{nameof(Propietario.Telefono)}", propietario.Telefono);
            parameters.AddWithValue($"@{nameof(Propietario.Email)}", propietario.Email);
            parameters.AddWithValue($"@{nameof(Propietario.Direccion)}", propietario.Direccion);
        });
        return result;
    }

    public bool BajaPropietario(int Id)
    {
        bool result = false;
        string query = @$"UPDATE propietario SET {nameof(Propietario.Estado)} = 0 WHERE {nameof(Propietario.Id)} = @{nameof(Propietario.Id)};";

        result = 0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Propietario.Id)}", Id);
        });

        return result;
    }

}
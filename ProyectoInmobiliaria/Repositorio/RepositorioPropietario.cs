using ProyectoInmobiliaria.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace ProyectoInmobiliaria.Repositorio;

public class RepositorioPropietario : RepositorioBase
{
    public RepositorioPropietario(IConfiguration configuration): base(configuration)
    {
    }

    public List<Propietario> GetPropietarios()
    {
        List<Propietario> resultPropietarios = new List<Propietario>();

        string query = @"select id, dni, nombre, apellido, telefono, email, direccion from propietario;";

        resultPropietarios = this.ExecuteReader<Propietario>(query, (reader) => {
            return new Propietario()
                        {
                            apellido = reader["apellido"].ToString() ?? "",
                            dni = reader["dni"].ToString() ?? "",
                            email = reader["email"].ToString() ?? "",
                            nombre = reader["nombre"].ToString() ?? "",
                            telefono = reader["telefono"].ToString() ?? "",
                            direccion = reader["direccion"].ToString() ?? "",
                            id = int.Parse(reader["id"].ToString() ?? "0"),
                        };
        });
        return resultPropietarios;

    }

}
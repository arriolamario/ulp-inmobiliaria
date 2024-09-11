using InmobiliariaCA.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace InmobiliariaCA.Repositorio;

public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario
{
    // IRepositorioInmueble _RepositorioInmueble;
    public RepositorioPropietario(IConfiguration configuration): base(configuration)
    {
        // _RepositorioInmueble = repositorioInmueble;
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
                        from propietario;";

        resultPropietarios = this.ExecuteReaderList<Propietario>(query, (parameters) => {}, (reader) => {
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
                            Fecha_Actualizacion = DateTime.Parse(reader["fecha_actualizacion"].ToString() ?? "0"),
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

        if(result != null){
            result.Inmuebles = GetInmuebles(result.Id);
        }

        return result;
    }

    private List<Inmueble> GetInmuebles(int idPropietario)
    {
        List<Inmueble> resultInmuebles = new List<Inmueble>();

        string query = @$"select {nameof(Inmueble.Id)}, 
                                {nameof(Inmueble.Direccion)},
                                {nameof(Inmueble.Id_Tipo_Inmueble_Uso)},
                                {nameof(Inmueble.Id_Tipo_Inmueble)},
                                {nameof(Inmueble.Ambientes)},
                                {nameof(Inmueble.Coordenada_Lat)}, 
                                {nameof(Inmueble.Coordenada_Lon)}, 
                                {nameof(Inmueble.Precio)},
                                {nameof(Inmueble.Id_Propietario)},
                                {nameof(Inmueble.Fecha_Creacion)},
                                {nameof(Inmueble.Fecha_Actualizacion)}		
                                from inmueble
                                where {nameof(Inmueble.Id_Propietario)} = @{nameof(Inmueble.Id_Propietario)};";

        resultInmuebles = this.ExecuteReaderList<Inmueble>(query,(parameters) => {
            parameters.AddWithValue($"@{nameof(Inmueble.Id_Propietario)}", idPropietario);
        }, (reader) =>  {
            return new Inmueble()
            {
                Id = int.Parse(reader[nameof(Inmueble.Id)].ToString() ?? "0"),
                Direccion = reader["direccion"].ToString() ?? "",
                Id_Tipo_Inmueble_Uso = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble_Uso)].ToString() ?? "0"),
                Id_Tipo_Inmueble = int.Parse(reader[nameof(Inmueble.Id_Tipo_Inmueble)].ToString() ?? "0"),
                Ambientes = int.Parse(reader[nameof(Inmueble.Ambientes)].ToString() ?? "0"),
                Coordenada_Lat = reader[nameof(Inmueble.Coordenada_Lat)].ToString() ?? "",
                Coordenada_Lon = reader[nameof(Inmueble.Coordenada_Lon)].ToString() ?? "",
                Precio = decimal.Parse(reader[nameof(Inmueble.Precio)].ToString() ?? "0"),
                Id_Propietario = int.Parse(reader[nameof(Inmueble.Id_Propietario)].ToString() ?? "0"),
                Fecha_Creacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Creacion)].ToString() ?? "0"),
                Fecha_Actualizacion = DateTime.Parse(reader[nameof(Inmueble.Fecha_Actualizacion)].ToString() ?? "0")
            };
        }); 

        List<int> propietariosIds = resultInmuebles.Select(x => x.Id_Propietario).ToList();

        return resultInmuebles;
    }


    public bool ExistePropietarioPorDni(string dni)
    {
        bool existe = false;
        string query = @$"select count(*) from propietario where {nameof(Propietario.Dni)} = @{nameof(Propietario.Dni)};";

        existe = this.ExecuteScalar(query, (parameters) => {
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

        int result = this.ExecuteScalar(query, (parameters) => {
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


    public List<Propietario> GetPropietarios(List<int> propietariosIds)
    {
        List<Propietario> result = new List<Propietario>();
        if(propietariosIds.Count == 0) return result;
        string query = @$"select * from propietario where {nameof(Propietario.Id)} in ({string.Join(",", propietariosIds)});";
        result = this.ExecuteReaderList<Propietario>(query, (parameters) => {}, (reader) => {
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

}
namespace ProyectoInmobiliaria.Repositorio;

using ProyectoInmobiliaria.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

public class RepositorioInquilino : RepositorioBase {
    public RepositorioInquilino(IConfiguration configuration) : base(configuration) {
    }

    public List<Inquilino> GetInquilinos() {
        List<Inquilino> resultInquilinos = new List<Inquilino>();

        string query = @$"select {nameof(Inquilino.Id)}, 
                            {nameof(Inquilino.Dni)}, 
                            {nameof(Inquilino.Nombre)}, 
                            {nameof(Inquilino.Apellido)}, 
                            {nameof(Inquilino.Telefono)}, 
                            {nameof(Inquilino.Email)}, 
                            {nameof(Inquilino.Direccion)}, 
                            {nameof(Inquilino.Fecha_Creacion)}, 
                            {nameof(Inquilino.Fecha_Actualizacion)} 
                    from inquilino
                    where activo = 1;";

        resultInquilinos = this.ExecuteReaderList<Inquilino>(query, (reader) => {
            return new Inquilino()
            {
                Apellido = reader["apellido"].ToString() ?? "",
                Dni = reader["dni"].ToString() ?? "",
                Email = reader["email"].ToString() ?? "",
                Nombre = reader["nombre"].ToString() ?? "",
                TelefonoArea = reader["telefono"].ToString().Split('-')[0] ?? "",
                TelefonoNumero = reader["telefono"].ToString().Split('-')[1] ?? "",
                Direccion = reader["direccion"].ToString() ?? "",
                Id = int.Parse(reader["id"].ToString() ?? "0"),
                Fecha_Creacion = DateTime.Parse(reader["fecha_creacion"].ToString() ?? "0"),
                Fecha_Actualizacion = DateTime.Parse(reader["fecha_actualizacion"].ToString() ?? "0")
            };
        });
        return resultInquilinos;
    }

    public Inquilino GetInquilino(int Id) {
        Inquilino result = default(Inquilino);

        string query = @$"select {nameof(Inquilino.Id)}, 
                            {nameof(Inquilino.Dni)}, 
                            {nameof(Inquilino.Nombre)}, 
                            {nameof(Inquilino.Apellido)}, 
                            {nameof(Inquilino.Telefono)}, 
                            {nameof(Inquilino.Email)}, 
                            {nameof(Inquilino.Direccion)}, 
                            {nameof(Inquilino.Fecha_Creacion)}, 
                            {nameof(Inquilino.Fecha_Actualizacion)} 
                    from inquilino
                    where {nameof(Inquilino.Id)} = {Id} and activo = 1;";

        result = this.ExecuteReader<Inquilino>(query, (reader) => {
            return new Inquilino() {
                Apellido = reader["apellido"].ToString() ?? "",
                Dni = reader["dni"].ToString() ?? "",
                Email = reader["email"].ToString() ?? "",
                Nombre = reader["nombre"].ToString() ?? "",
                TelefonoArea = reader["telefono"].ToString().Split('-')[0] ?? "",
                TelefonoNumero = reader["telefono"].ToString().Split('-')[1] ?? "",
                Direccion = reader["direccion"].ToString() ?? "",
                Id = int.Parse(reader["id"].ToString() ?? "0"),
                Fecha_Creacion = DateTime.Parse(reader["fecha_creacion"].ToString() ?? "0"),
                Fecha_Actualizacion = DateTime.Parse(reader["fecha_actualizacion"].ToString() ?? "0")
            };
        });

        return result;
    }

    public int InsertarInquilino(Inquilino inquilino) {
         string query = @$"INSERT INTO inquilino (
                {nameof(Inquilino.Dni)}, 
                {nameof(Inquilino.Nombre)}, 
                {nameof(Inquilino.Apellido)}, 
                {nameof(Inquilino.Telefono)}, 
                {nameof(Inquilino.Email)}, 
                {nameof(Inquilino.Direccion)})
            VALUES(
                @{nameof(Inquilino.Dni)}, 
                @{nameof(Inquilino.Nombre)}, 
                @{nameof(Inquilino.Apellido)}, 
                @{nameof(Inquilino.Telefono)}, 
                @{nameof(Inquilino.Email)}, 
                @{nameof(Inquilino.Direccion)});
             SELECT LAST_INSERT_ID();";

        int result = this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Inquilino.Dni)}", inquilino.Dni);
            parameters.AddWithValue($"@{nameof(Inquilino.Nombre)}", inquilino.Nombre);
            parameters.AddWithValue($"@{nameof(Inquilino.Apellido)}", inquilino.Apellido);
            parameters.AddWithValue($"@{nameof(Inquilino.Telefono)}", inquilino.Telefono);
            parameters.AddWithValue($"@{nameof(Inquilino.Email)}", inquilino.Email);
            parameters.AddWithValue($"@{nameof(Inquilino.Direccion)}", inquilino.Direccion);
        });

        return result;
    }

    public bool ActualizarInquilino(Inquilino inquilino) {
        bool result = false;
        string query = @$"UPDATE inquilino SET 
                                {nameof(Inquilino.Dni)} = @{nameof(Inquilino.Dni)}, 
                                {nameof(Inquilino.Nombre)} = @{nameof(Inquilino.Nombre)}, 
                                {nameof(Inquilino.Apellido)} = @{nameof(Inquilino.Apellido)}, 
                                {nameof(Inquilino.Telefono)} = @{nameof(Inquilino.Telefono)}, 
                                {nameof(Inquilino.Email)} = @{nameof(Inquilino.Email)}, 
                                {nameof(Inquilino.Direccion)} = @{nameof(Inquilino.Direccion)}
                        where {nameof(Inquilino.Id)} = @{nameof(Inquilino.Id)};";

        result = 0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Inquilino.Id)}", inquilino.Id);
            parameters.AddWithValue($"@{nameof(Inquilino.Dni)}", inquilino.Dni);
            parameters.AddWithValue($"@{nameof(Inquilino.Nombre)}", inquilino.Nombre);
            parameters.AddWithValue($"@{nameof(Inquilino.Apellido)}", inquilino.Apellido);
            parameters.AddWithValue($"@{nameof(Inquilino.Telefono)}", inquilino.Telefono);
            parameters.AddWithValue($"@{nameof(Inquilino.Email)}", inquilino.Email);
            parameters.AddWithValue($"@{nameof(Inquilino.Direccion)}", inquilino.Direccion);
        });

        return result;
    }

    public bool ExisteInquilinoPorDni(string dni) {
        bool existe = false;

        string query = @$"SELECT * 
                        FROM inquilino
                        WHERE {nameof(Inquilino.Dni)} = @Dni AND activo = 1;";
     
        existe =  0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Inquilino.Dni)}", dni);
        });

        return existe;
    }

    public bool BajaLogicaInquilino(int id) {
        bool result = false;

        string query = @$"UPDATE inquilino SET 
                            {nameof(Inquilino.Activo)} = 0
                        WHERE {nameof(Inquilino.Id)} = @{nameof(Inquilino.Id)};";

        result = 0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Inquilino.Id)}", id);
        });

        return result;
    }
}
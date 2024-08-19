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

        result = this.ExecuteReader<Inquilino>(query, (reader) =>
        {
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
        return result;
    }
}
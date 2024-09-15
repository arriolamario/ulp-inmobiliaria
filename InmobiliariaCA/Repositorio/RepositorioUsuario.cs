namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

public class RepositorioUsuario : RepositorioBase, IRepositorioUsuario
{
    IConfiguration _configuration;
    public RepositorioUsuario(IConfiguration configuration) : base(configuration)
    {
        _configuration = configuration;
    }

    public List<Usuario> GetUsuarios()
    {
        List<Usuario> resultUsuarios = new List<Usuario>();

        string query = $@"select 
                            {nameof(Usuario.Id)},
                            {nameof(Usuario.Email)},
                            {nameof(Usuario.Password_Hash)},
                            {nameof(Usuario.Nombre)},
                            {nameof(Usuario.Apellido)},
                            {nameof(Usuario.Avatar_Url)},
                            {nameof(Usuario.Rol)},
                            {nameof(Usuario.Fecha_Creacion)},
                            {nameof(Usuario.Fecha_Actualizacion)}
                    from usuario;";

        resultUsuarios =this.ExecuteReaderList<Usuario>(query, (parameters) => { }, (reader) => {
            return new Usuario(){
                Id = reader.GetInt32(nameof(Usuario.Id)),
                Apellido = reader.GetString(nameof(Usuario.Apellido)),
                Email = reader.GetString(nameof(Usuario.Email)),
                Nombre = reader.GetString(nameof(Usuario.Nombre)),
                Password_Hash = reader.GetString(nameof(Usuario.Password_Hash)),
                Avatar_Url = reader.IsDBNull(reader.GetOrdinal(nameof(Usuario.Avatar_Url))) ? "" :  reader.GetString(nameof(Usuario.Avatar_Url)),
                Rol = reader.GetString(nameof(Usuario.Rol)),
                Fecha_Creacion = reader.GetDateTime(nameof(Usuario.Fecha_Creacion)),
                Fecha_Actualizacion = reader.GetDateTime(nameof(Usuario.Fecha_Actualizacion))
            };
        });

        return resultUsuarios;
    }

    public Usuario? GetUsuario(int Id)
    {
        Usuario? usuario;

        string query = $@"select 
                            {nameof(Usuario.Id)},
                            {nameof(Usuario.Email)},
                            {nameof(Usuario.Password_Hash)},
                            {nameof(Usuario.Nombre)},
                            {nameof(Usuario.Apellido)},
                            {nameof(Usuario.Avatar_Url)},
                            {nameof(Usuario.Rol)},
                            {nameof(Usuario.Fecha_Creacion)},
                            {nameof(Usuario.Fecha_Actualizacion)}
                    from usuario
                    where {nameof(Usuario.Id)} = @{nameof(Usuario.Id)};";

        usuario =this.ExecuteReader<Usuario>(query, (parameters) => { 
            parameters.AddWithValue(@$"@{nameof(Usuario.Id)}", Id);
        }, (reader) => {
            return new Usuario(){
                Id = reader.GetInt32(nameof(Usuario.Id)),
                Apellido = reader.GetString(nameof(Usuario.Apellido)),
                Email = reader.GetString(nameof(Usuario.Email)),
                Nombre = reader.GetString(nameof(Usuario.Nombre)),
                Password_Hash = reader.GetString(nameof(Usuario.Password_Hash)),
                Avatar_Url = reader.IsDBNull(reader.GetOrdinal(nameof(Usuario.Avatar_Url))) ? "" :  reader.GetString(nameof(Usuario.Avatar_Url)),
                Rol = reader.GetString(nameof(Usuario.Rol)),
                Fecha_Creacion = reader.GetDateTime(nameof(Usuario.Fecha_Creacion)),
                Fecha_Actualizacion = reader.GetDateTime(nameof(Usuario.Fecha_Actualizacion))
            };
        });

        return usuario;
    }

    public int InsertarUsuario(Usuario usuario)
    {
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
								password: "123456",
								salt: System.Text.Encoding.ASCII.GetBytes(_configuration["Salt"] ?? ""),
								prf: KeyDerivationPrf.HMACSHA1,
								iterationCount: 1000,
								numBytesRequested: 256 / 8));
        usuario.Password_Hash = hashed;

        string query = $@"INSERT INTO usuario
                            ({nameof(Usuario.Email)},
                            {nameof(Usuario.Password_Hash)},
                            {nameof(Usuario.Nombre)},
                            {nameof(Usuario.Apellido)},
                            {nameof(Usuario.Rol)})
                        VALUES
                            (@{nameof(Usuario.Email)},
                            @{nameof(Usuario.Password_Hash)},
                            @{nameof(Usuario.Nombre)},
                            @{nameof(Usuario.Apellido)},
                            @{nameof(Usuario.Rol)});
                        SELECT LAST_INSERT_ID();";
        
        usuario.Id = this.ExecuteScalar(query, (parameters) => {
            parameters.AddWithValue(@$"@{nameof(Usuario.Email)}", usuario.Email);
            parameters.AddWithValue(@$"@{nameof(Usuario.Password_Hash)}", usuario.Password_Hash);
            parameters.AddWithValue(@$"@{nameof(Usuario.Nombre)}", usuario.Nombre);
            parameters.AddWithValue(@$"@{nameof(Usuario.Apellido)}", usuario.Apellido);
            parameters.AddWithValue(@$"@{nameof(Usuario.Rol)}", usuario.Rol);
        });
        
        return usuario.Id;
    }

    public int ActualizarUsuario(Usuario usuario)
    {
        string query = $@"UPDATE usuario
                        SET {nameof(Usuario.Email)} = @{nameof(Usuario.Email)},
                            {nameof(Usuario.Nombre)} = @{nameof(Usuario.Nombre)},
                            {nameof(Usuario.Apellido)} = @{nameof(Usuario.Apellido)},
                            {nameof(Usuario.Rol)} = @{nameof(Usuario.Rol)}
                        WHERE {nameof(Usuario.Id)} = @{nameof(Usuario.Id)};";

        return this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue(@$"@{nameof(Usuario.Id)}", usuario.Id);
            parameters.AddWithValue(@$"@{nameof(Usuario.Email)}", usuario.Email);   
            parameters.AddWithValue(@$"@{nameof(Usuario.Nombre)}", usuario.Nombre);
            parameters.AddWithValue(@$"@{nameof(Usuario.Apellido)}", usuario.Apellido);
            parameters.AddWithValue(@$"@{nameof(Usuario.Rol)}", usuario.Rol);
        });
    }

    public bool BajaUsuario(int Id)
    {
        bool result = false;
        string query = $@"delete from usuario
                        where {nameof(Usuario.Id)} = @{nameof(Usuario.Id)};";

        result = 0 < this.ExecuteNonQuery(query, (parameters) => {
            parameters.AddWithValue($"@{nameof(Usuario.Id)}", Id);
        });
        return result;
    }
}


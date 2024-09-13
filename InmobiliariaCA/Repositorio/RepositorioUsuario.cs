namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

public class RepositorioUsuario : RepositorioBase, IRepositorioUsuario
{
    public RepositorioUsuario(IConfiguration configuration) : base(configuration)
    {
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
                Avatar_Url = reader.GetString(nameof(Usuario.Avatar_Url)),
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
                Avatar_Url = reader.GetString(nameof(Usuario.Avatar_Url)),
                Rol = reader.GetString(nameof(Usuario.Rol)),
                Fecha_Creacion = reader.GetDateTime(nameof(Usuario.Fecha_Creacion)),
                Fecha_Actualizacion = reader.GetDateTime(nameof(Usuario.Fecha_Actualizacion))
            };
        });

        return usuario;
    }

    public int InsertarUsuario(Usuario usuario)
    {
        string query = $@"INSERT INTO usuario
                            ({nameof(Usuario.Email)},
                            {nameof(Usuario.Password_Hash)},
                            {nameof(Usuario.Nombre)},
                            {nameof(Usuario.Apellido)},
                            {nameof(Usuario.Avatar_Url)},
                            {nameof(Usuario.Rol)})
                        VALUES
                            (@{nameof(Usuario.Email)},
                            @{nameof(Usuario.Password_Hash)},
                            @{nameof(Usuario.Nombre)},
                            @{nameof(Usuario.Apellido)},
                            @{nameof(Usuario.Avatar_Url)},
                            @{nameof(Usuario.Rol)});";
        
        
        return 0;
    }

    public int ActualizarUsuario(Usuario usuario)
    {
        return 0;
    }

    public bool BajaUsuario(int Id)
    {
        return true;
    }
}


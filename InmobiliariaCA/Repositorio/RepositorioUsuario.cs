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
                        {nameof(Usuario.Apellido)}, 
                        {nameof(Usuario.Email)}, 
                        {nameof(Usuario.Id)}, 
                        {nameof(Usuario.Nombre)}, 
                        {nameof(Usuario.Telefono)} 
                    from usuario;";

        resultUsuarios =this.ExecuteReaderList<Usuario>(query, (parameters) => { }, (reader) => {
            return new Usuario(){
                Id = reader.GetInt32(0),
                Apellido = reader.GetString(1),
                Email = reader.GetString(2),
                Nombre = reader.GetString(3),
                Telefono = reader.GetString(4)
            };
        });

        return resultUsuarios;
    }

    public Usuario? GetUsuario(int Id)
    {
        return null;
    }

    public int InsertarContrato(Usuario usuario)
    {
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


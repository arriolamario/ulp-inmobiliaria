namespace InmobiliariaCA.Repositorio;
using InmobiliariaCA.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;

public interface IRepositorioUsuario
{
    public List<Usuario> GetUsuarios();
    public Usuario? GetUsuario(int Id);
    public int InsertarUsuario(Usuario usuario);
    public int ActualizarUsuario(Usuario usuario);
    public bool BajaUsuario(int Id);
}


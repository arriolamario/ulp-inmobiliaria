using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InmobiliariaCA.Models;

namespace InmobiliariaCA.Models;

public class UsuarioLoginViewModel
{
    public UsuarioLoginViewModel()
    {
        
    }
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

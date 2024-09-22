using System.ComponentModel.DataAnnotations;

namespace InmobiliariaCA.Models;

public class UsuarioResetPasswordViewModel
{
    public UsuarioResetPasswordViewModel()
    {
        
    }
    public UsuarioResetPasswordViewModel(Usuario u)
    {
        this.Email = u.Email;
    }
    public string Email { get; set; } = "";
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string NewPassword { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmar contraseña")]
    [Compare("NewPassword", ErrorMessage = "La contraseña y el confirmar contraseña no coinciden")]
    public string ConfirmPassword { get; set; } = "";
}


namespace InmobiliariaCA.Models;
using System.ComponentModel.DataAnnotations;

public class Propietario
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El documento es obligatorio.")]
    [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El documento debe tener 7 u 8 dígitos.")]	
    public string Dni { get; set; } = "";
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = "";
    [Required(ErrorMessage = "El apellido es obligatorio.")]
    public string Apellido { get; set; } = "";
    private string telefono = "";
    public string Telefono
    {
        get { return $"{TelefonoArea}-{TelefonoNumero}"; }
        set { telefono = value; }
    }
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
    public string Email { get; set; } = "";
    [Required(ErrorMessage = "La dirección es obligatoria.")]
    public string Direccion { get; set; } = "";
    public DateTime Fecha_Creacion { get; set; }
    public DateTime Fecha_Actualizacion { get; set; }
    [Required(ErrorMessage = "El area es obligatorio.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "El area solo debe tener dígitos.")]	
    public string TelefonoArea { get; set; } = "";
    [Required(ErrorMessage = "El numero de telefono es obligatorio.")]
    [RegularExpression(@"^\d+$", ErrorMessage = "El numero solo debe tener dígitos.")]	
    public string TelefonoNumero { get; set; } = "";
    public int Estado { get; set; } = 1;
    public List<Inmueble> Inmuebles = new List<Inmueble>();
}

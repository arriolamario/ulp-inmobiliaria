namespace InmobiliariaCA.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PropietarioApi
{
    public int Id { get; set; }
    public string Documento { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Apellido { get; set; } = "";
    
    public string TelefonoArea {get; set;} = "";
    public string TelefonoNumero {get; set;} = "";
    public string Email { get; set; } = "";
    public string Direccion { get; set; } = "";
    public string Password_Hash { get; set; } = "";
    public string Avatar_Url { get; set; } = "";
    public string Usuario { get; set; } = "";
}

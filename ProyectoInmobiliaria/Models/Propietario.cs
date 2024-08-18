namespace ProyectoInmobiliaria.Models;

public class Propietario
{
    public int Id { get; set; }
    public string Dni { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Apellido { get; set; } = "";
    private string telefono = "";
    public string Telefono
    {
        get { return $"{TelefonoArea}-{TelefonoNumero}"; }
        set { telefono = value; }
    }
    
    public string Email { get; set; } = "";
    public string Direccion { get; set; } = "";
    public DateTime Fecha_Creacion { get; set; }
    public DateTime Fecha_Actualizacion { get; set; }
    public string TelefonoArea { get; set; } = "";
    public string TelefonoNumero { get; set; } = "";
}

namespace InmobiliariaCA.Models
{
    public class Miembro
    {
        public string Nombre { get; set; } = "";
        public string Rol { get; set; } = "";
        public string Email { get; set; } = "";
        public string Telefono { get; set; } = "";
        public string FotoUrl { get; set; } = "";  // URL de la foto del miembro (opcional)
        public string LinkedInUrl { get; set; } = "";  // Enlace al perfil de LinkedIn (opcional)
    }
}

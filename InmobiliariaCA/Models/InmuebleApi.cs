namespace InmobiliariaCA.Models
{
    public class InmuebleApi
    {
        public string Direccion { get; set; }
        public int Ambientes { get; set; }
        public decimal Precio { get; set; }
        public int IdTipo { get; set; }
        public int IdUso { get; set; }
        public bool Activo { get; set; }
    }
}
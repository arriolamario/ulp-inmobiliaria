namespace InmobiliariaCA.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InmobiliariaCA.Models.ContratoModels;

public class InmuebleViewModel
{
    public InmuebleViewModel()
    {
        
    }

    public InmuebleViewModel(Inmueble inmueble, List<Contrato> contratos)
    {
        this.Id = inmueble.Id;
        this.Direccion = inmueble.Direccion;
        this.Ambientes = inmueble.Ambientes;
        this.Coordenada_Lat = inmueble.Coordenada_Lat;
        this.Coordenada_Lon = inmueble.Coordenada_Lon;
        this.Precio = inmueble.Precio;
        this.Activo = inmueble.Activo;
        this.Tipo_Uso = inmueble.Tipo_Uso;
        this.Propietario = inmueble.Propietario;
        this.Tipo = inmueble.Tipo;
        this.Contratos = contratos;
    }
    public int Id { get; set; }
    public string Direccion { get; set; } = "";
    public TipoInmuebleUso? Tipo_Uso { get; set; }
    public TipoInmueble? Tipo { get; set; }
    public int Ambientes { get; set; }
    public string Coordenada_Lat { get; set; } = "-33.30158732843527";
    public string Coordenada_Lon { get; set; } = "-66.33797013891889";
    public decimal Precio { get; set; }
    public bool Activo { get; set; }
    public Propietario? Propietario { get; set; }
    public List<Contrato> Contratos { get; set; } = new List<Contrato>();
}

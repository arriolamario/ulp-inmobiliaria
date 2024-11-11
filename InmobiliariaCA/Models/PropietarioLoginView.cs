using System.ComponentModel.DataAnnotations;

namespace InmobiliariaCA.Models
{
    public class PropietarioLoginView
    {
        [DataType(DataType.EmailAddress)]
		public string Usuario { get; set; } = "";
		[DataType(DataType.Password)]
		public string Clave { get; set; } = "";
    }
}
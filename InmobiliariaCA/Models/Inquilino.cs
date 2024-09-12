namespace InmobiliariaCA.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Inquilino {
        public int Id { get; set; }

        [Required(ErrorMessage = "El documento es obligatorio.")]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El documento debe tener 7 u 8 dígitos.")]	
        public string Dni { get; set; } = "";

        [Required(ErrorMessage = "El Nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El Nombre no puede tener más de 50 caracteres.")]
        public string Nombre { get; set; } = "";

        [Required(ErrorMessage = "El Apellido es obligatorio.")]
        [StringLength(50, ErrorMessage = "El Apellido no puede tener más de 50 caracteres.")]
        public string Apellido { get; set; } = "";

        [Required(ErrorMessage = "El Teléfono es obligatorio.")]
        [RegularExpression(@"^\d{2,4}$", ErrorMessage = "El área debe contener entre 2 y 4 dígitos numéricos.")]
        public string TelefonoArea { get; set; } = "";

        [Required(ErrorMessage = "El Teléfono es obligatorio.")]
        [RegularExpression(@"^\d{6,10}$", ErrorMessage = "El número de teléfono debe contener entre 6 y 10 dígitos numéricos.")]
        public string TelefonoNumero { get; set; } = "";

        public string Telefono => $"{TelefonoArea}-{TelefonoNumero}";
        [Required(ErrorMessage = "Campo obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del Email no es válido.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "El Direccion es obligatorio.")]
        [StringLength(100, ErrorMessage = "La Dirección no puede tener más de 100 caracteres.")]
        public string Direccion { get; set; } = "";

        public DateTime Fecha_Creacion { get; set; }
        public DateTime Fecha_Actualizacion { get; set; }
        [NotMapped]
        public string NombreCompletoDNI => $"{Dni} - {Nombre} {Apellido}";
  
}

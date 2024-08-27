using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaCA.Models
{
    public class Contrato {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Inmueble")]
        public int IdInmueble { get; set; }

        [Required]
        [ForeignKey("Inquilino")]
        public int IdInquilino { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaDesde { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaHasta { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal MontoAlquiler { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaFinalizacionAnticipada { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal? Multa { get; set; }

        [Required]
        public bool Estado { get; set; } = true;

        [Required]
        [ForeignKey("UsuarioCreacion")]
        public int IdUsuarioCreacion { get; set; }

        [ForeignKey("UsuarioFinalizacion")]
        public int? IdUsuarioFinalizacion { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
       
        public virtual Inmueble Inmueble { get; set; }
        public virtual Inquilino Inquilino { get; set; }
        public virtual Usuario UsuarioCreacion { get; set; }
        public virtual Usuario UsuarioFinalizacion { get; set; }
      
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaCA.Models;
public class Contrato {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Inmueble")]
        public int Id_Inmueble { get; set; }

        [Required]
        [ForeignKey("Inquilino")]
        public int Id_Inquilino { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha_Desde { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha_Hasta { get; set; }

        [Required]
        [Column("monto_alquiler", TypeName = "decimal(10, 2)")]
        public decimal Monto_Alquiler { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Fecha_Finalizacion_Anticipada { get; set; }

        [Column("multa", TypeName = "decimal(10, 2)")]
        public decimal? Multa { get; set; }

        [Required]
        [Column("estado")]
        public bool Estado { get; set; } = true;

        [Required]
        [ForeignKey("UsuarioCreacion")]
        public int Id_Usuario_Creacion { get; set; }

        [ForeignKey("UsuarioFinalizacion")]
        public int? Id_Usuario_Finalizacion { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Fecha_Creacion { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime Fecha_Actualizacion { get; set; } = DateTime.Now;
       
        public virtual Inmueble Inmueble { get; set; }
        public virtual Inquilino Inquilino { get; set; }
        public virtual Usuario Usuario_Creacion { get; set; }
        public virtual Usuario Usuario_Finalizacion { get; set; }
    
}

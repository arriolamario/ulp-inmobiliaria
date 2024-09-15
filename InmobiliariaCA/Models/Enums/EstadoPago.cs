using System.ComponentModel.DataAnnotations;

namespace InmobiliariaCA.Models;

public enum EstadoPago
{
    [Display(Name = "Pagado")]
    Pagado,

    [Display(Name = "Anulado")]
    Anulado,

    [Display(Name = "Pendiente")]
    Pendiente,

}
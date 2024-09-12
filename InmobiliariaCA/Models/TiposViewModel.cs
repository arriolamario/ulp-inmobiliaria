namespace InmobiliariaCA.Models;
using System;
using System.Collections.Generic;

public class TiposViewModel
{
    public List<TipoInmueble> TiposInmuebles { get; set; } = new List<TipoInmueble>();
    public List<TipoInmuebleUso> TiposInmueblesUsos { get; set; } = new List<TipoInmuebleUso>();
}
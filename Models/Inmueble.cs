using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InmobiliariaEfler.Models;

public class Inmueble
{
    public int Id { get; set; }
    public String Direccion { get; set; }
    public int Ambientes { get; set; }
    public decimal Superficie { get; set; }
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
    public decimal Precio { get; set; }
    public String Uso { get; set; }
    public Boolean Alquilado { get; set; }
    public Boolean OfertaActiva { get; set; }

    [Display(Name = "Dueño")]
    public int IdPropietario { get; set; }
    [ForeignKey(nameof(IdPropietario))]
    public Propietario Duenio { get; set; }

    [Display(Name = "TipoInmueble")]
    public int idTipo { get; set; }
    [ForeignKey(nameof(idTipo))]
    public TipoInmueble TipoInmueble { get; set; }

}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InmobiliariaEfler.Models;

public class Inmueble
{
    public int Id { get; set; }
    [Display(Name = "Dirección")]
    public String Direccion { get; set; }
    public int Ambientes { get; set; }
    public decimal Superficie { get; set; }
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
    public decimal Precio { get; set; }
    public decimal Coordenadas { get; set; }

    [Display(Name = "Disponible")]
    public Boolean OfertaActiva { get; set; }

    [Display(Name = "Dueño")]
    public int IdPropietario { get; set; }
    [ForeignKey(nameof(IdPropietario))]
    public Propietario Duenio { get; set; }

    [Display(Name = "Tipo Inmueble")]
    public int IdTipo { get; set; }
    [ForeignKey(nameof(IdTipo))]
    public TipoInmueble TipoInmueble { get; set; }

}
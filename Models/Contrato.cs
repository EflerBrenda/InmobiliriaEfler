using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InmobiliariaEfler.Models;

public class Contrato
{
    public int Id { get; set; }
    public DateTime FechaDesde { get; set; }
    public DateTime FechaHasta { get; set; }
    public decimal MontoAlquiler { get; set; }
    public Boolean PagoAlDia { get; set; }

    [Display(Name = "Inmueble")]
    public int IdInmueble { get; set; }
    [ForeignKey(nameof(IdInmueble))]
    public Inmueble Inmueble { get; set; }

    [Display(Name = "Inquilino")]
    public int IdInquilino { get; set; }
    [ForeignKey(nameof(IdInquilino))]
    public Inquilino Inquilino { get; set; }


}
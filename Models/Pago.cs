using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace InmobiliariaEfler.Models;

public class Pago
{
    public int Id { get; set; }
    public String NumeroPago { get; set; }
    public DateTime FechaPago { get; set; }
    public decimal Importe { get; set; }

    [Display(Name = "Contrato")]
    public int IdContrato { get; set; }
    [ForeignKey(nameof(IdContrato))]
    public Contrato Contrato { get; set; }


}
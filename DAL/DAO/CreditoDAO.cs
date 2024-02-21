using DAL.DAO;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class CreditoDAO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long id_credito { get; set; }
    public decimal cantidad_prestamo { get; set; }
    public decimal cuota_mensual { get; set; }
    public string estado_prestamo { get; set; }
    public DateTime? fch_final { get; set; }
    public DateTime? fch_inicio { get; set; }
    public decimal tasa_interes { get; set; }
    public string tipo_prestamo { get; set; }

    // Cambia el tipo de datos de la propiedad id_cuenta a long
    [ForeignKey("id_cuenta")]
    public long id_cuenta { get; set; }

    public virtual CuentaDAO Cuenta { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    [Table("Credito", Schema = "schemabody")]
    public class CreditoDAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_credito { get; set; }
        public decimal cantidad_prestamo { get; set; }
        public decimal cuota_mensual { get; set; }
        public string estado_prestamo { get; set; }
        public DateTime fch_final { get; set; }
        public DateTime fch_inicio { get; set; }
        public decimal tasa_interes { get; set; }
        public string tipo_prestamo { get; set; }
        [ForeignKey("id_cuenta")]
        public virtual CuentaDAO Cuenta { get; set; }

    }
}

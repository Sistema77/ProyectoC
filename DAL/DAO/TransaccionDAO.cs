using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    public class TransaccionDAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_transaccion { get; set; }
        public DateTime fch_hora { get; set; }
        public decimal cantidad_dinero { get; set; }
        [ForeignKey("cuenta_envia")]
        public virtual CuentaDAO CuentaEnvia { get; set; }
        [ForeignKey("cuenta_recibe")]
        public virtual CuentaDAO CuentaRecibe { get; set; }

    }
}

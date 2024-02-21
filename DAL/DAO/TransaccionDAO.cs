using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    [Table("Transaccion", Schema = "schemabody")]
    public class TransaccionDAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id_transaccion { get; set; }
        public DateTime? fch_hora { get; set; }
        public decimal cantidad_dinero { get; set; }
        public string TipoTransacion {  get; set; }
        public long NumeroTrasaccion {  get; set; }

        [ForeignKey("id_cuenta")]
        public long id_cuenta { get; set; }
        public virtual CuentaDAO Cuenta { get; set; }

    }
}

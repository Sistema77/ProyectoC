using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    [Table("Usuario", Schema = "schemabody")]
    public class CuentaDAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_cuenta { get; set; }
        public bool con_nomina { get; set; }
        public DateTime fch_apertura { get; set; }
        public string numero_cuenta { get; set; }
        public decimal saldo { get; set; }

        [ForeignKey("id_usuario")]
        public virtual UsuarioDAO Usuario { get; set; }
        public virtual ICollection<CreditoDAO> Creditos { get; set; }
        public virtual ICollection<TransaccionDAO> Transacciones { get; set; }

    }
}

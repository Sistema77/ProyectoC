using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DAL.DAO
{
    [Table("Cuenta", Schema = "schemabody")]
    public class CuentaDAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id_cuenta { get; set; }
        public bool con_nomina { get; set; }
        public DateTime? fch_apertura { get; set; }
        public string numero_cuenta { get; set; }
        public decimal saldo { get; set; }

        [ForeignKey("id_usuario")]
        public virtual UsuarioDAO Usuario { get; set; }
        public virtual ICollection<CreditoDAO> Creditos { get; set; }
        public virtual ICollection<TransaccionDAO> Transacciones { get; set; }

        // Cambiar el tipo de datos de la propiedad Usuario
        // para que coincida con el tipo de datos de la clave primaria en UsuarioDAO
        [ForeignKey("id_usuario")]
        public long id_usuario { get; set; }
    }
}

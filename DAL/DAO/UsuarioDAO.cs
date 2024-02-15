using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DAO
{
    [Table("Usuario", Schema = "schemausuario")]
    public class UsuarioDAO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_usuario { get; set; }
        public string dni { get; set; }
        public string email { get; set; }
        public DateTime? expiracion_token { get; set; }
        public DateTime? fch_alta { get; set; }
        public byte[]? foto { get; set; }
        public string last_name { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string? tipo_usuario { get; set; }
        public string tlf { get; set; }
        public string? token { get; set; }
        public bool cuentaConfirmada { get; set; }
        public virtual ICollection<CuentaDAO> Cuentas { get; set; }
    }
}

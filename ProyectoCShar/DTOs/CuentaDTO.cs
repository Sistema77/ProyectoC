namespace ProyectoCShar.DTOs
{
    public class CuentaDTO
    {
        public int id_cuenta { get; set; }
        public bool con_nomina { get; set; }
        public DateTime fch_apertura { get; set; }
        public string numero_cuenta { get; set; }
        public decimal saldo { get; set; }
    }
}

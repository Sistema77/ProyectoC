namespace ProyectoCShar.DTOs
{
    public class TransaccionDTO
    {
        public int id_transaccion { get; set; }
        public DateTime fch_hora { get; set; }
        public decimal cantidad_dinero { get; set; }
        public string TipoTransacion { get; set; }
        public long NumeroTrasaccion { get; set; }
    }
}

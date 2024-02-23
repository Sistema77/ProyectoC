namespace ProyectoCShar.DTOs
{
    public class Class
    {
        public long id_credito { get; set; }
        public decimal cantidad_prestamo { get; set; }
        public decimal cuota_mensual { get; set; }
        public string estado_prestamo { get; set; }
        public DateTime fch_final { get; set; }
        public DateTime fch_inicio { get; set; }
        public decimal tasa_interes { get; set; }
        public string tipo_prestamo { get; set; }
    }
}

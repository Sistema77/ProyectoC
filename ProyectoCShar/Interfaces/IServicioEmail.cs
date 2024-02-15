namespace ProyectoCShar.Interfaces
{
    public interface IServicioEmail
    {
        public void enviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token);

        public void enviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token);
    }
}

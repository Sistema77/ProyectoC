namespace ProyectoCShar.Interfaces
{
    public interface IServicioEmail
    {
        // Enviar el Email de Recuperacion de Contraseña
        public void enviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token);

        // Enviar el Email de la Confirmacion de Cuenta
        public void enviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token);
    }
}

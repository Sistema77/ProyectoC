using Microsoft.Extensions.Logging;
using ProyectoCShar.Interfaces;
using ProyectoCShar.Util;
using System.Net.Mail;

namespace ProyectoCShar.Servicio
{
    public class ServicioEmailImpl : IServicioEmail
    {

        void IServicioEmail.enviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                // Declaro la URL
                string urlDominio = "http://localhost:5071";

                string EmailOrigen = "parafirebaseestudiar@gmail.com";

                string urlDeRecuperacion = String.Format("{0}/auth/confirmar-cuenta/?token={1}", urlDominio, token);

                // Trasformamos el texto del Email en un HTML
                string directorioProyecto = System.IO.Directory.GetCurrentDirectory();
                string rutaArchivo = System.IO.Path.Combine(directorioProyecto, "Plantilla/ConfirmarCorreo.html");
                string htmlContent = System.IO.File.ReadAllText(rutaArchivo);

                // Asignamos el nombre de usuario que tendrá el cuerpo del mail y el URL de recuperación con el token al HTML.
                htmlContent = String.Format(htmlContent, nombreUsuario, urlDeRecuperacion);

                MailMessage mensajeDelCorreo = new MailMessage(EmailOrigen, emailDestino, "CONFIRMAR EMAIL RoBank", htmlContent);

                mensajeDelCorreo.IsBodyHtml = true;

                SmtpClient smtpCliente = new SmtpClient("smtp.gmail.com");
                smtpCliente.EnableSsl = true;
                smtpCliente.UseDefaultCredentials = false;
                smtpCliente.Port = 587;
                smtpCliente.Credentials = new System.Net.NetworkCredential(EmailOrigen, "kctpbonxybeipmqr");

                smtpCliente.Send(mensajeDelCorreo);

                smtpCliente.Dispose();

                Logs.log("Mensaje enviado");

            }
            catch (IOException ioe)
            {
                Logs.log("A ocurrido un error de entrada o salida");
            }
            catch (SmtpException se)
            {
                Logs.log("A avida un error al envio del mensaje SMTP");
            }

        }

        void IServicioEmail.enviarEmailRecuperacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {

                string urlDominio = "http://localhost:5071";

                string EmailOrigen = "parafirebaseestudiar@gmail.com";
                //Se crea la URL de recuperación con el token que se enviará al mail del user.
                string urlDeRecuperacion = String.Format("{0}/auth/recuperar/?token={1}", urlDominio, token);

                // Trasformamos el texto del Email en un HTML
                string directorioProyecto = System.IO.Directory.GetCurrentDirectory();
                string rutaArchivo = System.IO.Path.Combine(directorioProyecto, "Plantilla/RecuperacionPassword.html");
                string htmlContent = System.IO.File.ReadAllText(rutaArchivo);
                //Asignamos el nombre de usuario que tendrá el cuerpo del mail y el URL de recuperación con el token al HTML.
                htmlContent = String.Format(htmlContent, nombreUsuario, urlDeRecuperacion);

                MailMessage mensajeDelCorreo = new MailMessage(EmailOrigen, emailDestino, "RESTABLECER CONTRASEÑA RoBank", htmlContent);

                mensajeDelCorreo.IsBodyHtml = true;

                SmtpClient smtpCliente = new SmtpClient("smtp.gmail.com");
                smtpCliente.EnableSsl = true;
                smtpCliente.UseDefaultCredentials = false;
                smtpCliente.Port = 587;
                smtpCliente.Credentials = new System.Net.NetworkCredential(EmailOrigen, "kctpbonxybeipmqr");

                smtpCliente.Send(mensajeDelCorreo);

                smtpCliente.Dispose();

                Logs.log("Mensaje enviado");

            }
            catch (IOException ioe)
            {
                Logs.log("A ocurrido un error de entrada o salida");
            }
            catch (SmtpException se)
            {
                Logs.log("A avida un error al envio del mensaje SMTP");
            }

        }
    }
}
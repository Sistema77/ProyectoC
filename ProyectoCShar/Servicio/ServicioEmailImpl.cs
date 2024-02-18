﻿using ProyectoCShar.Interfaces;
using System.Net.Mail;

namespace ProyectoCShar.Servicio
{
    public class ServicioEmailImpl : IServicioEmail
    {
        void IServicioEmail.enviarEmailConfirmacion(string emailDestino, string nombreUsuario, string token)
        {
            try
            {
                ////////////////////////////////
                Console.WriteLine("Envia Correo");
                string urlDominio = "http://localhost:5071";

                string EmailOrigen = "parafirebaseestudiar@gmail.com";

                string urlDeRecuperacion = String.Format("{0}/auth/confirmar-cuenta/?token={1}", urlDominio, token);

                //Hacemos que el texto del email sea un archivo html que se encuentra en la carpeta Plantilla.
                string directorioProyecto = System.IO.Directory.GetCurrentDirectory();
                string rutaArchivo = System.IO.Path.Combine(directorioProyecto, "Plantilla/ConfirmarCorreo.html");
                string htmlContent = System.IO.File.ReadAllText(rutaArchivo);
                //Asignamos el nombre de usuario que tendrá el cuerpo del mail y el URL de recuperación con el token al HTML.
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



            }
            catch (IOException ioe)
            {

            }
            catch (SmtpException se)
            {

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

                //Hacemos que el texto del email sea un archivo html que se encuentra en la carpeta Plantilla.
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



            }
            catch (IOException ioe)
            {

            }
            catch (SmtpException se)
            {

            }

        }
    }
}
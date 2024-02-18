using Microsoft.AspNetCore.Mvc;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;

namespace ProyectoCShar.Controllers
{
    public class RecuperarPasswordController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;

        public RecuperarPasswordController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [HttpGet]
        [Route("/auth/recuperar")]
        public IActionResult MostrarVistaRecuperar([FromQuery(Name = "token")] string token)
        {

            try
            {

                UsuarioDTO usuario = _usuarioServicio.obtenerUsuarioPorToken(token);

                if (usuario != null)
                {
                    ViewData["UsuarioDTO"] = usuario;
                }
                else
                {
                    ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido o el usuario no se ha encontrado";
                   
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }
                
                return View("~/Views/Home/recuperar.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
               
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }

        [HttpPost]
        [Route("/auth/recuperar")]
        public IActionResult ProcesarRecuperacionContraseña(UsuarioDTO usuarioDTO)
        {
            try
            {

                UsuarioDTO usuarioExistente = _usuarioServicio.obtenerUsuarioPorToken(usuarioDTO.token);

                if (usuarioExistente == null)
                {
                    ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido";
                    
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }

                if (usuarioExistente.expiracion_token.HasValue && usuarioExistente.expiracion_token.Value < DateTime.Now)
                {
                    ViewData["MensajeErrorTokenExpirado"] = "El enlace de recuperación ha expirado";
                    
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }

                bool modificadaPassword = _usuarioServicio.modificarContraseñaConToken(usuarioDTO);

                if (modificadaPassword)
                {
                    ViewData["ContraseñaModificadaExito"] = "Contraseña modificada OK";
                    
                    return View("~/Views/Home/login.cshtml");
                }
                else
                {
                    ViewData["ContraseñaModificadaError"] = "Error al cambiar de contraseña";
                   
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }

        [HttpGet]
        [Route("/auth/solicitar-recuperacion")]
        public IActionResult MostrarVistaIniciarRecuperacion()
        {
            try
            {
               

                UsuarioDTO usuarioDTO = new UsuarioDTO();
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml", usuarioDTO);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }

        /// <summary>
        /// Método HTTP POST para procesar el inicio del proceso de recuperación de contraseña.
        /// </summary>
        /// <param name="usuarioDTO">DTO del usuario con el email para iniciar la recuperación</param>
        /// <returns>La vista correspondiente según el resultado del inicio de recuperación</returns>
        [HttpPost]
        [Route("/auth/iniciar-recuperacion")]
        public IActionResult ProcesarInicioRecuperacion([Bind("email")] UsuarioDTO usuarioDTO)
        {
            try
            {
               

                bool envioConExito = _usuarioServicio.iniciarProcesoRecuperacion(usuarioDTO.email);

                if (envioConExito)
                {
                    ViewData["MensajeExitoMail"] = "Proceso de recuperación OK";
                    
                    return View("~/Views/Home/login.cshtml");
                }
                else
                {
                    ViewData["MensajeErrorMail"] = "No se inició el proceso de recuperación, cuenta de correo electrónico no encontrada.";
                }
                
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }
    }
}

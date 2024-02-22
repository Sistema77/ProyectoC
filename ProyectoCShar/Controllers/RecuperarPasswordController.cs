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

        // Constructor de recuperación
        [HttpGet]
        [Route("/auth/recuperar")]
        public IActionResult MostrarVistaRecuperar([FromQuery(Name = "token")] string token)
        {

            try
            {
               // _logger.LogInformation("Error al procesar a recuperar Contraseña");

                // Obtiene el usuario por token
                UsuarioDTO usuario = _usuarioServicio.obtenerUsuarioPorToken(token);

                if (usuario != null)
                {
                    // Si el usuario es válido, lo pasa a la vista
                    ViewData["UsuarioDTO"] = usuario;
                }
                else
                {
                    // Si el usuario no es válido, registra un mensaje de error y redirige a una vista
                    ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido o el usuario no se ha encontrado";

                  //  _logger.LogError("El enlace de recuperación no es válido o el usuario no se ha encontrado");

                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }
                
                return View("~/Views/Home/recuperar.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";

                //_logger.LogError("Error al procesar la solicitud");

                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }

        // Controlador que realiza la Recuperacion
        [HttpPost]
        [Route("/auth/recuperar")]
        public IActionResult ProcesarRecuperacionContraseña(UsuarioDTO usuarioDTO)
        {
            try
            {
                // Obtiene el usuario por token
                UsuarioDTO usuarioExistente = _usuarioServicio.obtenerUsuarioPorToken(usuarioDTO.token);

                if (usuarioExistente == null)
                {

                    ViewData["MensajeErrorTokenValidez"] = "El enlace de recuperación no es válido";

                    //_logger.LogError("El enlace de recuperación no es válido");
                    
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }

                if (usuarioExistente.expiracion_token.HasValue && usuarioExistente.expiracion_token.Value < DateTime.Now)
                {
                    ViewData["MensajeErrorTokenExpirado"] = "El enlace de recuperación ha expirado";

                    //_logger.LogError("El enlace de recuperación ha expirado");

                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }

                // Modifica la contraseña del usuario
                bool modificadaPassword = _usuarioServicio.modificarContraseñaConToken(usuarioDTO);

                if (modificadaPassword)
                {
                    ViewData["ContraseñaModificadaExito"] = "Contraseña modificada OK";

                    //_logger.LogInformation("Contraseña modificada OK");
                    
                    return View("~/Views/Home/login.cshtml");
                }
                else
                {
                    ViewData["ContraseñaModificadaError"] = "Error al cambiar de contraseña";

                    //_logger.LogError("Error al cambiar de contraseña");
                   
                    return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";

               // _logger.LogError("Error al procesar la solicitud");

                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }

        // Controlador que maneja la solicitud de la Recuperación
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
                
                //_logger.LogError("Error al procesar la solicitud");
                
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }

        // Controlador que realiza la Recuperacion
        [HttpPost]
        [Route("/auth/iniciar-recuperacion")]
        public IActionResult ProcesarInicioRecuperacion([Bind("email")] UsuarioDTO usuarioDTO)
        {
            try
            {
                // Iniciar el proceso de recuperación de contraseña
                bool envioConExito = _usuarioServicio.iniciarProcesoRecuperacion(usuarioDTO.email);

                if (envioConExito)
                {
                    ViewData["MensajeExitoMail"] = "Proceso de recuperación OK";

                    //_logger.LogInformation("Proceso de recuperación OK");

                    return View("~/Views/Home/login.cshtml");
                }
                else
                {
                    ViewData["MensajeErrorMail"] = "No se inició el proceso de recuperación, cuenta de correo electrónico no encontrada.";
                    
                    //_logger.LogError("No se inició el proceso de recuperación, cuenta de correo electrónico no encontrada.");
                }
                
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                
                //_logger.LogError("Error al procesar la solicitud");
                
                return View("~/Views/Home/solicitarRecuperacionPassword.cshtml");
            }
        }
    }
}

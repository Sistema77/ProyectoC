using Microsoft.AspNetCore.Mvc;
using ProyectoCShar.DTOs;

namespace ProyectoCShar.Controllers
{
    public class LoginRegitroController : Controller
    {
        [HttpGet]
        [Route("/auth/login")]
        public IActionResult Login()
        {
            try
            {
                UsuarioDTO usuarioDTO = new UsuarioDTO();
                return View("~/Views/Home/login.cshtml", usuarioDTO);

            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/login.cshtml");
            }
        }

        [HttpGet]
        [Route("/auth/crear-cuenta")]
        public IActionResult RegistrarGet()
        {
            try
            {
                UsuarioDTO usuarioDTO = new UsuarioDTO();
                return View("~/Views/Home/registro.cshtml", usuarioDTO);

            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                
                return View("~/Views/Home/registro.cshtml");
            }
        }

        [HttpPost]
        [Route("/auth/crear-cuenta")]
        public IActionResult RegistrarPost(UsuarioDTO usuarioDTO)
        {
            try
            {

                UsuarioDTO nuevoUsuario = usuarioServicio.registrarUsuario(usuarioDTO);

                if (nuevoUsuario.EmailUsuario == "EmailNoConfirmado")
                {
                    ViewData["EmailNoConfirmado"] = "Ya existe un usuario registrado con ese email pero con la cuenta sin verificar";
                    EscribirLog.escribirEnFicheroLog("[INFO] Saliendo del método RegistrarGet() de la clase RegistroController. " + ViewData["EmailNoConfirmado"]);
                    return View("~/Views/Home/login.cshtml");

                }
                else if (nuevoUsuario.EmailUsuario == "EmailRepetido")
                {
                    ViewData["EmailRepetido"] = "Ya existe un usuario con ese email registrado en el sistema";
                   
                    return View("~/Views/Home/registro.cshtml");
                }
                else
                {
                    ViewData["MensajeRegistroExitoso"] = "Registro del nuevo usuario OK";
                    return View("~/Views/Home/login.cshtml");
                }


            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/registro.cshtml");
            }
        }
    }
}

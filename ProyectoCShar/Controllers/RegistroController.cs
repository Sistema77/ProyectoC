using Microsoft.AspNetCore.Mvc;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;

namespace ProyectoCShar.Controllers
{
    public class RegistroController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;
            
        public RegistroController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
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

            Console.WriteLine("UUSUARIO: " + usuarioDTO.email);

            try
            {
                Console.WriteLine(usuarioDTO);

                UsuarioDTO nuevoUsuario = _usuarioServicio.registrarUsuario(usuarioDTO);

                if (nuevoUsuario.email == "EmailNoConfirmado")
                {
                    ViewData["EmailNoConfirmado"] = "Ya existe un usuario registrado con ese email pero con la cuenta sin verificar";

                    return View("~/Views/Home/login.cshtml");

                }
                else if (nuevoUsuario.email == "EmailRepetido")
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

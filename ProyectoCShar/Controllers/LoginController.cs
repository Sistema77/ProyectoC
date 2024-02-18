using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoCShar.Controllers
{
    public class LoginController : Controller
    {

        private readonly IUsuarioServicio _usuarioServicio;

        public LoginController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }


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

        [HttpPost]
        [Route("/auth/iniciar-sesion")]
        public async Task<IActionResult> ProcesarInicioSesionAsync(UsuarioDTO usuarioDTO)
        {
            try
            {
                bool credencialesValidas = _usuarioServicio.verificarCredenciales(usuarioDTO.email, usuarioDTO.password);

                if (credencialesValidas)
                {
                    UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(usuarioDTO.email);

                    // Al hacer login correctamente se crea una identidad de reclamaciones (claims identity) con información del usuario 
                    // y su rol, de esta manera se controla que solo los admin puedan acceder a la administracion de usuarios
                    // y se mantiene esa info del usuario autenticado durante toda la sesión en una cookie.
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuarioDTO.email),
                    };
                    if (!string.IsNullOrEmpty(u.tipo_usuario))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, u.tipo_usuario));
                    }

                    var identidadDeReclamaciones = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // establece una cookie en el navegador con los datos del usuario antes mencionados y se mantiene en el contexto.
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identidadDeReclamaciones));

                    //return RedirectToAction("Dashboard", "Login");

                    UsuarioDTO user = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
                    ViewBag.UsuarioDTO = user;

                    return View("~/Views/Home/dashboard.cshtml");

                }
                else
                {
                    ViewData["MensajeErrorInicioSesion"] = "Credenciales inválidas o cuenta no confirmada. Inténtelo de nuevo.";
                    
                    return View("~/Views/Home/login.cshtml");
                }
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
               
                return View("~/Views/Home/login.cshtml");
            }
        }

        [HttpGet]
        [Route("/auth/confirmar-cuenta")]
        public IActionResult ConfirmarCuenta([FromQuery] string token)
        {
            try
            {
                Console.WriteLine("Entra en Confirmar Cuenta");

                bool confirmacionExitosa = _usuarioServicio.confirmarCuenta(token);

                if (confirmacionExitosa)
                {
                    ViewData["CuentaVerificada"] = "La dirección de correo ha sido confirmada correctamente";
                }
                else
                {
                    ViewData["yaEstabaVerificada"] = "El usuario ya estaba registrado y verificado";
                }

               
                return View("~/Views/Home/login.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
               
                return View("~/Views/Home/login.cshtml");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("/privada/dashboard")]
        public IActionResult Dashboard()
        {
            Console.WriteLine("22222222222222222222222222222222222");
            Console.WriteLine(User.Identity.Name);
            UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
            ViewBag.UsuarioDTO = u;
            
            return View("~/Views/Home/dashboard.cshtml");
        }
    }
}

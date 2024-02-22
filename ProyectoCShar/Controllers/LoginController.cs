using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System;
using ProyectoCShar.Util;

namespace ProyectoCShar.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;
       

        public LoginController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        // Controlador para mostrar el formulario de inicio de sesión
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
                Logs.log("Error al procesar la solicitud");

                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/login.cshtml");
            }
        }

        // Controlador para procesar el inicio de sesión
        [HttpPost]
        [Route("/auth/iniciar-sesion")]
        public async Task<IActionResult> ProcesarInicioSesionAsync(UsuarioDTO usuarioDTO)
        {
            try
            {
                // Verificar si las credenciales son válidas
                bool credencialesValidas = _usuarioServicio.verificarCredenciales(usuarioDTO.email, usuarioDTO.password);

                if (credencialesValidas)
                {
                    UsuarioDTO u = _usuarioServicio.obtenerUsuarioPorEmail(usuarioDTO.email);

                    // Crear reclamaciones (claims) para el usuario autenticado
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuarioDTO.email),
                    };

                    if (!string.IsNullOrEmpty(u.tipo_usuario))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, u.tipo_usuario));
                    }

                    // Crear identidad de reclamaciones
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Establecer la cookie de autenticación en el navegador
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    UsuarioDTO user = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);
                    ViewBag.UsuarioDTO = user;

                    Logs.log("Usuario " + User.Identity.Name + " Autentificado");
                    
                    // Redirigir al dashboard
                    return View("~/Views/Home/dashboard.cshtml");
                }
                else
                {
                    // Si las credenciales no son válidas, mostrar mensaje de error
                    ViewData["MensajeErrorInicioSesion"] = "Credenciales inválidas o cuenta no confirmada. Inténtelo de nuevo.";

                    Logs.log("Error, Credenciales inválidas o cuenta no confirmada");

                    return View("~/Views/Home/login.cshtml");
                }
            }
            catch (Exception e)
            {
                
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";

                Logs.log("Error al procesar la solicitud");
               
                return View("~/Views/Home/login.cshtml");
            }
        }

        // Controlador para confirmar la cuenta
        [HttpGet]
        [Route("/auth/confirmar-cuenta")]
        public IActionResult ConfirmarCuenta([FromQuery] string token)
        {
            try
            {
                
                Console.WriteLine("Entra en Confirmar Cuenta");

                // Confirmar la cuenta usando el token
                bool confirmacionExitosa = _usuarioServicio.confirmarCuenta(token);

                if (confirmacionExitosa)
                {
                    ViewData["CuentaVerificada"] = "La dirección de correo ha sido confirmada correctamente";
                    Logs.log("La dirección de correo ha sido confirmada correctamente");
                }
                else
                {
                    ViewData["yaEstabaVerificada"] = "El usuario ya estaba registrado y verificado";
                    Logs.log("El usuario ya estaba registrado y verificado");
                }

                return View("~/Views/Home/login.cshtml");
            }
            catch (Exception e)
            {   
                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";

                Logs.log("Error al procesar la solicitud");
                
                return View("~/Views/Home/login.cshtml");
            }
        }
    }
}

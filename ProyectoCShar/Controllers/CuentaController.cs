using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;
using ProyectoCShar.Util;

namespace ProyectoCShar.Controllers
{
    public class CuentaController : Controller
    {
        private readonly ICuentaServicio _cuentaServicio;
        private readonly IUsuarioServicio _usuarioServicio;

        public CuentaController(ICuentaServicio cuentaServicio, IUsuarioServicio usuarioServicio)
        {
            _cuentaServicio = cuentaServicio;
            _usuarioServicio = usuarioServicio;
        }

        // Metodo Para redirigir
        [HttpGet]
        [Route("/privada/cuenta")]
        public IActionResult cuentas()
        {
            try
            {
                UsuarioDTO usuarioDTO = new UsuarioDTO();
                return View("~/Views/Home/cuentas.cshtml");
            }
            catch (Exception e)
            {
                Logs.log("Error al procesar la solicitud");

                ViewData["error"] = "Error al procesar la solicitud. Por favor, inténtelo de nuevo.";
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        // Controlador para obtener la lista de cuentas
        [Authorize(Roles = "ROLE_USER")]
        [HttpGet]
        [Route("/privada/lista-cuenta")]
        public IActionResult ListaCuenta(string busquedaCuenta)
        {
            try
            {
                Logs.log("Inicio de la obtención de la lista de cuenta");

                List<CuentaDTO> cuenta = new List<CuentaDTO>();
                
                ViewBag.Cuentas = _cuentaServicio.obtenerTodosLasCuentas();
                

                // Pasa a la vista la foto del usuario
                ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);

                return View("~/Views/Home/listascuentas.cshtml");
            }
            catch (Exception e)
            {
                Logs.log("Error al obtener la lista de usuarios" + e);
                ViewData["error"] = "Ocurrió un error al obtener la lista de usuarios";

                // Pasa a la vista la foto del usuario
                ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);
                return View("~/Views/Home/dashboard.cshtml");
            }
        }


        // Controlador para obtener la lista de cuentas
        [Authorize(Roles = "ROLE_USER")]
        [HttpGet]
        [Route("/privada/crear-cuenta")]
        public IActionResult CrearCuenta(string busquedaCuenta)
        {
            try
            {
                Logs.log("Inicio de la creación de cuenta");

                _cuentaServicio.registrarCuenta(User.Identity.Name);

                // Pasa a la vista la foto del usuario
                ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);

                return View("~/Views/Home/listascuentas.cshtml");
            }
            catch (Exception e)
            {
                Logs.log("Error al obtener la lista de usuarios" + e);
                ViewData["error"] = "Ocurrió un error al obtener la lista de usuarios";

                // Pasa a la vista la foto del usuario
                ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);
                return View("~/Views/Home/dashboard.cshtml");
            }
        }
    }
}

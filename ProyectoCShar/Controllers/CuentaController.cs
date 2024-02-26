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
        [Authorize(Roles = "ROLE_USER")]
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
        public IActionResult ListaCuenta()
        {
            try
            {
                Logs.log("Inicio de la obtención de la lista de cuenta");

                List<CuentaDTO> cuenta = new List<CuentaDTO>();
                
                UsuarioDTO usuarioPropietario = _usuarioServicio.obtenerUsuarioPorEmail(User.Identity.Name);

                ViewBag.Cuentas = _cuentaServicio.obtenerTodosLasCuentas(usuarioPropietario.id_usuario);
                

                // Pasa a la vista la foto del usuario
                ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);

                return View("~/Views/Home/listascuentas.cshtml");
            }
            catch (Exception e)
            {
                Logs.log("Error al obtener la lista de Cuenta" + e);
                ViewData["error"] = "Ocurrió un error al obtener la lista de Cuenta";

                // Pasa a la vista la foto del usuario
                ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);
                return View("~/Views/Home/dashboard.cshtml");
            }
        }


        // Controlador para obtener la lista de cuentas
        [Authorize(Roles = "ROLE_USER")]
        [HttpGet]
        [Route("/privada/crear-cuenta")]
        public IActionResult CrearCuenta()
        {
            try
            {
                Logs.log("Inicio de la creación de cuenta");

                _cuentaServicio.registrarCuenta(User.Identity.Name);

                // Pasa a la vista la foto del usuario
                ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);

                return RedirectToAction("ListaCuenta", "Cuenta");
            }
            catch (Exception e)
            {
                Logs.log("Error al obtener la lista de Cuenta" + e);
                ViewData["error"] = "Ocurrió un error al obtener la lista de Cuenta";

                // Pasa a la vista la foto del usuario
                ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        [Authorize(Roles = "ROLE_USER")]
        [HttpGet]
        [Route("/privada/eliminar-cuenta/{id}")]
        public IActionResult EliminarCuenta(long id)
        {
            try
            {
                Logs.log("Inicio Eliminación de cuenta");

                _cuentaServicio.eliminar(id);

                // Pasa a la vista la foto del usuario
                ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);

                return RedirectToAction("ListaCuenta", "Cuenta");
            }
            catch (Exception e)
            {
                Logs.log("Error al Eliminar una Cuenta " + e);
                ViewData["error"] = "Ocurrió un error al eliminar la Cuenta";

                // Pasa a la vista la foto del usuario
                ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        // Controlador para procesar el formulario de edición de usuario
        [Authorize(Roles = "ROLE_USER")]
        [HttpPost]
        [Route("/privada/editar-cuenta")]
        public IActionResult EditarCuenta(long id, string con_nomina, decimal saldo)
        {
            try
            {
                Logs.log("Inicio de procesamiento de edición del Cuenta " + id);

                // Buscar el cuenta por su id
                CuentaDTO cuentaDTO = _cuentaServicio.buscarPorId(id);
                bool nomina;

                if (con_nomina == "")
                {
                    nomina = false;
                }
                else
                {
                    nomina = true;
                }

                Console.WriteLine("Con Nomina: " + con_nomina + " y " + nomina);

                cuentaDTO.con_nomina = nomina;
                cuentaDTO.saldo = saldo;

                // Actualizar los datos de la cuenta
                _cuentaServicio.actualizarCuenta(cuentaDTO);

                ViewData["EdicionCorrecta"] = "El Usuario se ha editado correctamente";

                // Pasa a la vista la foto del usuario
                ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);
                return RedirectToAction("ListaCuenta", "Cuenta");
            }
            catch (Exception e)
            {
                Logs.log("Error al editar el usuario" + e);
                ViewData["Error"] = "Ocurrió un error al editar el usuario";
                // Pasa a la vista la foto del usuario
                ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        // Controladro para mostrar el formulario de edición de usuario
        [Authorize(Roles = "ROLE_USER")]
        [HttpGet]
        [Route("/privada/editar-cuenta/{id}")]
        public IActionResult RedirijeEditarCuenta(long id)
        {
            try
            {
                Logs.log("Inicio de edición de Cuenta " + id);

                CuentaDTO cuentaDTO = _cuentaServicio.buscarPorId(id);

                if (cuentaDTO == null)
                {
                    ViewData["usuarioNoEncontrado"] = "Ocurrió un error al obtener el usuario para editar";
                    // Pasa a la vista la foto del usuario
                    ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);
                    return View("~/Views/Home/dashboard.cshtml");
                }

                return View("~/Views/Home/editarCuenta.cshtml", cuentaDTO);
            }
            catch (Exception e)
            {
                Logs.log("Error al obtener el usuario para editar" + e);
                ViewData["error"] = "Ocurrió un error al obtener el usuario para editar";
                // Pasa a la vista la foto del usuario
                ViewBag.foto = _usuarioServicio.mostrarFoto(User.Identity.Name);
                return View("~/Views/Home/dashboard.cshtml");
            }
        }
    }
}

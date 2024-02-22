using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;

namespace ProyectoCShar.Controllers
{
    public class AdministracionController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;

        public AdministracionController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpGet]
        [Route("/privada/eliminar-usuario/{id}")]
        public IActionResult eliminarUsuario(long id)
        {
            try
            {
                UsuarioDTO usuario = _usuarioServicio.buscarPorId(id);
                List<UsuarioDTO> usuarios = _usuarioServicio.obtenerTodosLosUsuarios();

                string emailUsuarioActual = User.Identity.Name;

                int adminsRestantes = _usuarioServicio.contarUsuariosPorRol("ROLE_ADMIN");

                if (emailUsuarioActual == usuario.email)
                {
                    ViewData["noTePuedesEliminar"] = "Un administrador no puede eliminarse a sí mismo";
                    ViewBag.Usuarios = usuarios;
                   
                    return View("~/Views/Home/administracion.cshtml");
                }
                else if (User.IsInRole("ROLE_ADMIN") && adminsRestantes == 1 && usuario.tipo_usuario == "ROLE_ADMIN")
                {
                    ViewData["noSePuedeEliminar"] = "No se puede eliminar al último administrador del sistema";
                    ViewBag.Usuarios = usuarios;
                    
                    return View("~/Views/Home/administracion.cshtml");
                }
              

                _usuarioServicio.eliminar(id);
                ViewData["eliminacionCorrecta"] = "El usuario se ha eliminado correctamente";
                ViewBag.Usuarios = _usuarioServicio.obtenerTodosLosUsuarios();

                return View("~/Views/Home/administracion.cshtml");

            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al eliminar el usuario";
               
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpGet]
        [Route("/privada/administracion-usuarios")]
        public IActionResult ListaUsuarios(string busquedaUser)
        {
            try
            {
                List<UsuarioDTO> usuarios = new List<UsuarioDTO>();

                if (!string.IsNullOrEmpty(busquedaUser))
                {
                    usuarios = _usuarioServicio.buscarPorCoincidenciaEnEmail(busquedaUser);
                    if (usuarios.Count > 0)
                    {
                        ViewBag.Usuarios = usuarios;
                    }
                    else
                    {
                        ViewData["usuarioNoEncontrado"] = "No se encontraron email de usuario que contenga la palabra introducida";
                        ViewBag.Usuarios = _usuarioServicio.obtenerTodosLosUsuarios();
                    }
                }
                else
                {
                    ViewBag.Usuarios = _usuarioServicio.obtenerTodosLosUsuarios();
                }

                return View("~/Views/Home/administracion.cshtml");
            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al obtener la lista de usuarios";
               
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpGet]
        [Route("/privada/editar-usuario/{id}")]
        public IActionResult FormularioEdicion(long id)
        {
            try
            {
                UsuarioDTO usuarioDTO = _usuarioServicio.buscarPorId(id);

                if (usuarioDTO == null)
                {
                    ViewData["usuarioNoEncontrado"] = "Ocurrió un error al obtener el usuario para editar";
                    return View("~/Views/Home/administracion.cshtml");
                }

                return View("~/Views/Home/editarUsuario.cshtml", usuarioDTO);
            }
            catch (Exception e)
            {
                ViewData["error"] = "Ocurrió un error al obtener el usuario para editar";
                
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost]
        [Route("/privada/procesar-editar")]
        public IActionResult ProcesarFormularioEdicion(long id, string nombre, string apellidos, string telefono, string rol, IFormFile foto)
        {
            try
            {

                UsuarioDTO usuarioDTO = _usuarioServicio.buscarPorId(id);
                usuarioDTO.name = nombre;
                usuarioDTO.last_name = apellidos;
                usuarioDTO.tlf = telefono;

                _usuarioServicio.actualizarUsuario(usuarioDTO);

                ViewData["EdicionCorrecta"] = "El Usuario se ha editado correctamente";
                ViewBag.Usuarios = _usuarioServicio.obtenerTodosLosUsuarios();

                return View("~/Views/Home/administracion.cshtml");
            }
            catch (Exception e)
            {
                ViewData["Error"] = "Ocurrió un error al editar el usuario";
               
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

    }
}

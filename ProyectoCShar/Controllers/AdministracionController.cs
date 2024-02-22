using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Añadir este using para el Logger
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;
using ProyectoCShar.Util;
using System;
using System.Collections.Generic;

namespace ProyectoCShar.Controllers
{
    public class AdministracionController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;
       
        public AdministracionController(IUsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;

        }

        // Controlador para eliminar un usuario
        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpGet]
        [Route("/privada/eliminar-usuario/{id}")]
        public IActionResult eliminarUsuario(long id)
        {
            try
            {
                Logs.log("Inicio de Eliminación del usuario " + id);
                
                // Buscar el usuario por su id
                UsuarioDTO usuario = _usuarioServicio.buscarPorId(id);
                List<UsuarioDTO> usuarios = _usuarioServicio.obtenerTodosLosUsuarios();

                string emailUsuarioActual = User.Identity.Name;

                // Contar cuántos administradores restantes hay en el sistema
                int adminsRestantes = _usuarioServicio.contarUsuariosPorRol("ROLE_ADMIN");

                // Verificar si el usuario actual es el mismo que se está intentando eliminar
                if (emailUsuarioActual == usuario.email)
                {
                    // Si el usuario intenta eliminarse a sí mismo, mostrar mensaje y redirigir
                    ViewData["noTePuedesEliminar"] = "Un administrador no puede eliminarse a sí mismo";
                    ViewBag.Usuarios = usuarios;
                    return View("~/Views/Home/administracion.cshtml");
                }
                // Verificar si el usuario actual es administrador, y si es el último, no permitir la eliminación
                else if (User.IsInRole("ROLE_ADMIN") && adminsRestantes == 1 && usuario.tipo_usuario == "ROLE_ADMIN")
                {
                    ViewData["noSePuedeEliminar"] = "No se puede eliminar al último administrador del sistema";
                    ViewBag.Usuarios = usuarios;
                    return View("~/Views/Home/administracion.cshtml");
                }

                // Eliminar el usuario
                _usuarioServicio.eliminar(id);
                Logs.log("Usuario " + id + " Eliminado");
                ViewData["eliminacionCorrecta"] = "El usuario se ha eliminado correctamente";
                ViewBag.Usuarios = _usuarioServicio.obtenerTodosLosUsuarios();

                return View("~/Views/Home/administracion.cshtml");
            }
            catch (Exception e)
            {
                Logs.log("Error al eliminar el usuario" + e);
                ViewData["error"] = "Ocurrió un error al eliminar el usuario";
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        // Controlador para obtener la lista de usuarios
        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpGet]
        [Route("/privada/administracion-usuarios")]
        public IActionResult ListaUsuarios(string busquedaUser)
        {
            try
            {
                Logs.log("Inicio de la obtención de la lista de usuarios");

                List<UsuarioDTO> usuarios = new List<UsuarioDTO>();

                if (!string.IsNullOrEmpty(busquedaUser))
                {
                    // Buscar usuarios por coincidencia en el email
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
                Logs.log("Error al obtener la lista de usuarios" + e); 
                ViewData["error"] = "Ocurrió un error al obtener la lista de usuarios";
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        // Controladro para mostrar el formulario de edición de usuario
        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpGet]
        [Route("/privada/editar-usuario/{id}")]
        public IActionResult FormularioEdicion(long id)
        {
            try
            {
                Logs.log("Inicio de edición de usuario " + id); 

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
                Logs.log("Error al obtener el usuario para editar" + e); 
                ViewData["error"] = "Ocurrió un error al obtener el usuario para editar";
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

        // Controlador para procesar el formulario de edición de usuario
        [Authorize(Roles = "ROLE_ADMIN")]
        [HttpPost]
        [Route("/privada/procesar-editar")]
        public IActionResult ProcesarFormularioEdicion(long id, string nombre, string apellidos, string telefono, string rol, IFormFile foto)
        {
            try
            {
                Logs.log("Inicio de procesamiento de edición del usuario " + id); 

                // Buscar el usuario por su id
                UsuarioDTO usuarioDTO = _usuarioServicio.buscarPorId(id);

                usuarioDTO.name = nombre;
                usuarioDTO.last_name = apellidos;
                usuarioDTO.tlf = telefono;

                // Actualizar los datos del usuario
                _usuarioServicio.actualizarUsuario(usuarioDTO);

                ViewData["EdicionCorrecta"] = "El Usuario se ha editado correctamente";
                ViewBag.Usuarios = _usuarioServicio.obtenerTodosLosUsuarios();

                return View("~/Views/Home/administracion.cshtml");
            }
            catch (Exception e)
            {
                Logs.log("Error al editar el usuario" + e); 
                ViewData["Error"] = "Ocurrió un error al editar el usuario";
                return View("~/Views/Home/dashboard.cshtml");
            }
        }

    }
}

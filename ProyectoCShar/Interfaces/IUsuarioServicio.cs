using DAL.DAO;
using Microsoft.AspNetCore.Mvc;
using ProyectoCShar.DTOs;

namespace ProyectoCShar.Interfaces
{
    public interface IUsuarioServicio
    {
        // Registrar Usuario en la BD
        public UsuarioDTO registrarUsuario(UsuarioDTO userDTO, IFormFile fotofile);

        // Verifica si el usuario es el que esta iniciando sesion
        public bool verificarCredenciales(String email, String password);

        // Te devuelve UsuarioDTO por el Email ingresado
        public UsuarioDTO obtenerUsuarioPorEmail(string email);

        // Confirmacion de Cuenta
        public bool confirmarCuenta(string token);

        // Devuelve UsuarioDTO por el Token
        public UsuarioDTO obtenerUsuarioPorToken(string token);

        // modificar la contraseña
        public bool modificarContraseñaConToken(UsuarioDTO usuario);

        // Iniciar la Recuperacion de Contraseñas
        public bool iniciarProcesoRecuperacion(string emailUsuario);

        // Devuelve todos los UsuarioDTO 
        public List<UsuarioDTO> obtenerTodosLosUsuarios();

        // Devuelve un UsuarioDTO por el Id
        public UsuarioDTO buscarPorId(long id);
        
        // Cuenta cuantos Roles Con Admin Existen
        public int contarUsuariosPorRol(string rol);
        
        // Eliminar un usuario por el Id
        public void eliminar(long id);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="palabra"></param>
        /// <returns></returns>
        public List<UsuarioDTO> buscarPorCoincidenciaEnEmail(string palabra);

        // Actualiza el usuario con nuevos datos
        public void actualizarUsuario(UsuarioDTO usuario);

        // Pasar a la vista la Foto del Usuario
        public String mostrarFoto(String email);

    }
}

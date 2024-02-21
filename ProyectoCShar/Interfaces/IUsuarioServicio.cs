using DAL.DAO;
using Microsoft.AspNetCore.Mvc;
using ProyectoCShar.DTOs;

namespace ProyectoCShar.Interfaces
{
    public interface IUsuarioServicio
    {
        public UsuarioDTO registrarUsuario(UsuarioDTO userDTO);

        public bool verificarCredenciales(String email, String password);

        public UsuarioDTO obtenerUsuarioPorEmail(string email);

        public bool confirmarCuenta(string token);

        public UsuarioDTO obtenerUsuarioPorToken(string token);

        public bool modificarContraseñaConToken(UsuarioDTO usuario);

        public bool iniciarProcesoRecuperacion(string emailUsuario);

        public List<UsuarioDTO> obtenerTodosLosUsuarios();

        public UsuarioDTO buscarPorId(long id);
        
        public int contarUsuariosPorRol(string rol);
        
        public void eliminar(long id);
        
        public List<UsuarioDTO> buscarPorCoincidenciaEnEmail(string palabra);

    }
}

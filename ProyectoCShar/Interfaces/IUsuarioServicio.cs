using DAL.DAO;
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
    }
}

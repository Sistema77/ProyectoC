using DAL.DAO;
using ProyectoCShar.DTOs;

namespace ProyectoCShar.Interfaces
{
    public interface IPasarADTO
    {
        // Pasar un UsuarioDAO a DTO
        public UsuarioDTO usuarioToDto(UsuarioDAO usuarioDAO);

        // Pasar una lista de DAOs a DTOs
        public List<UsuarioDTO> listaUsuarioToDto(List<UsuarioDAO> listaUsuario);
    }
}

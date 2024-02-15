using DAL.DAO;
using ProyectoCShar.DTOs;

namespace ProyectoCShar.Interfaces
{
    public interface IPasarADTO
    {
        public UsuarioDTO usuarioToDto(UsuarioDAO usuarioDAO);
    }
}

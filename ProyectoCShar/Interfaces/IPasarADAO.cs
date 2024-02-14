using DAL.DAO;
using ProyectoCShar.DTOs;

namespace ProyectoCShar.Interfaces
{
    public interface IPasarADAO
    {
        /// Convierte un objeto UsuarioDTO en un objeto Usuario de la capa de acceso a datos.
        public UsuarioDAO usuarioToDao(UsuarioDTO usuarioDTO);

        
    }
}

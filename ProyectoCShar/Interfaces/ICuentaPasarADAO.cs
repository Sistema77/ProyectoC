using DAL.DAO;
using ProyectoCShar.DTOs;

namespace ProyectoCShar.Interfaces
{
    public interface ICuentaPasarADAO
    {
        /// Convierte un objeto UsuarioDTO en un objeto Usuario de la capa de acceso a datos.
        public CuentaDAO cuentaToDao(CuentaDTO cuentaDTO);

    }
}

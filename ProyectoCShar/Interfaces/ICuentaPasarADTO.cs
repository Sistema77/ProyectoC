using DAL.DAO;
using ProyectoCShar.DTOs;

namespace ProyectoCShar.Interfaces
{
    public interface ICuentaPasarADTO
    {
        // Pasar un CuentaDAO a DTO
        public CuentaDTO cuentaToDto(CuentaDAO cuentaDAO);

        // Pasar una lista de DAOs a DTOs
        public List<CuentaDTO> listaCuentaToDto(List<CuentaDAO> listacuenta);
    }
}

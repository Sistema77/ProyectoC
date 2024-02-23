using DAL.DAO;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;
using ProyectoCShar.Util;

namespace ProyectoCShar.Servicio
{
    public class CuentaPasarADTOImple : ICuentaPasarADTO

    {
        public List<CuentaDTO> listaCuentaToDto(List<CuentaDAO> listacuenta)
        {
            try
            {
                List<CuentaDTO> listaDto = new List<CuentaDTO>();

                // va de Cuenta a Cuenta pasando a DTO 
                foreach (CuentaDAO u in listacuenta)
                {
                    listaDto.Add(cuentaToDto(u));
                }

                return listaDto;
            }
            catch (Exception e)
            {
                Logs.log("Error al pasar los datos del de DAO a DTO");
            }
            return null;
        }

        public CuentaDTO cuentaToDto(CuentaDAO cuentaDAO)
        {
            try
            {
                CuentaDTO dto = new CuentaDTO();

                // Pasar de DAO a DTO

                dto.id_cuenta = cuentaDAO.id_cuenta;
                dto.saldo = cuentaDAO.saldo;
                dto.fch_apertura = cuentaDAO.fch_apertura;
                dto.numero_cuenta = cuentaDAO.numero_cuenta;
                dto.con_nomina = cuentaDAO.con_nomina;

                return dto;
            }
            catch (Exception e)
            {
                Logs.log("Error al pasar los datos del de DAO a DTO");
                return null;
            }
        }
    }
}

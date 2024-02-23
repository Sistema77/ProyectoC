using DAL.DAO;
using Humanizer;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;
using ProyectoCShar.Util;

namespace ProyectoCShar.Servicio
{
    public class CuentaPasarADAOImple : ICuentaPasarADAO
    {
        public CuentaDAO cuentaToDao(CuentaDTO cuentaDTO)
        {
            try
            {
                CuentaDAO cuentaDao = new CuentaDAO();

                // Pasar los datos de DTO a DAO

                cuentaDao.id_cuenta = cuentaDTO.id_cuenta;
                cuentaDao.saldo = cuentaDTO.saldo;
                cuentaDao.fch_apertura = cuentaDTO.fch_apertura;
                cuentaDao.numero_cuenta = cuentaDTO.numero_cuenta;
                cuentaDao.con_nomina = cuentaDTO.con_nomina;
                cuentaDao.id_usuario = cuentaDTO.id_usuario;
                return cuentaDao;
            }
            catch (Exception e)
            {
                Logs.log("Error a pasar de DTO a DAO");

                return null;
            }
        }
    }
}
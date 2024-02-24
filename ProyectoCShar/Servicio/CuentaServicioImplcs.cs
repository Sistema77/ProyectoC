using DAL;
using DAL.DAO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;
using ProyectoCShar.Util;

namespace ProyectoCShar.Servicio
{
    public class CuentaServicioImplcs : ICuentaServicio
    {
        private readonly ModelContext _contexto;
        private readonly ICuentaPasarADAO _pasaraDAO;
        private readonly ICuentaPasarADTO _pasaraDTO;
        private readonly IUsuarioServicio _usarioServicio;

        public CuentaServicioImplcs(ModelContext contexto, ICuentaPasarADAO pasaraDAO, ICuentaPasarADTO pasaraDTO, IUsuarioServicio usarioServicio)
        {
            _contexto = contexto;
            _pasaraDAO = pasaraDAO;
            _pasaraDTO = pasaraDTO;
            _usarioServicio = usarioServicio;
        }

        public CuentaDTO registrarCuenta(/*CuentaDTO cuentDTO,*/ String emailDueño)
        {
            try
            {
                CuentaDTO cuentDTO = new CuentaDTO(); // Borrar despues de las Pruebas

                CuentaDAO cuentaDao = new CuentaDAO();

                cuentaDao.fch_apertura = DateTime.Now.ToUniversalTime();
                cuentaDao.saldo = 0;
                cuentaDao.con_nomina = false;
                cuentaDao.numero_cuenta = "ES9420805801101234567891";
                
                // Relacionar Cuenta con Usuario
                UsuarioDTO usuario = _usarioServicio.obtenerUsuarioPorEmail(emailDueño);
                cuentaDao.id_usuario = usuario.id_usuario;

                _contexto.cuentaDAO.Add(cuentaDao);
                _contexto.SaveChanges();

                return cuentDTO;
            }
            catch (DbUpdateException dbe)
            {
                Logs.log("Error al intentar actualizar la base de datos");
                return null;
            }
            catch (Exception e)
            {
                Logs.log("Error al intentar registrar al usuario");
                return null;
            }
        }

        public List<CuentaDTO> obtenerTodosLasCuentas(long idUsuario)
        {
            try
            {
                List<CuentaDTO> listaCompletaDeCuentas = _pasaraDTO.listaCuentaToDto(_contexto.cuentaDAO.ToList());
                List<CuentaDTO> listaCuemtaDelUsuario = new List<CuentaDTO>();

                foreach(CuentaDTO cuentaDTO in listaCompletaDeCuentas)
                {
                    if(cuentaDTO.id_usuario == idUsuario)
                    {
                        listaCuemtaDelUsuario.Add(cuentaDTO);
                    }
                }

                return listaCuemtaDelUsuario;
            }
            catch (Exception e)
            {
                Logs.log("Error al optener Todos los usuarios");
                return null;
            }
        }

        public CuentaDTO buscarPorId(long id)
        {
            try
            {
                CuentaDAO? cuenta = _contexto.cuentaDAO.FirstOrDefault(u => u.id_cuenta == id);
                if (cuenta != null)
                {
                    return _pasaraDTO.cuentaToDto(cuenta);
                }
            }
            catch (ArgumentException iae)
            {
                Logs.log("Error en el Argumento");
            }
            return null;
        }

        public void eliminar(long id)
        {
            try
            {
                CuentaDAO cuenta = _contexto.cuentaDAO.Find(id);
                if (cuenta != null)
                {
                    _contexto.cuentaDAO.Remove(cuenta);
                    _contexto.SaveChanges();
                    Logs.log("Cuenta Eliminado");
                }
            }
            catch (DbUpdateException dbe)
            {
                Logs.log("Error al Eliminar la Cuenta");
            }
        }

        public void actualizarCuenta(CuentaDTO cuenta)
        {
            try
            {

                CuentaDAO? cuentaActual = _contexto.cuentaDAO.Find(cuenta.id_cuenta);

                if (cuentaActual != null)
                {
                    cuentaActual.con_nomina = cuenta.con_nomina;
                    cuentaActual.saldo = cuenta.saldo;
                    

                    _contexto.cuentaDAO.Update(cuentaActual);
                    _contexto.SaveChanges();

                    Logs.log("Cuenta Actualizado");
                }
                else
                {
                    Logs.log("Cuenta no encontrado");
                }
            }
            catch (DbUpdateException e)
            {
                Logs.log("Error al Actualizar el Cuenta");
            }
        }
    }
}

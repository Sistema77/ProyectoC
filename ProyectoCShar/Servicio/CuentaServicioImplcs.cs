﻿using DAL;
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

        public CuentaServicioImplcs(ModelContext contexto, ICuentaPasarADAO pasaraDAO, ICuentaPasarADTO pasaraDTO)
        {
            _contexto = contexto;
            _pasaraDAO = pasaraDAO;
            _pasaraDTO = pasaraDTO;
        }

        public CuentaDTO registrarCuenta(CuentaDTO cuentDTO)
        {
            try
            {
                CuentaDAO cuentaDao = _pasaraDAO.cuentaToDao(cuentDTO);

                cuentaDao.fch_apertura = DateTime.Now.ToUniversalTime();
                cuentaDao.saldo = 0;
                cuentaDao.con_nomina = false;
                cuentaDao.numero_cuenta = "ES9420805801101234567891";

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

        public List<CuentaDTO> obtenerTodosLasCuentas()
        {
            try
            {
                return _pasaraDTO.listaCuentaToDto(_contexto.cuentaDAO.ToList());
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
    }
}
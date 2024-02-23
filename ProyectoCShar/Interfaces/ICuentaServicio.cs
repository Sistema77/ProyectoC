using Microsoft.AspNetCore.Mvc;
using ProyectoCShar.DTOs;

namespace ProyectoCShar.Interfaces
{
    public interface ICuentaServicio 
    {
        // Registrar Cuenta en la BD
        public CuentaDTO registrarCuenta(/*CuentaDTO cuentDTO,*/ String emailUsuario);

        public List<CuentaDTO> obtenerTodosLasCuentas();

        public CuentaDTO buscarPorId(long id);

        public void eliminar(long id);

    }
}

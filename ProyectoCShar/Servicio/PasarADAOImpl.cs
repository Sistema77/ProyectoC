using DAL.DAO;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;

namespace ProyectoCShar.Servicio
{

    public class PasarADAOImpl : IPasarADAO
    {
        /// <summary>
        /// Pasar A DAO
        /// </summary>
        /// <param name="usuarioDTO"></param>
        /// <returns></returns>
        public UsuarioDAO usuarioToDao(UsuarioDTO usuarioDTO)
        {
            try
            {
                UsuarioDAO usuarioDao = new UsuarioDAO();

                usuarioDao.id_usuario = usuarioDTO.id_usuario;
                usuarioDao.name = usuarioDTO.name;
                usuarioDao.dni = usuarioDTO.dni;
                usuarioDao.last_name = usuarioDTO.last_name;
                usuarioDao.email = usuarioDTO.email;
                usuarioDao.password = usuarioDTO.password;
                usuarioDao.tlf = usuarioDTO.tlf;

                return usuarioDao;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}

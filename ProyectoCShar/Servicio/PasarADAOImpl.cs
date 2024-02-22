using DAL.DAO;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;
using ProyectoCShar.Util;

namespace ProyectoCShar.Servicio
{

    public class PasarADAOImpl : IPasarADAO
    {

        public UsuarioDAO usuarioToDao(UsuarioDTO usuarioDTO)
        {
            try
            {
                UsuarioDAO usuarioDao = new UsuarioDAO();

                // Pasar los datos de DTO a DAO

                usuarioDao.id_usuario = usuarioDTO.id_usuario;
                usuarioDao.name = usuarioDTO.name;
                usuarioDao.foto = usuarioDTO.foto; 
                usuarioDao.dni = usuarioDTO.dni;
                usuarioDao.expiracion_token = usuarioDTO.expiracion_token;
                usuarioDao.last_name = usuarioDTO.last_name;
                usuarioDao.email = usuarioDTO.email;
                usuarioDao.password = usuarioDTO.password;
                usuarioDao.tlf = usuarioDTO.tlf;

                return usuarioDao;
            }
            catch (Exception e)
            {
                Logs.log("Error a pasar de DTO a DAO");

                return null;
            }
        }
    }
}

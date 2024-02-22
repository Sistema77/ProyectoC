using DAL.DAO;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;
using ProyectoCShar.Util;
using System.Text;

namespace ProyectoCShar.Servicio
{
    public class PasarADTOImpl : IPasarADTO
    {
        public UsuarioDTO usuarioToDto(UsuarioDAO UsuarioDAO)
        {
            try
            {
                UsuarioDTO dto = new UsuarioDTO();

                // Pasar de DAO a DTO

                dto.name = UsuarioDAO.name;
                dto.tlf = UsuarioDAO.tlf;
                dto.email = UsuarioDAO.email;
                dto.foto = UsuarioDAO.foto; 
                dto.password = UsuarioDAO.password;
                dto.token = UsuarioDAO.token;
                dto.expiracion_token = UsuarioDAO.expiracion_token;
                dto.id_usuario = UsuarioDAO.id_usuario;
                dto.fch_alta = UsuarioDAO.fch_alta;
                dto.cuentaConfirmada = UsuarioDAO.cuentaConfirmada;
                dto.tipo_usuario = UsuarioDAO.tipo_usuario;

                if (UsuarioDAO.foto != null)
                {
                    dto.foto = UsuarioDAO.foto;
                }

                return dto;
            }
            catch (Exception e)
            {
                Logs.log("Error al pasar los datos del de DAO a DTO");
                return null;
            }
        }

        public List<UsuarioDTO> listaUsuarioToDto(List<UsuarioDAO> listaUsuario)
        {
            try
            {
                List<UsuarioDTO> listaDto = new List<UsuarioDTO>();

                // va de Usuario a Usuario pasando a DTO 
                foreach (UsuarioDAO u in listaUsuario)
                {
                    listaDto.Add(usuarioToDto(u));
                }
                
                return listaDto;
            }
            catch (Exception e)
            {
                Logs.log("Error al pasar los datos del de DAO a DTO");
            }
            return null;
        }
    }
}

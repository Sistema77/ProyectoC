using DAL.DAO;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;
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

                dto.name = UsuarioDAO.name;

                dto.tlf = UsuarioDAO.tlf;
                dto.email = UsuarioDAO.email;
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
                return null;
            }
        }

        public List<UsuarioDTO> listaUsuarioToDto(List<UsuarioDAO> listaUsuario)
        {
            try
            {
                List<UsuarioDTO> listaDto = new List<UsuarioDTO>();
                foreach (UsuarioDAO u in listaUsuario)
                {
                    listaDto.Add(usuarioToDto(u));
                }
                
                return listaDto;
            }
            catch (Exception e)
            {
                //Log
            }
            return null;
        }
    }
}

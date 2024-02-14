using DAL;
using DAL.DAO;
using Microsoft.EntityFrameworkCore;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;
using System.Security.Cryptography;

namespace ProyectoCShar.Servicio
{
    public class UsuarioServicioImpl : IUsuarioServicio
    {
        private readonly ModelContext _contexto;
        private readonly IPasarADAO _pasaraDAO;
        private readonly IPasarADTO _pasaraDTO;
        private readonly IEncriptar _encriptar;
        public UsuarioServicioImpl(ModelContext contexto, IPasarADAO pasaraDAO, IPasarADTO pasaraDTO, IEncriptar encriptar)
        {
            _contexto = contexto;
            _pasaraDAO = pasaraDAO;
            _pasaraDTO = pasaraDTO;
            _encriptar = encriptar;
        }
        public UsuarioDTO registrarUsuario(UsuarioDTO userDTO)
        {
            try
            {
                
                var usuarioExistente = _contexto.usuarioDAO.FirstOrDefault(u => u.email == userDTO.email && !u.cuentaConfirmada);

                if (usuarioExistente != null)
                {
                    userDTO.email = "EmailNoConfirmado";
                    return userDTO;
                }

                var emailExistente = _contexto.usuarioDAO.FirstOrDefault(u => u.email == userDTO.email && u.cuentaConfirmada);

                if (emailExistente != null)
                {
                    userDTO.email = "EmailRepetido";
                    return userDTO;
                }

                userDTO.password = _encriptar.encriptar(userDTO.password);
                UsuarioDAO usuarioDao = _pasaraDAO.usuarioToDao(userDTO);
                usuarioDao.fch_alta = DateTime.Now;
                usuarioDao.tipo_usuario = "ROLE_USER";
                string token = generarToken();
                usuarioDao.token = token;

                _contexto.usuarioDAO.Add(usuarioDao);
                _contexto.SaveChanges();

                string nombreUsuario = usuarioDao.name;

                _servicioEmail.enviarEmailConfirmacion(userDTO.email, nombreUsuario, token);

                return userDTO;
            }
            catch (DbUpdateException dbe)
            {
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private string generarToken()
        {
            try
            {
                using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
                {
                    byte[] tokenBytes = new byte[30];
                    rng.GetBytes(tokenBytes);

                    return BitConverter.ToString(tokenBytes).Replace("-", "").ToLower();
                }
            }
            catch (ArgumentException ae)
            {
                return null;
            }

        }

        public bool confirmarCuenta(string token)
        {
            try
            {
                

                UsuarioDAO? usuarioExistente = _contexto.usuarioDAO.Where(u => u.token == token).FirstOrDefault();

                if (usuarioExistente != null && !usuarioExistente.cuentaConfirmada)
                {
                    // Entra en esta condición si el usuario existe y su cuenta no se ha confirmado
                    usuarioExistente.cuentaConfirmada = true;
                    usuarioExistente.token = null;
                    _contexto.usuarioDAO.Update(usuarioExistente);
                    _contexto.SaveChanges();
         
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (ArgumentException ae)
            {
               
                return false;
            }
            catch (Exception e)
            {
                
                return false;
            }
        }
    }
}

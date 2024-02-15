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
        private readonly IServicioEncriptar _servicioEncriptar;
        private readonly IServicioEmail _servicioEmail;
        public UsuarioServicioImpl(ModelContext contexto, IPasarADAO pasaraDAO, IPasarADTO pasaraDTO, IServicioEncriptar servicioEncriptar, IServicioEmail servicioEmail)
        {
            _contexto = contexto;
            _pasaraDAO = pasaraDAO;
            _pasaraDTO = pasaraDTO;
            _servicioEncriptar = servicioEncriptar;
            _servicioEmail = servicioEmail;
        }
        public UsuarioDTO registrarUsuario(UsuarioDTO userDTO)
        {
            Console.WriteLine("/////////////////////////////////////////////////////////////////////////////////");
            Console.WriteLine(userDTO.email);
            try
            {
                Console.WriteLine("Entra en Registro");
                var usuarioExistente = _contexto.usuarioDAO.FirstOrDefault(u => u.email == userDTO.email && !u.cuentaConfirmada);
                
                if (usuarioExistente != null)
                {
                    Console.WriteLine("/////////////////////////////////////////////////////////////////////////////////");
                    Console.WriteLine("Entra como Email no Confirmado");
                    userDTO.email = "EmailNoConfirmado";
                    return userDTO;
                }

                var emailExistente = _contexto.usuarioDAO.FirstOrDefault(u => u.email == userDTO.email && u.cuentaConfirmada);

                if (emailExistente != null)
                {
                    Console.WriteLine("/////////////////////////////////////////////////////////////////////////////////");
                    Console.WriteLine("Entra como Email Repetido");
                    userDTO.email = "EmailRepetido";
                    return userDTO;
                }
                
                userDTO.password = _servicioEncriptar.Encriptar(userDTO.password);
                Console.WriteLine("/////////////////////////////////////////////////////////////////////////////////");
                Console.WriteLine("Pone COntraseña" + userDTO.password);
                UsuarioDAO usuarioDao = _pasaraDAO.usuarioToDao(userDTO);
                
                usuarioDao.fch_alta = DateTime.Now.ToUniversalTime();
                Console.WriteLine("/////////////////////////////////////////////////////////////////////////////////");
                Console.WriteLine("Pone Fecha Actial" + usuarioDao.fch_alta);
               
                usuarioDao.tipo_usuario = "ROLE_USER";
                Console.WriteLine("/////////////////////////////////////////////////////////////////////////////////");
                Console.WriteLine("Asigna Rol" + usuarioDao.tipo_usuario);
                string token = generarToken();
                usuarioDao.token = token;
                Console.WriteLine("/////////////////////////////////////////////////////////////////////////////////");
                Console.WriteLine("Asigna TOKEN" + usuarioDao.token);

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
                    Console.WriteLine("Genera TOKEN");
                    byte[] tokenBytes = new byte[30];
                    rng.GetBytes(tokenBytes);

                    return BitConverter.ToString(tokenBytes).Replace("-", "").ToLower();
                }
            }
            catch (ArgumentException ae)
            {
                return null;
            }
            catch (Exception ae)
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

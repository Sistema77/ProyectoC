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
                
                userDTO.password = _servicioEncriptar.Encriptar(userDTO.password);
                UsuarioDAO usuarioDao = _pasaraDAO.usuarioToDao(userDTO);
                
                usuarioDao.fch_alta = DateTime.Now.ToUniversalTime();
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

        public bool verificarCredenciales(string emailUsuario, string claveUsuario)
        {
            try
            {
                Console.WriteLine("/////////////////");
                Console.WriteLine("Verifica Credenciales");
                string contraseñaEncriptada = _servicioEncriptar.Encriptar(claveUsuario);
                Console.WriteLine("/////////////////");
                Console.WriteLine("Encripta");
                UsuarioDAO? usuarioExistente = _contexto.usuarioDAO.FirstOrDefault(u => u.email == emailUsuario && u.password == contraseñaEncriptada);
                Console.WriteLine("/////////////////");
                Console.WriteLine("Busca Usuario");
                Console.WriteLine(usuarioExistente.email);
                if (usuarioExistente == null)
                {
                   //log
                }
                if (!usuarioExistente.cuentaConfirmada)
                {
                    
                    return false;
                }

                return true;
            }
            catch (ArgumentNullException e)
            {
               
                return false;
            }

        }

        public UsuarioDTO obtenerUsuarioPorEmail(string email)
        {
            try
            {
                Console.WriteLine("/////////////////");
                Console.WriteLine("obtenerUsuarioPorEmail");
                UsuarioDTO usuarioDTO = new UsuarioDTO();
                var usuario = _contexto.usuarioDAO.FirstOrDefault(u => u.email == email);
                Console.WriteLine("/////////////////");
                Console.WriteLine("Buscar Email");
                if (usuario != null)
                {
                    usuarioDTO = _pasaraDTO.usuarioToDto(usuario);
                    Console.WriteLine("/////////////////");
                    Console.WriteLine("Pasar a DTO");
                }

                return usuarioDTO;
            }
            catch (ArgumentNullException e)
            {
                // Console.WriteLine("[Error UsuarioServicioImpl - obtenerUsuarioPorEmail()] Error al obtener el usuario por email: " + e.Message);
                return null;
            }
        }

    }
}

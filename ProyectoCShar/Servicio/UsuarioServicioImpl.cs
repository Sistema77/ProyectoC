using DAL;
using DAL.DAO;
using Microsoft.AspNetCore.Mvc;
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
               
                string contraseñaEncriptada = _servicioEncriptar.Encriptar(claveUsuario);

                UsuarioDAO? usuarioExistente = _contexto.usuarioDAO.FirstOrDefault(u => u.email == emailUsuario && u.password == contraseñaEncriptada);

                if (usuarioExistente == null)
                {
                    //log
                    return false;
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

                UsuarioDTO usuarioDTO = new UsuarioDTO();
                var usuario = _contexto.usuarioDAO.FirstOrDefault(u => u.email == email);

                if (usuario != null)
                {
                    usuarioDTO = _pasaraDTO.usuarioToDto(usuario);
                }

                return usuarioDTO;
            }
            catch (ArgumentNullException e)
            {
                // Console.WriteLine("[Error UsuarioServicioImpl - obtenerUsuarioPorEmail()] Error al obtener el usuario por email: " + e.Message);
                return null;
            }
        }

        public bool iniciarProcesoRecuperacion(string emailUsuario)
        {
            try
            {
                UsuarioDAO? usuarioExistente = _contexto.usuarioDAO.FirstOrDefault(u => u.email == emailUsuario);

                if (usuarioExistente != null)
                {
                    // Generar el token y establecer la fecha de expiración
                    string token = generarToken();
                    DateTime fechaExpiracion = DateTime.Now.AddMinutes(1);

                    // Actualizar el usuario con el nuevo token y la fecha de expiración
                    usuarioExistente.token = token;
                    usuarioExistente.expiracion_token = fechaExpiracion;

                    // Actualizar el usuario en la base de datos
                    _contexto.usuarioDAO.Update(usuarioExistente);
                    _contexto.SaveChanges();

                    // Enviar el correo de recuperación
                    string nombreUsuario = usuarioExistente.name;
                    _servicioEmail.enviarEmailRecuperacion(emailUsuario, nombreUsuario, token);

                    return true;
                }
                else
                {
                    //Log
                    return false;
                }
            }
            catch (DbUpdateException dbe)
            {
                Console.WriteLine("[Error UsuarioServicioImpl - iniciarProcesoRecuperacion()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Error UsuarioServicioImpl - iniciarProcesoRecuperacion()] Error al iniciar el proceso de recuperación: " + ex.Message);
                return false;
            }
        }

        public UsuarioDTO obtenerUsuarioPorToken(string token)
        {
            try
            {
                
                UsuarioDAO? usuarioExistente = _contexto.usuarioDAO.FirstOrDefault(u => u.token == token);

                if (usuarioExistente != null)
                {
                    UsuarioDTO usuario = _pasaraDTO.usuarioToDto(usuarioExistente);
                    
                    return usuario;
                }
                else
                {
                   //LOG
                    return null;
                }
            }
            catch (ArgumentNullException e)
            {
               
                return null;
            }
        }

        public bool modificarContraseñaConToken(UsuarioDTO usuario)
        {
            try
            {

                UsuarioDAO? usuarioExistente = _contexto.usuarioDAO.FirstOrDefault(u => u.token == usuario.token);

                if (usuarioExistente != null)
                {
                    string nuevaContraseña = _servicioEncriptar.Encriptar(usuario.password);
                    usuarioExistente.password = nuevaContraseña;
                    usuarioExistente.token = null; // Se establece como null para invalidar el token ya consumido al cambiar la contraseña
                    _contexto.usuarioDAO.Update(usuarioExistente);
                    _contexto.SaveChanges();

                    
                    return true;
                }
            }
            catch (DbUpdateException dbe)
            {
                //Log
            }
            catch (ArgumentNullException e)
            {
                //Log
                return false;
            }
            return false;
        }

        public void eliminar(long id)
        {
            try
            {
                UsuarioDAO usuario = _contexto.usuarioDAO.Find(id);
                if (usuario != null)
                {
                    _contexto.usuarioDAO.Remove(usuario);
                    _contexto.SaveChanges();
                }
            }
            catch (DbUpdateException dbe)
            {
                // Log
            }
        }

        public UsuarioDTO buscarPorId(long id)
        {
            try
            {
               

                UsuarioDAO? usuario = _contexto.usuarioDAO.FirstOrDefault(u => u.id_usuario == id);
                if (usuario != null)
                {
                    return _pasaraDTO.usuarioToDto(usuario);
                }
            }
            catch (ArgumentException iae)
            {
               //Log
            }
            return null;
        }

        public List<UsuarioDTO> obtenerTodosLosUsuarios()
        {
            return _pasaraDTO.listaUsuarioToDto(_contexto.usuarioDAO.ToList());
        }
        public int contarUsuariosPorRol(string rol)
        {
            return _contexto.usuarioDAO.Count(u => u.tipo_usuario == rol);
        }
        public List<UsuarioDTO> buscarPorCoincidenciaEnEmail(string palabra)
        {
            try
            {
                List<UsuarioDAO> usuarios = _contexto.usuarioDAO.Where(u => u.email.Contains(palabra)).ToList();
                if (usuarios != null)
                {
                    return _pasaraDTO.listaUsuarioToDto(usuarios);
                }
            }
            catch (Exception e)
            {
                //Log
            }
            return null;
        }
    }
}

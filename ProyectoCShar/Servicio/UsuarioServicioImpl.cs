using DAL;
using DAL.DAO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;
using ProyectoCShar.Util;
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
        public UsuarioDTO registrarUsuario(UsuarioDTO userDTO, IFormFile fotofile)
        {
            try
            {
                //Veo si el Usuario Existe y si esta confirmado 
                var usuarioExistente = _contexto.usuarioDAO.FirstOrDefault(u => u.email == userDTO.email && !u.cuentaConfirmada);
                
                if (usuarioExistente != null)
                {
                    userDTO.email = "EmailNoConfirmado";

                    Logs.log("Email no Encontrado o no Existente");
                    
                    return userDTO;
                }

                // Veo si el email del usuario existe y el usuario esta confirmado
                var emailExistente = _contexto.usuarioDAO.FirstOrDefault(u => u.email == userDTO.email && u.cuentaConfirmada);

                if (emailExistente != null)
                {
                    userDTO.email = "EmailRepetido";

                    Logs.log("Email esta repetido");
                    
                    return userDTO;
                }
                // Encriptando la contraseña
                userDTO.password = _servicioEncriptar.Encriptar(userDTO.password);

                if(fotofile != null)
                {
                    //Pasando a byte[] la foto
                    var imagenesBinarios = new ImagenesBinarios();
                    userDTO.foto = imagenesBinarios.PasarAByte(fotofile);
                }

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
                Logs.log("Error al intentar actualizar la base de datos");
                return null;
            }
            catch (Exception e)
            {
                Logs.log("Error al intentar registrar al usuario");
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
                    
                    Logs.log("Token Generado");

                    return BitConverter.ToString(tokenBytes).Replace("-", "").ToLower();
                }
            }
            catch (ArgumentException ae)
            {
                Logs.log("Error en el Argumento");
                return null;
            }
            catch (Exception ae)
            {
                Logs.log("Error al intentar generar el Token");
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

                    Logs.log("Usuario Confirmado");
                    return true;
                }
                else
                {
                    Logs.log("Usuario No se a Confirmado");
                    return false;
                }
            }
            catch (ArgumentException ae)
            {
                Logs.log("Error Argumento Incorrecto al confirmarCuenta");
                return false;
            }
            catch (Exception e)
            {
                Logs.log("Error al Confirmar Usuario");
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
                    Logs.log("usuario encontrado");
                    return false;
                }
                if (!usuarioExistente.cuentaConfirmada)
                {
                    Logs.log("Usuario encontrado pero sin Confirmar");
                    return false;
                }

                return true;
            }
            catch (ArgumentNullException e)
            {
                Logs.log("Error al intentar verigicar Credenciales del usuario");
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
                Logs.log("[Error UsuarioServicioImpl - obtenerUsuarioPorEmail()] Error al obtener el usuario por email: " + e.Message);
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

                    Logs.log("Proceso de recuperacion iniciado");
                    return true;
                }
                else
                {
                    Logs.log("Usuario no encontrado");
                    return false;
                }
            }
            catch (DbUpdateException dbe)
            {
                Logs.log("[Error UsuarioServicioImpl - iniciarProcesoRecuperacion()] Error de persistencia al actualizar la bbdd: " + dbe.Message);
                return false;
            }
            catch (Exception ex)
            {
                Logs.log("[Error UsuarioServicioImpl - iniciarProcesoRecuperacion()] Error al iniciar el proceso de recuperación: " + ex.Message);
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
                    Logs.log("Problemas al comprara el Token y el Token del Usuario");
                    return null;
                }
            }
            catch (ArgumentNullException e)
            {
                Logs.log("Error de Argumentos");
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

                    Logs.log("Contraseña Actualizada");

                    return true;
                }
            }
            catch (DbUpdateException dbe)
            {
                Logs.log("Error al Actualizar la Contraseña del usuario");
            }
            catch (ArgumentNullException e)
            {
                Logs.log("Error en el Argumento");
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
                    Logs.log("Usuario Eliminado");
                }
            }
            catch (DbUpdateException dbe)
            {
                Logs.log("Error al Eliminar al usuariario");
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
                Logs.log("Error en el Argumento");
            }
            return null;
        }

        public List<UsuarioDTO> obtenerTodosLosUsuarios()
        {
            try
            {
                return _pasaraDTO.listaUsuarioToDto(_contexto.usuarioDAO.ToList());
            }
            catch (Exception e)
            {
                Logs.log("Error al optener Todos los usuarios");
                return null;
            }
        }
        public int contarUsuariosPorRol(string rol)
        {
            try
            {
                return _contexto.usuarioDAO.Count(u => u.tipo_usuario == rol);
            }catch(Exception e)
            {
                Logs.log("Error Contar cuantos Administradores hay");
                return 0;
            }
        }

        /// <summary>
        /// Por si da tiempo
        /// </summary>
        /// <param name="palabra"></param>
        /// <returns></returns>
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
                Logs.log("Error al buscoar por Email");
                return null;
            }
            return null;
        }

        public void actualizarUsuario(UsuarioDTO usuario)
        {
            try
            {

                UsuarioDAO? usuarioActual = _contexto.usuarioDAO.Find(usuario.id_usuario);

                if (usuarioActual != null)
                {
                    usuarioActual.name = usuario.name;
                    usuarioActual.last_name = usuario.last_name;
                    usuarioActual.tlf = usuario.tlf;

                    _contexto.usuarioDAO.Update(usuarioActual);
                    _contexto.SaveChanges();

                    Logs.log("Usuario Actualizado");
                }
                else
                {
                    Logs.log("Usuario no encontrado");
                }
            }
            catch (DbUpdateException e)
            {
                Logs.log("Error al Actualizar el Usuario");
            }
        }

        public String mostrarFoto(String email)
        {
            try
            {
                ImagenesBinarios imagen = new ImagenesBinarios();

                UsuarioDTO usuario = obtenerUsuarioPorEmail(email);

                return imagen.PasarAFile(usuario.foto);
            }catch(Exception e)
            {
                Logs.log("Error al Cargar la foto");
            }
            return null;
        }
    }
}

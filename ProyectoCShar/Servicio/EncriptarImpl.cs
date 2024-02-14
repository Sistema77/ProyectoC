using ProyectoCShar.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoCShar.Servicio
{
    public class EncriptarImpl : IEncriptar
    {
        public string encriptar(string password)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(password);
                    byte[] hashBytes = sha256.ComputeHash(bytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    return hash;
                }
            }
            catch (ArgumentException e)
            {
                return null;
            }

        }
    }
}

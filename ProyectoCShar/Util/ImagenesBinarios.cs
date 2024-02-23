using ProyectoCShar.DTOs;
using ProyectoCShar.Interfaces;
using System;
using System.IO;

namespace ProyectoCShar.Util
{
    public class ImagenesBinarios : IImagenes
    {
        public byte[] PasarAByte(IFormFile foto)
        {
            try
            {
                byte[] fotoBytes;

                using (var memoryStream = new MemoryStream())
                {
                    foto.CopyTo(memoryStream);
                    fotoBytes = memoryStream.ToArray();
                }

                return fotoBytes;
            }
            catch (Exception ex)
            {
                Logs.log("Error a pasar Foto a byte[]");
                return null;
            }
        }

        public String PasarAFile(byte[] fotoByte)
        {
            try
            {
                MemoryStream ms = new MemoryStream(fotoByte);
                String file = Convert.ToBase64String(fotoByte);

                return file;
            }
            catch (Exception ex)
            {
                Logs.log("Error al pasar Foto a IFormFile");
                return null;
            }
        }
    }
}

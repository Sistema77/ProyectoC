﻿using ProyectoCShar.DTOs;
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

        public IFormFile PasarAFile(byte[] fotoByte)
        {
            try
            {
                MemoryStream ms = new MemoryStream(fotoByte);
                IFormFile file = new FormFile(ms, 0, fotoByte.Length, "foto", "foto");

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
namespace ProyectoCShar.Interfaces
{
    public interface IImagenes
    {
        byte[] PasarAByte(IFormFile foto);

        IFormFile PasarAFile(byte[] fotoByte);

    }
}

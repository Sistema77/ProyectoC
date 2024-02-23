namespace ProyectoCShar.Interfaces
{
    public interface IImagenes
    {
        byte[] PasarAByte(IFormFile foto);

        String PasarAFile(byte[] fotoByte);

    }
}

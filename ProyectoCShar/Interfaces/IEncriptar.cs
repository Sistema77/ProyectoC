namespace ProyectoCShar.Interfaces
{
    public interface IEncriptar
    {
        public interface IServicioEncriptar
        {
            // Encripta la Contraseña
            public string encriptar(string texto);
        }
    }
}
}

namespace ProyectoCShar.Util
{
    public class Logs
    {
        public static void log(string message)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(@AppDomain.CurrentDomain.BaseDirectory + "log.txt"))
                {
                    writer.WriteLine($"{DateTime.Now} - {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en el log file: {ex.Message}");
            }
        }
    }
}

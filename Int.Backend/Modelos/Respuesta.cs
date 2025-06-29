namespace Int.Backend.Modelos
{
    public class Respuesta<T>
    {
        public T? Resultado { get; set; }
        public string? Mensaje { get; set; }
        public int CantidadRegistrada { get; set; }
    }
}

namespace Int.Backend.Entidades
{
    public class Profesor
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public ICollection<ProfesorMateria> ProfesorMaterias { get; set; }
    }
}

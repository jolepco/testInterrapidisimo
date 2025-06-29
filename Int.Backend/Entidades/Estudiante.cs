namespace Int.Backend.Entidades
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;

        public ICollection<EstudianteMateria>? EstudianteMaterias { get; set; }
    }
}

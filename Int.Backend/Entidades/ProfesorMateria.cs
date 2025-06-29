namespace Int.Backend.Entidades
{
    public class ProfesorMateria
    {
        public int Id { get; set; }
        public int ProfesorId { get; set; }
        public Profesor? Profesor { get; set; }

        public int MateriaId { get; set; }
        public Materia? Materia { get; set; }

        public ICollection<EstudianteMateria>? EstudianteMaterias { get; set; }
    }
}

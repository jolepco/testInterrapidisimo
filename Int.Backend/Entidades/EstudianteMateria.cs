namespace Int.Backend.Entidades
{
    public class EstudianteMateria
    {
        public int Id { get; set; }
        public int EstudianteId { get; set; }
        public Estudiante? Estudiante { get; set; }
        public int ProfesorMateriaId { get; set; }
        public ProfesorMateria? ProfesorMateria { get; set; }
    }
}

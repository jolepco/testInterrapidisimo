namespace Int.Backend.Entidades
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int Creditos { get; set; } = 3;

        public ICollection<ProfesorMateria> ProfesorMaterias { get; set; }
    }
}

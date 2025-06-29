using Int.Backend.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Int.Backend.Data
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
        {
        }

        public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
        public DbSet<EstudianteMateria> EstudianteMaterias => Set<EstudianteMateria>();
        public DbSet<Materia> Materias => Set<Materia>();
        public DbSet<Profesor> Profesores => Set<Profesor>();
        public DbSet<ProfesorMateria> ProfesorMaterias => Set<ProfesorMateria>();

    }
}

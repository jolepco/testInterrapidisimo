using Int.Backend.Entidades;

namespace Int.Backend.Data
{
    public class SeedDb
    {
        private readonly AppDataContext _context;

        public SeedDb(AppDataContext context)
        {
            _context = context;
        }
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckProfesoresAsync();
            await CheckMateriasAsync();
            await CheckMateriasProfesorAsync();
        }
        private async Task CheckProfesoresAsync()
        {
            if (!_context.Profesores.Any())
            {
                _context.Profesores.Add(new Profesor { Nombre = "Pedro Gomez" });
                _context.Profesores.Add(new Profesor { Nombre = "Carolina Cepeda" });
                _context.Profesores.Add(new Profesor { Nombre = "Fernando Puerto" });
                _context.Profesores.Add(new Profesor { Nombre = "Cecilia Corredor" });
                _context.Profesores.Add(new Profesor { Nombre = "Manolo Cardenas" });
                await _context.SaveChangesAsync();
                
            }
        }
        
        private async Task CheckMateriasAsync()
        {
            if (!_context.Materias.Any())
            {
                _context.Materias.Add(new Materia {Nombre = "Base de datos I" , Creditos =3});
                _context.Materias.Add(new Materia {Nombre = "Fisica", Creditos = 3 });
                _context.Materias.Add(new Materia {Nombre = "Humanidades", Creditos = 3 });
                _context.Materias.Add(new Materia {Nombre = "Analisis de Datos I", Creditos = 3 });
                _context.Materias.Add(new Materia {Nombre = "Algoritmos I", Creditos = 3 });
                _context.Materias.Add(new Materia {Nombre = "Educacion Fisica", Creditos = 3 });
                _context.Materias.Add(new Materia {Nombre = "Lógica basica", Creditos = 3 });
                _context.Materias.Add(new Materia {Nombre = "Matematicas", Creditos = 3 });
                _context.Materias.Add(new Materia {Nombre = "Introduccion Ciencias computación", Creditos = 3 });
                _context.Materias.Add(new Materia {Nombre = "Ingles I", Creditos = 3 });
                await _context.SaveChangesAsync();

            }
        }
        private async Task CheckMateriasProfesorAsync()
        {
            if (!_context.ProfesorMaterias.Any())
            {
                _context.ProfesorMaterias.Add(new ProfesorMateria {MateriaId=1 , ProfesorId=1 });
                _context.ProfesorMaterias.Add(new ProfesorMateria {MateriaId = 2, ProfesorId = 1 });

                _context.ProfesorMaterias.Add(new ProfesorMateria {MateriaId = 3, ProfesorId = 2 });
                _context.ProfesorMaterias.Add(new ProfesorMateria {MateriaId = 4, ProfesorId = 2 });

                _context.ProfesorMaterias.Add(new ProfesorMateria {MateriaId = 5, ProfesorId = 3 });
                _context.ProfesorMaterias.Add(new ProfesorMateria {MateriaId = 6, ProfesorId = 3 });

                _context.ProfesorMaterias.Add(new ProfesorMateria {MateriaId = 7, ProfesorId = 4 });
                _context.ProfesorMaterias.Add(new ProfesorMateria {MateriaId = 8, ProfesorId = 4 });

                _context.ProfesorMaterias.Add(new ProfesorMateria {MateriaId = 9, ProfesorId = 5 });
                _context.ProfesorMaterias.Add(new ProfesorMateria { MateriaId =10, ProfesorId = 5 });
                await _context.SaveChangesAsync();

            }
        }
    }
}
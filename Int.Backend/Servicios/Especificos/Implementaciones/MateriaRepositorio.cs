using Int.Backend.Data;
using Int.Backend.Entidades;
using Int.Backend.Servicios.Especificos.Interfaces;
using Int.Backend.Servicios.Genericos.Implementaciones;

namespace Int.Backend.Servicios.Especificos.Implementaciones
{
    public class MateriaRepositorio(AppDataContext context) : GenericRepository<Materia>(context), IMateriaRepositorio
    {
    }
}

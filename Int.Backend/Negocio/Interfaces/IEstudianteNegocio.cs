using Int.Backend.Entidades;
using Int.Backend.Modelos;
using Microsoft.AspNetCore.Mvc;

namespace Int.Backend.Negocio.Interfaces
{
    public interface IEstudianteNegocio
    {
        Task<Respuesta<List<EstudianteDtoOut>>> obtenerTodos();
        Task<Respuesta<Estudiante>> agregar(Estudiante estudiante);
        Task<Respuesta<List<ObjetoRespuestaDto>>> VerCompaneros(int id);
        Task<Respuesta<bool>> InscribirMaterias(int id, EnvioData profesorMateriaIds);
        Task<Respuesta<EstudianteDtoOut>> obtenerUno(int id);
        Task<Respuesta<Estudiante>> actualizar(int id, Estudiante estudiante);
        Task<bool> delete(int id);
        Task<Respuesta<MateriasEstudianteDto>> ObtenerMateriasporstudiante(int id);

    }
}

using Int.Backend.Entidades;
using Int.Backend.Modelos;
using Int.Backend.Negocio.Interfaces;
using Int.Backend.Servicios.Especificos.Implementaciones;
using Int.Backend.Servicios.Especificos.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Int.Backend.Negocio.Implementaciones
{
    public class EstudianteNegocio : IEstudianteNegocio
    {
        private readonly IEstudianteMateriaRepositorio _estudianteMateriaRepositorio;
        private readonly IEstudianteRepositorio _estudiante;
        private readonly IProfesorMateriaRepositorio _profesorMateriaRepositorio;
        private readonly IMateriaRepositorio _materiaRepositorio;

        public EstudianteNegocio(IEstudianteMateriaRepositorio estudianteMateriaRepositorio,
            IEstudianteRepositorio estudiante, IProfesorMateriaRepositorio profesorMateriaRepositorio,
            IMateriaRepositorio materiaRepositorio
            )
        {
            _estudianteMateriaRepositorio = estudianteMateriaRepositorio;
            _estudiante = estudiante;
            _profesorMateriaRepositorio = profesorMateriaRepositorio;
            _materiaRepositorio = materiaRepositorio;
        }
        public async Task<Respuesta<Estudiante>> agregar(Estudiante estudiante)
        {
            Respuesta<Estudiante> result = new Respuesta<Estudiante>();
            await _estudiante.AddAsync(estudiante);
            await _estudiante.SaveChangesAsync();
            result.Resultado = estudiante;
            return result;
        }

        public async Task<Respuesta<bool>> InscribirMaterias(int id, EnvioData profesorMateriaIds)
        {
            Respuesta<bool> response = new Respuesta<bool>();
            response.Resultado = false;
            var estudiante = await _estudiante.GetFirst(a => a.Id == id);

            if (estudiante == null)
            {
                response.Mensaje = "Estudiante no encontrado";
                return response;
            }

            //buscamos las materia del alumno en base de datos 

            var materiasporAlumno = await _estudianteMateriaRepositorio.GetAll(a => a.EstudianteId == id);

            if (profesorMateriaIds.seleccionadas.Count > 3)
            {
                
                response.Mensaje = $"La cantidad de materias en base de datos es {materiasporAlumno.TotalCount} y envia {profesorMateriaIds.seleccionadas .Count} , la cantidad maxima de materia son 3 ";
                return response;
            }
            
            List<int> listaid = new List<int>();
            listaid.AddRange(profesorMateriaIds.seleccionadas);

            listaid.AddRange(materiasporAlumno.Items.Select(r => r.ProfesorMateriaId));
            listaid = listaid.Distinct().ToList();

            //var materiasSeleccionadas = await _context.ProfesorMaterias.Include(pm => pm.Profesor).Where(pm => profesorMateriaIds.Contains(pm.Id)).ToListAsync();

            var materiasSeleccionadas = await _profesorMateriaRepositorio.GetAll(a => listaid.Contains(a.MateriaId));

            var cantidad = materiasSeleccionadas.Items.GroupBy(r => r.ProfesorId);

            if (cantidad.Count() != listaid.Count)
            {
                response.Mensaje = $"No puede tener más de una materia con el mismo profesor ";
                return response;
            }

            var transaction = await _estudianteMateriaRepositorio.BeginTransactionAsync();
            List<EstudianteMateria> listaMateriaEstudianteAdd = new List<EstudianteMateria>();
            try
            {
                foreach (var item in profesorMateriaIds.seleccionadas)
                {
                    listaMateriaEstudianteAdd.Add(new EstudianteMateria()
                    {
                        EstudianteId = id,
                        ProfesorMateriaId = item
                    });
                }

                foreach (var item in profesorMateriaIds.faltantes)
                {
                    var estMarDelete = await _estudianteMateriaRepositorio.GetFirst(a => a.EstudianteId == id && a.ProfesorMateriaId == item);
                    await _estudianteMateriaRepositorio.Delete(estMarDelete.Id);

                }

                await _estudianteMateriaRepositorio.AddRangeAsync(listaMateriaEstudianteAdd);

                await _estudianteMateriaRepositorio.SaveChangesAsync();
                await _estudianteMateriaRepositorio.CommitTransactionAsync(transaction);
            }
            catch (Exception ex)
            {
                await _estudianteMateriaRepositorio.RollbackTransactionAsync(transaction);
                throw (ex);
            }


            return response;

        }

        public async Task<Respuesta<List<EstudianteDtoOut>>> obtenerTodos()
        {
            Respuesta<List<EstudianteDtoOut>> result = new Respuesta<List<EstudianteDtoOut>>();

            var listaEstudiante = await _estudiante.GetAll();


            result.Resultado = new List<EstudianteDtoOut>();
            result.Resultado.AddRange(listaEstudiante.Items.ToList().Select(x => new EstudianteDtoOut
            {
                Email = x.Email,
                Id = x.Id,
                Nombre = x.Nombre
            }).ToList());

            return result;
        }

        public async Task<Respuesta<List<ObjetoRespuestaDto>>> VerCompaneros(int id)
        {
            Respuesta<List<ObjetoRespuestaDto>> result = new Respuesta<List<ObjetoRespuestaDto>>();

            var est = await _estudiante.GetFirst(a => a.Id == id);



            if (est == null)
            {
                result.Mensaje = "No se encuentra el estudiante solicitado";
                return result;
            }

            var materiasEstudiante = await _estudianteMateriaRepositorio.GetAll(a => a.EstudianteId == id, include: c => c.Include(r => r.ProfesorMateria).ThenInclude(p => p.Materia));

            List<int> idMaterias = new List<int>();

            foreach (var item in materiasEstudiante.Items)
            {
                idMaterias.Add(item.ProfesorMateriaId);
            }

            var materias = await _estudianteMateriaRepositorio.GetAll(a => idMaterias.Contains(a.ProfesorMateriaId));

            List<int> estudiantesid = materias.Items.Select(r => r.EstudianteId).ToList();

            var _EstudianteListdo = await _estudiante.GetAll(a => estudiantesid.Contains(a.Id) && a.Id != id, include: c => c.Include(r => r.EstudianteMaterias).ThenInclude(e => e.ProfesorMateria).ThenInclude(n => n.Materia));

            List<ObjetoRespuestaDto> resultado = new List<ObjetoRespuestaDto>();

            foreach (var item in _EstudianteListdo.Items)
            {

                ObjetoRespuestaDto res = new ObjetoRespuestaDto
                {

                    Materia = item.EstudianteMaterias?.Where(r => idMaterias.Contains(r.ProfesorMateriaId)).FirstOrDefault().ProfesorMateria.Materia.Nombre,
                    NombreEstudiante = item.Nombre,
                };
                resultado.Add(res);
            }


            result.Resultado = resultado;
            result.CantidadRegistrada = _EstudianteListdo.TotalCount;

            return result;

        }

        public async Task<Respuesta<EstudianteDtoOut>> obtenerUno(int id)
        {
            Respuesta<EstudianteDtoOut> result = new Respuesta<EstudianteDtoOut>();

            var _Estudiante = await _estudiante.GetFirst(a => a.Id == id);


            result.Resultado = new EstudianteDtoOut()
                ;
            if (_Estudiante is not null)
                result.Resultado = new EstudianteDtoOut
                {
                    Email = _Estudiante.Email,
                    Id = _Estudiante.Id,
                    Nombre = _Estudiante.Nombre
                };

            return result;
        }

        public async Task<Respuesta<Estudiante>> actualizar(int id, Estudiante estudiante)
        {
            Respuesta<Estudiante> result = new Respuesta<Estudiante>();
            await _estudiante.Update(id, estudiante);
            await _estudiante.SaveChangesAsync();
            result.Resultado = estudiante;
            return result;
        }

        public async Task<bool> delete(int id)
        {
            var estudianteitem = await _estudiante.GetFirst(a => a.Id == id);
            if (estudianteitem == null) {
                return false;
            }

             await _estudiante.Delete(id);
            await _estudiante.SaveChangesAsync();

            return true;
        }

        public async Task<Respuesta<MateriasEstudianteDto>> ObtenerMateriasporstudiante(int id)
        {
            Respuesta<MateriasEstudianteDto> result = new Respuesta<MateriasEstudianteDto>();
            result.Resultado = new MateriasEstudianteDto();
            result.Resultado.seleccionadas = new List<ComboDto>();
            result.Resultado.faltantes = new List<ComboDto>();

            var materias = _materiaRepositorio.GetAll( include: c => c.Include(r => r.ProfesorMaterias).ThenInclude(e=>e.Profesor));
            var listaMaterias = materias.Result.Items;

            var estudianteMaterias = _estudianteMateriaRepositorio.GetAll(e => e.EstudianteId == id);
            var listaestudianteMaterias = estudianteMaterias.Result.Items;
            
            List<ComboDto> materiaSeleccionada = new List<ComboDto>();
            List<ComboDto> materiaFaltante = new List<ComboDto>();

            var materiasEstudiante = listaestudianteMaterias.Select(r => r.ProfesorMateriaId).Distinct();

            foreach (var item in listaMaterias)
            {
                ComboDto materia = new ComboDto()
                {
                    Id = item.Id,
                    Nombre =  string.Format("Materia: {0} Docente: {1}" ,item.Nombre , item.ProfesorMaterias.Select(r=>r.Profesor.Nombre).FirstOrDefault() ) 
                };
                if (listaestudianteMaterias.Select(r => r.ProfesorMateriaId).Distinct().Contains( item.Id ))
                {
                    materiaSeleccionada.Add(materia);
                }
                else
                {
                    materiaFaltante.Add(materia);
                }
            }
            result.Resultado = new MateriasEstudianteDto()
            {
                faltantes = materiaFaltante,
                seleccionadas = materiaSeleccionada
            };

            return result;
        }
    }
}

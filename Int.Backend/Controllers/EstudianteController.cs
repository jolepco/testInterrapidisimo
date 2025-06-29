using Int.Backend.Entidades;
using Int.Backend.Modelos;
using Int.Backend.Negocio.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Int.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstudianteController : ControllerBase
    {
        private readonly IEstudianteNegocio _negocio;

        public EstudianteController(IEstudianteNegocio negocio)
        {
            _negocio = negocio;
        }

        [HttpGet]
        public async Task<IActionResult> get()
        {
            var respuesta = await _negocio.obtenerTodos();
            return Ok(respuesta);
        }

        [HttpPost("AddEst")]
        public async Task<IActionResult> post(EstudianteDtoIn estudiante)
        {
            Estudiante est = new Estudiante()
            {
                Email = estudiante.Email,
                Nombre = estudiante.Nombre,
            };
            var respuesta = await _negocio.agregar(est);
            return Ok(respuesta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> put01(int id, EstudianteDtoOut estudiante )
        {
            if(id != estudiante.Id)
            {
                return NotFound();
            }

            Estudiante est = new Estudiante();
            est.Nombre = estudiante.Nombre;
            est.Email = estudiante.Email;
            est.Id = estudiante.Id;

            var aa = await _negocio.actualizar(id, est);
            
            return Ok(aa);
        }

        [HttpPost("actualizar/{id}")]
        public async Task<IActionResult> post(int id, EnvioData materias )
        {
            var respuesta = await _negocio.InscribirMaterias(id, materias);
            return Ok(respuesta);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> getins(int id)
        {
            var respuesta = await _negocio.VerCompaneros(id);
            return Ok(respuesta);
        }


        [HttpGet("Estudianteid/{id}")]
        public async Task<IActionResult> getest(int id)
        {
            var respuesta = await _negocio.obtenerUno(id);
            return Ok(respuesta);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> dele(int id)
        {
            var respuesta = await _negocio.delete(id);
            return Ok(respuesta);
        }

        [HttpGet("Materias/{id}")]
        public async Task<IActionResult> getmaterias(int id)
        {
            var respuesta= await _negocio.ObtenerMateriasporstudiante(id);
            return Ok(respuesta);
        }
    }
}

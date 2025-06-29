using System.Collections.Generic;

namespace Int.Backend.Modelos
{
    public class ComboDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
    }

    public class MateriasEstudianteDto
    {
        public IEnumerable<ComboDto> seleccionadas { get; set; } = null!;
        public IEnumerable<ComboDto> faltantes { get; set; } = null!;
    }
}

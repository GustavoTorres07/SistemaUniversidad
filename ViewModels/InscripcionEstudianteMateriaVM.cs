using SistemaUniversidad.Models;
using System;
using System.Collections.Generic;

namespace SistemaUniversidad.ViewModels
{
    public class InscripcionEstudianteMateriaVM
    {
        public int idEstudiante { get; set; }
        public int numeroLegajo { get; set; }
        public string nombreEstudiante { get; set; }
        public string apellidoEstudiante { get; set; }
        public int dniEstudiante { get; set; }
        public string nombreCarrera { get; set; }


        // Lista de materias agrupadas por ciclo
        public List<MateriaPorCiclo> MateriasPorCiclo { get; set; } = new List<MateriaPorCiclo>();
    }

    public class MateriaInscripcionVM
    {
        public int idMateria { get; set; }
        public string nombreMateria { get; set; }
        public int? codigoMateria { get; set; }
        public string correlativas { get; set; }

        public bool Inscrito { get; set; }

    }

    public class MateriaPorCiclo
    {
        public string NombreCiclo { get; set; }
        public List<MateriaInscripcionVM> Materias { get; set; }
    }
}

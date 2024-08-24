using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;

namespace SistemaUniversidad.Models
{
    public class carreraCLS
    {
        [Key]
        public int idCarrera { get; set; }

        [Required]
        [Display(Name = "Nombre de la Carrera")]
        [StringLength(100, ErrorMessage = "El {0} debe tener entre {2} y {1} caracteres", MinimumLength = 1)]
        public string nombreCarrera { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de ciclos debe ser al menos 1.")]
        public int cantidadCiclo { get; set; }

        public int ciclo_id { get; set; }

        // Relación con materias
        public virtual ICollection<MATERIA> Materias { get; set; }

        // Relación con ciclos
        public virtual ICollection<CICLO> Ciclos { get; set; }
    }
}

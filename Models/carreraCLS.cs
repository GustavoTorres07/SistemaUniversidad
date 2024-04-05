using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;

namespace SistemaUniversidad.Models
{
    public class carreraCLS
    {
        public int idCarrera { get; set; }
        [Required]
        [Display(Name = "Nombre de la Carrera")]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {1} caracteres", MinimumLength = 1)]
        public string nombreCarrera { get; set; }
        [Required]
        [Display(Name = "Cantidad de Ciclos")]
        public int cantidadCiclo { get; set; }
        public virtual ICollection<MATERIA> Materias { get; set; }

    }
}
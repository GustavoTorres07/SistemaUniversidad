using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaUniversidad.Models
{
    public class EstadoProfesorCLS
    {
        public int idEstadoProfesor {  get; set; }
        [Required]
        [Display(Name = "Nombre del Estado")]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {1} caracteres", MinimumLength = 1)]
        public string nombreEstadoProfesor { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public bool estado {  get; set; }   
    }
}

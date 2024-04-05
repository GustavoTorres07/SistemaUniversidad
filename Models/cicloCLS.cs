using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaUniversidad.Models
{
    public class cicloCLS
    {

        public int idCiclo { get; set; }

        [Required]
        [Display(Name = "Nombre del Ciclo")]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {1} caracteres", MinimumLength = 1)]
        public string nombreCiclo { get; set; } 
    }
}
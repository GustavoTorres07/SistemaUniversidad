using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaUniversidad.Models
{
    public class ciudadCLS
    {

        public int idCiudad {  get; set; }

        [Required]
        [Display(Name ="Nombre de la Ciudad")]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {1} caracteres", MinimumLength = 1)]
        public string nombreCiudad { get; set; }    
    }   
}
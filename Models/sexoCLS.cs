using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaUniversidad.Models
{
    public class sexoCLS
    {
        public int idSexo { get; set; }

        [Required]
        [Display(Name = "Nombre del sexo")]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {1} caracteres", MinimumLength = 1)]
        public string nombreSexo { get; set; }
    }
}
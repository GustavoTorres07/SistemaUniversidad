using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaUniversidad.Models
{
    public class EstadoUsuarioCLS
    {
        public int idEstadoUsuario { get; set; }
        [Required]
        [Display(Name = "Nombre del Estado")]
        public string nombreEstadoUsuario { get; set; }
        public bool activo { get; set; }
    }
}
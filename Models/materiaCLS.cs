using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaUniversidad.Models
{
    public class materiaCLS
    {
        public int idMateria { get; set; }
        [Required]
        [Display(Name = "Ciclo de la Materia")]
        public int ciclo_id { get; set; }
        [Required]
        [Display(Name = "Carrera de la Materia")]
        public int carrera_id { get; set; }
        [Required]
        [Display(Name = "Nombre de la Materia")]
        public string nombreMateria { get; set; }
        [Required]
        [Display(Name = "Codigo de Materia")]
        public int codigoMateria { get; set; }
        [Required]
        [Display(Name = "Correlativas")]
        public string correlativas { get; set; }
        public virtual CARRERA Carrera { get; set; }
    }
}
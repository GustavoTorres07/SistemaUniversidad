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
        [Required]
        [Display(Name = "Nombre de la Carrera")]
        public int carrera_id { get; set; }

        public virtual CARRERA CARRERA { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<INSCRIPCIONESTUDIANTE> INSCRIPCIONESTUDIANTE { get; set; }
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MATERIA> MATERIA { get; set; }
    }
}
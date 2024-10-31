using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaUniversidad.Models
{
    public class EstudianteCLS
    {
        public int idEstudiante { get; set; }

        [Required(ErrorMessage = "El número de legajo es obligatorio.")]
        [RegularExpression(@"^\d{7}$", ErrorMessage = "El número de legajo debe tener exactamente 7 dígitos.")]
        [Display(Name = "Número de Legajo")]
        public int numeroLegajo { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        //[StringLength(100, ErrorMessage = "El nombre debe tener al menos {2} caracteres", MinimumLength = 1)]
        public string nombreEstudiante { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        //[StringLength(100, ErrorMessage = "El apellido debe tener al menos {2} caracteres", MinimumLength = 1)]
        public string apellidoEstudiante { get; set; }

        [Required(ErrorMessage = "La edad es obligatorio.")]
        [Display(Name = "Edad")]
        [Range(18, 35, ErrorMessage = "La edad debe estar entre {1} y {2}")]
        public int edadEstudiante { get; set; }

        [Required(ErrorMessage = "El número de DNI es obligatorio.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El número de legajo debe tener exactamente 8 dígitos.")]
        [Display(Name = "DNI")]
        public int dniEstudiante { get; set; }

        [Required]
        [Display(Name = "Ciudad")]
        public int ciudad_id { get; set; }

        [Required]
        [Display(Name = "Sexo")]
        public int sexo_id { get; set; }

        [Required]
        [Display(Name = "Estado del Estudiante")]
        public int estadoEstudiante_id { get; set; }

        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string correoElectronico { get; set; }

        [Required]
        [Display(Name = "Carrera")]
        public int carrera_id { get; set; }

        [Required]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime fechaNacimientoEstudiante { get; set; }


        [Display(Name = "Fecha de Registro")]
        public DateTime fechaRegistroEstudiante { get; set; }

        public bool? activo {  get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [RegularExpression(@"^\s*\d{10}\s*$", ErrorMessage = "El número de teléfono debe tener exactamente 10 dígitos.")]
        [Display(Name = "Número de teléfono")]
        public int? telefono { get; set; }    

        // Propiedades de navegación (opcional)
        public virtual CIUDAD Ciudad { get; set; }
        public virtual SEXO Sexo { get; set; }
        public virtual ESTADOESTUDIANTE EstadoEstudiante { get; set; }
        public virtual CARRERA Carrera { get; set; }

        // Colecciones de relaciones (opcional)
        public virtual ICollection<GESTIONMATERIA> GestionMaterias { get; set; }
        //public virtual ICollection<INSCRIPCIONESTUDIANTE> InscripcionesEstudiante { get; set; }
    }
}

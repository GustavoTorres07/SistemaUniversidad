using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaUniversidad.Models
{
    public class UsuarioCLS
    {
        public int idUsuario { get; set; }
     
        [Display(Name = "Nombre de Usuario")]
        [Required(ErrorMessage = "El nombre del Usuario es obligatorio.")]
        public string nombreUsuario { get; set; }

        [Display(Name = "Apelido del Usuario")]
        [Required(ErrorMessage = "El Apellido del Usuario es obligatorio.")]
        public string apellidoUsuario { get; set; }

        [Display(Name = "Edad del Usuario")]
        [Required(ErrorMessage = "La Edad del Usuario es obligatoria.")]
        public int edadUsuario { get; set; }

        [Display(Name = "DNI del Usuario")]
        [Required(ErrorMessage = "El DNI del Usuario es obligatorio.")]
        public int dniUsuario { get; set; }

        [Display(Name = "Nombre de la Ciudad")]
        [Required(ErrorMessage = "Ciudad donde vive el Usuario es obligatoria.")]
        public int ciudad_id { get; set; }

        [Display(Name = "Fecha de Nacimiento del Usuario")]
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        public System.DateTime fechaNacimientoUsuario { get; set; }

        [Display(Name = "Email del Usuario")]
        [Required(ErrorMessage = "El Email es obligatorio.")]
        public string emailUsuario { get; set; }

        [Display(Name = "Sexo del Usuario")]
        [Required(ErrorMessage = "El Sexo es obligatorio y segun DNI.")]
        public int sexo_id { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El Usuario es obligatorio.")]
        public string usuarioUsuario { get; set; }

        [Display(Name = "Clave")]
        [Required(ErrorMessage = "la Clave es obligatoria.")]
        public string claveUsuario { get; set; }

        [Display(Name = "Fecha de Registro")]
        public System.DateTime fechaRegistroUsuario { get; set; }
        [Display(Name = "Rol")]
        public int rol_id { get; set; }

        [Display(Name = "Numero de Celular")]
        public Nullable<int> celularUsuario { get; set; }
        [Display(Name = "Estado")]
        public int estadoUsuario_id { get; set; }

        public bool activo {  get; set; }

        public virtual CIUDAD CIUDAD { get; set; }
        public virtual ESTADOUSUARIO ESTADOUSUARIO { get; set; }
        public virtual ROL ROL { get; set; }
        public virtual SEXO SEXO { get; set; }
    }
}
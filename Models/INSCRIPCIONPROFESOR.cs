//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SistemaUniversidad.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class INSCRIPCIONPROFESOR
    {
        public int idInscripcionProfesor { get; set; }
        public int profesor_id { get; set; }
        public int carrera_id { get; set; }
        public int materia_id { get; set; }
        public System.DateTime fechaInscripcionProfesor { get; set; }
        public int estadoInscripcionProfesor_id { get; set; }
    
        public virtual CARRERA CARRERA { get; set; }
        public virtual ESTADOINSCRIPCIONPROFESOR ESTADOINSCRIPCIONPROFESOR { get; set; }
        public virtual PROFESOR PROFESOR { get; set; }
    }
}

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
    
    public partial class GESTIONMATERIA
    {
        public int idGestionMateria { get; set; }
        public int estudiante_id { get; set; }
        public int profesor_id { get; set; }
        public int materia_id { get; set; }
        public int examen1 { get; set; }
        public Nullable<int> recupExamen1 { get; set; }
        public int examen2 { get; set; }
        public Nullable<int> recupExamen2 { get; set; }
        public int examen3 { get; set; }
        public Nullable<int> recupExamen3 { get; set; }
        public int notaFinalMateria { get; set; }
        public Nullable<int> examenFinal { get; set; }
        public Nullable<int> estadoMateria_id { get; set; }
        public System.DateTime anioCursada { get; set; }
    
        public virtual ESTADOMATERIA ESTADOMATERIA { get; set; }
        public virtual ESTUDIANTE ESTUDIANTE { get; set; }
        public virtual MATERIA MATERIA { get; set; }
        public virtual PROFESOR PROFESOR { get; set; }
    }
}
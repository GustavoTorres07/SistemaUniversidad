﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UniversidadContext : DbContext
    {
        public UniversidadContext()
            : base("name=UniversidadContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CARRERA> CARRERA { get; set; }
        public virtual DbSet<CICLO> CICLO { get; set; }
        public virtual DbSet<CIUDAD> CIUDAD { get; set; }
        public virtual DbSet<ESTADOESTUDIANTE> ESTADOESTUDIANTE { get; set; }
        public virtual DbSet<ESTADOINSCRIPCIONESTUDIANTE> ESTADOINSCRIPCIONESTUDIANTE { get; set; }
        public virtual DbSet<ESTADOINSCRIPCIONPROFESOR> ESTADOINSCRIPCIONPROFESOR { get; set; }
        public virtual DbSet<ESTADOMATERIA> ESTADOMATERIA { get; set; }
        public virtual DbSet<ESTADOUSUARIO> ESTADOUSUARIO { get; set; }
        public virtual DbSet<ESTUDIANTE> ESTUDIANTE { get; set; }
        public virtual DbSet<GESTIONMATERIA> GESTIONMATERIA { get; set; }
        public virtual DbSet<INSCRIPCIONESTUDIANTEMATERIA> INSCRIPCIONESTUDIANTEMATERIA { get; set; }
        public virtual DbSet<INSCRIPCIONPROFESOR> INSCRIPCIONPROFESOR { get; set; }
        public virtual DbSet<MATERIA> MATERIA { get; set; }
        public virtual DbSet<PLANESTUDIO> PLANESTUDIO { get; set; }
        public virtual DbSet<ROL> ROL { get; set; }
        public virtual DbSet<SEXO> SEXO { get; set; }
        public virtual DbSet<USUARIO> USUARIO { get; set; }
    }
}

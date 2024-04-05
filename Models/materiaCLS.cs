using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaUniversidad.Models
{
    public class materiaCLS
    {
        public int idMateria { get; set; }
        public int ciclo_id { get; set; }
        public int carrera_id { get; set; }
        public string nombreMateria { get; set; }
        public int codigoMateria { get; set; }
        public string correlativas { get; set; }
        public virtual CARRERA Carrera { get; set; }
    }
}
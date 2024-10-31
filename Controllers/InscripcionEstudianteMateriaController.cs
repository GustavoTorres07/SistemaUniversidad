using SistemaUniversidad.Models;
using SistemaUniversidad.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Configuration;

namespace SistemaUniversidad.Controllers
{
    public class InscripcionEstudianteMateriaController : BaseController
    {
        private readonly UniversidadContext db = new UniversidadContext();

        public ActionResult InscribirEstudianteMateria(int idEstudiante)
        {
            var estudiante = db.ESTUDIANTE
                .Include(e => e.CARRERA)
                .SingleOrDefault(e => e.idEstudiante == idEstudiante);

            if (estudiante == null)
            {
                return HttpNotFound();
            }

            // Obtener los ciclos asociados a la carrera del estudiante
            var ciclos = db.CICLO
                .Where(c => c.CARRERA.idCarrera == estudiante.carrera_id)
                .ToList();

            // Obtener las inscripciones del estudiante
            var materiasInscritasIds = db.INSCRIPCIONESTUDIANTEMATERIA
                .Where(i => i.estudiante_id == idEstudiante)
                .Select(i => i.materia_id)
                .ToList();

            // Obtener las materias asociadas a los ciclos y marcar las inscritas
            var materiasPorCiclo = ciclos.Select(ciclo => new MateriaPorCiclo
            {
                NombreCiclo = ciclo.nombreCiclo,
                Materias = db.MATERIA
                    .Where(m => m.ciclo_id == ciclo.idCiclo)
                    .Select(m => new MateriaInscripcionVM
                    {
                        idMateria = m.idMateria,
                        nombreMateria = m.nombreMateria,
                        codigoMateria = m.codigoMateria,
                        correlativas = m.correlativas,
                        Inscrito = materiasInscritasIds.Contains(m.idMateria) // Verifica si la materia está inscrita
                    })
                    .ToList()
            }).ToList();

            var model = new InscripcionEstudianteMateriaVM
            {
                idEstudiante = idEstudiante,
                MateriasPorCiclo = materiasPorCiclo
            };

            return View(model);
        }



        [HttpPost]
        public ActionResult GuardarInscripciones(int idEstudiante, List<int> selectedMaterias)
        {
            try
            {
                // Verificar que el idEstudiante y selectedMaterias no sean null o invalidos
                if (idEstudiante <= 0 || selectedMaterias == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                // Eliminar inscripciones existentes
                var inscripcionesExistentes = db.INSCRIPCIONESTUDIANTEMATERIA
                    .Where(i => i.estudiante_id == idEstudiante)
                    .ToList();
                db.INSCRIPCIONESTUDIANTEMATERIA.RemoveRange(inscripcionesExistentes);
                db.SaveChanges();

                // Agregar nuevas inscripciones
                foreach (var materiaId in selectedMaterias)
                {
                    var inscripcion = new INSCRIPCIONESTUDIANTEMATERIA
                    {
                        estudiante_id = idEstudiante,
                        materia_id = materiaId,
                        fechaInscripcionEstudiante = DateTime.Now  // Asignar la fecha de inscripción
                    };
                    db.INSCRIPCIONESTUDIANTEMATERIA.Add(inscripcion);
                }
                db.SaveChanges();

                // Redireccionar a DetalleEstudiante
                return RedirectToAction("DetalleEstudiante", "Estudiante", new { idEstudiante = idEstudiante });
            }
            catch (Exception)
            {
                // Manejo del error
                ModelState.AddModelError("", "Hubo un problema al guardar las inscripciones.");
                return View("Error"); // O la vista de error adecuada
            }
        }


        public ActionResult VerInscripciones(int idEstudiante)
        {
            // Obtener el estudiante, incluyendo la carrera asociada
            var estudiante = db.ESTUDIANTE
                .Include(e => e.CARRERA) // Incluir la carrera
                .SingleOrDefault(e => e.idEstudiante == idEstudiante);

            if (estudiante == null)
            {
                return HttpNotFound();
            }

            // Obtener los ciclos asociados a la carrera del estudiante
            var ciclos = db.CICLO
                .Where(c => c.CARRERA.idCarrera == estudiante.carrera_id) // Filtrar por la carrera del estudiante
                .ToList();

            // Obtener las materias asociadas a los ciclos de la carrera del estudiante
            var materiasPorCiclo = ciclos.Select(ciclo => new MateriaPorCiclo
            {
                NombreCiclo = ciclo.nombreCiclo,
                Materias = db.MATERIA
                    .Where(m => m.ciclo_id == ciclo.idCiclo && m.carrera_id == estudiante.carrera_id) // Filtrar por la carrera del estudiante
                    .Select(m => new MateriaInscripcionVM
                    {
                        idMateria = m.idMateria,
                        correlativas = m.correlativas,
                        codigoMateria = m.codigoMateria,
                        nombreMateria = m.nombreMateria,
                        Inscrito = db.INSCRIPCIONESTUDIANTEMATERIA
                            .Any(i => i.estudiante_id == idEstudiante && i.materia_id == m.idMateria)
                    })
                    .ToList()
            }).ToList();

            // Crear el ViewModel con los datos del estudiante, número de legajo, carrera y materias por ciclo
            var model = new InscripcionEstudianteMateriaVM
            {
                idEstudiante = idEstudiante,
                nombreEstudiante = estudiante.nombreEstudiante,
                numeroLegajo = estudiante.numeroLegajo, // Asegúrate de tener esta propiedad
                nombreCarrera = estudiante.CARRERA.nombreCarrera, 
                apellidoEstudiante = estudiante.apellidoEstudiante,
                dniEstudiante = estudiante.dniEstudiante,
                MateriasPorCiclo = materiasPorCiclo
            };

            return View(model);
        }








    }
}

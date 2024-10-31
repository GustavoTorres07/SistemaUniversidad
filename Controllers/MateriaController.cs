using SistemaUniversidad.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SistemaUniversidad.Controllers
{
    public class MateriaController : BaseController
    {
        private UniversidadContext db = new UniversidadContext();

        // GET: Materia
        public ActionResult Index()
        {
            // Prepara los datos necesarios para la vista.
            ViewBag.Ciclos = new SelectList(db.CICLO, "idCiclo", "nombreCiclo");
            ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera");

            // Obtiene la lista de todas las materias y muestra la vista correspondiente.
            var materias = db.MATERIA.ToList();
            return View(materias);
        }

        // GET: AgregarMateria
        [HttpGet]
        public ActionResult AgregarMateria()
        {
            // Prepara los datos necesarios para la vista.
            ViewBag.Ciclos = new SelectList(db.CICLO, "idCiclo", "nombreCiclo");
            ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera");

            return View();
        }

        // POST: AgregarMateria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarMateria(materiaCLS materia)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verifica si ya existe una materia con el mismo código en la misma carrera.
                    if (db.MATERIA.Any(m => m.codigoMateria == materia.codigoMateria && m.carrera_id == materia.carrera_id))
                    {
                        ModelState.AddModelError("codigoMateria", "Ya existe una materia con este código en esta carrera.");
                    }
                    // Verifica si ya existe una materia con el mismo nombre, ciclo y carrera.
                    else if (db.MATERIA.Any(m => m.nombreMateria == materia.nombreMateria && m.ciclo_id == materia.ciclo_id && m.carrera_id == materia.carrera_id))
                    {
                        ModelState.AddModelError("nombreMateria", "Ya existe una materia con este nombre y ciclo en esta carrera.");
                    }
                    else
                    {
                        // Crea y agrega la nueva materia.
                        var nuevaMateria = new MATERIA
                        {
                            nombreMateria = materia.nombreMateria,
                            ciclo_id = materia.ciclo_id,
                            carrera_id = materia.carrera_id,
                            codigoMateria = materia.codigoMateria,
                            correlativas = materia.correlativas,
                        };
                        db.MATERIA.Add(nuevaMateria);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al agregar la materia: " + ex.Message);
                }
            }

            // Si hay errores, rellenar ViewBag y devolver la vista.
            ViewBag.Ciclos = new SelectList(db.CICLO, "idCiclo", "nombreCiclo", materia.ciclo_id);
            ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera", materia.carrera_id);
            return View(materia);
        }

        // GET: EditarMateria
        [HttpGet]
        public ActionResult EditarMateria(int idMateria)
        {
            try
            {
                var materia = db.MATERIA.Find(idMateria);
                if (materia == null)
                {
                    return HttpNotFound();
                }

                // Prepara los datos necesarios para la vista.
                ViewBag.Ciclos = new SelectList(db.CICLO, "idCiclo", "nombreCiclo", materia.ciclo_id);
                ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera", materia.carrera_id);

                return View(materia);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error al cargar la materia. Por favor, inténtelo de nuevo.");
                return RedirectToAction("Index");
            }
        }

        // POST: EditarMateria
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarMateria(MATERIA materia)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var materiaOriginal = db.MATERIA.Find(materia.idMateria);
                    if (materiaOriginal == null)
                    {
                        return HttpNotFound();
                    }

                    // Verifica si ya existe una materia con el mismo nombre o código.
                    if (db.MATERIA.Any(m => m.nombreMateria == materia.nombreMateria && m.idMateria != materia.idMateria))
                    {
                        ModelState.AddModelError("nombreMateria", "Ya existe una materia con este nombre.");
                    }
                    else if (db.MATERIA.Any(m => m.codigoMateria == materia.codigoMateria && m.idMateria != materia.idMateria))
                    {
                        ModelState.AddModelError("codigoMateria", "Ya existe otra materia con este código.");
                    }
                    else
                    {
                        // Actualiza los datos de la materia y guarda los cambios.
                        materiaOriginal.nombreMateria = materia.nombreMateria;
                        materiaOriginal.ciclo_id = materia.ciclo_id;
                        materiaOriginal.carrera_id = materia.carrera_id;
                        materiaOriginal.codigoMateria = materia.codigoMateria;
                        materiaOriginal.correlativas = materia.correlativas;
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocurrió un error al actualizar la materia. Por favor, inténtelo de nuevo.");
                }
            }

            // Si el modelo no es válido, vuelve al formulario de edición.
            ViewBag.Ciclos = new SelectList(db.CICLO, "idCiclo", "nombreCiclo", materia.ciclo_id);
            ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera", materia.carrera_id);
            return View(materia);
        }

        // GET: EliminarMateria
        public ActionResult EliminarMateria(int idMateria)
        {
            try
            {
                var materia = db.MATERIA.Find(idMateria);
                if (materia == null)
                {
                    return HttpNotFound();
                }

                db.MATERIA.Remove(materia);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar la materia: " + ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}

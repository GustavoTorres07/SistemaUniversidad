using SistemaUniversidad.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SistemaUniversidad.Controllers
{
    public class CicloController : Controller
    {
        private UniversidadContext db = new UniversidadContext();

        // GET: Ciclo
        public ActionResult Index()
        {
            var ciclos = db.CICLO.Include("CARRERA").ToList();
            return View(ciclos);
        }

        [HttpGet]
        public ActionResult AgregarCiclo()
        {
            ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarCiclo(cicloCLS ciclo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.CICLO.Any(c => c.nombreCiclo == ciclo.nombreCiclo && c.carrera_id == ciclo.carrera_id))
                    {
                        ModelState.AddModelError("nombreCiclo", "Ya existe un ciclo con este nombre en esta carrera.");
                    }
                    else
                    {
                        CICLO nuevoCiclo = new CICLO()
                        {
                            nombreCiclo = ciclo.nombreCiclo,
                            carrera_id = ciclo.carrera_id
                        };

                        db.CICLO.Add(nuevoCiclo);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al agregar el ciclo: " + ex.Message);
                }
            }

            ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera", ciclo.carrera_id);
            return View(ciclo);
        }

        [HttpGet]
        public ActionResult EditarCiclo(int idCiclo)
        {
            try
            {
                var ciclo = db.CICLO.Find(idCiclo);
                if (ciclo == null)
                {
                    return HttpNotFound();
                }

                // Convertir la entidad a cicloCLS para el modelo de la vista
                var cicloModel = new cicloCLS
                {
                    idCiclo = ciclo.idCiclo,
                    nombreCiclo = ciclo.nombreCiclo,
                    carrera_id = ciclo.carrera_id
                };

                ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera", ciclo.carrera_id);
                return View(cicloModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCiclo(cicloCLS ciclo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera", ciclo.carrera_id);
                    return View(ciclo);
                }

                var cicloOriginal = db.CICLO.Find(ciclo.idCiclo);
                if (cicloOriginal == null)
                {
                    return HttpNotFound();
                }

                // Verificar si ya existe un ciclo con el mismo nombre en la misma carrera, excluyendo el ciclo actual
                if (db.CICLO.Any(c => c.nombreCiclo == ciclo.nombreCiclo && c.carrera_id == ciclo.carrera_id && c.idCiclo != ciclo.idCiclo))
                {
                    ModelState.AddModelError("nombreCiclo", "Ya existe un ciclo con este nombre en esta carrera.");
                    ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera", ciclo.carrera_id);
                    return View(ciclo);
                }

                cicloOriginal.nombreCiclo = ciclo.nombreCiclo;
                cicloOriginal.carrera_id = ciclo.carrera_id;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarCiclo(int idCiclo)
        {
            try
            {
                var ciclo = db.CICLO.Find(idCiclo);
                if (ciclo == null)
                {
                    return HttpNotFound();
                }

                // Verificar si el ciclo está siendo utilizado por materias
                bool cicloEnUso = db.MATERIA.Any(m => m.ciclo_id == idCiclo);

                if (cicloEnUso)
                {
                    // Redirigir con un mensaje de error si el ciclo está en uso
                    TempData["ErrorMessage"] = "El ciclo no puede ser eliminado porque está en uso en alguna materia.";
                    return RedirectToAction("Index");
                }

                db.CICLO.Remove(ciclo);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y agregar el mensaje de error al modelo
                ModelState.AddModelError("", "Error al eliminar el ciclo: " + ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}

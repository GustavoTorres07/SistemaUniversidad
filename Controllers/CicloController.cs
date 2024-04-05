using SistemaUniversidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaUniversidad.Controllers
{
    public class CicloController : Controller
    {
        private UniversidadContext db = new UniversidadContext();
        // GET: Ciclo
        public ActionResult Index()
        {
            
            return View(db.CICLO.ToList());
        }

        [HttpGet]
        public ActionResult AgregarCiclo()
        {

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]// Añadido por seguridad
        public ActionResult AgregarCiclo(cicloCLS ciclo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe un ciclo con el mismo nombre
                    if (db.CICLO.Any(c => c.nombreCiclo == ciclo.nombreCiclo))
                    {
                        ModelState.AddModelError("nombreCiclo", "Ya existe un Ciclo con este nombre.");
                        return View(ciclo);
                    }
                    // Crear una nueva entidad CICLO a partir del modelo CICLO
                    CICLO nuevoCiclo = new CICLO()
                    {
                        nombreCiclo = ciclo.nombreCiclo,
                    };
                    // Agregar el nuevo ciclo a la base de datos
                    db.CICLO.Add(nuevoCiclo);
                    // Guardar los cambios en la base de datos
                    db.SaveChanges();
                    // Agregar el nuevo ciclo a la base de datos
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción y agregar el mensaje de error al modelo
                    ModelState.AddModelError("", "Error al agregar la ciudad: " + ex.Message);
                    // Volver a mostrar la vista con el modelo ciudadCLS y los errores
                    return View(ciclo);
                }
            }
            // Si el modelo no es válido, volver a mostrar la vista con el modelo ciudadCLS
            return View(ciclo);

        }

        [HttpGet]
        public ActionResult EditarCiclo(int idCiclo)
        {
            try
            {
                using (var db = new UniversidadContext())
                {
                    // Obtener la ciudad a editar
                    var ciclo = db.CICLO.Find(idCiclo);

                    // Si la ciudad no existe, redirigir al índice
                    if (ciclo == null)
                    {
                        return RedirectToAction("Index");
                    }

                    // Devolver la vista con la ciudad a editar
                    return View(ciclo);
                }
            }
            catch (Exception)
            {
                // Manejar la excepción
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCiclo(CICLO ciclo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Si el modelo no es válido, devolver la vista con errores
                    return View(ciclo);
                }

                using (var db = new UniversidadContext())
                {
                    // Buscar la ciudad a editar en la base de datos
                    var cicloOriginal = db.CICLO.Find(ciclo.idCiclo);

                    // Actualizar los datos de la ciudad
                    cicloOriginal.nombreCiclo = ciclo.nombreCiclo;

                    // Guardar los cambios en la base de datos
                    db.SaveChanges();

                    // Redirigir al índice después de la edición exitosa
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                // Manejar la excepción
                throw;
            }
        }

        public ActionResult EliminarCiclo(int idCiclo)
        {
            try
            {
                using (UniversidadContext db = new UniversidadContext())
                {

                    CICLO ciclo = db.CICLO.Find(idCiclo);
                    if (ciclo == null)
                    {

                        return HttpNotFound();
                    }

                    db.CICLO.Remove(ciclo);
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar el Ciclo: " + ex.Message);
                return RedirectToAction("Index");

            }
        }



    }
}
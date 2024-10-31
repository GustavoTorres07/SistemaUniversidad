using SistemaUniversidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaUniversidad.Controllers
{
    public class CarreraController : BaseController
    {
        private UniversidadContext db = new UniversidadContext();

        // GET: Carrera
        public ActionResult Index()
        {
            return View(db.CARRERA.ToList());
        }

        [HttpGet]
        public ActionResult AgregarCarrera()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]// Añadido por seguridad
        public ActionResult AgregarCarrera(carreraCLS carrera)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe una ciudad con el mismo nombre
                    if (db.CARRERA.Any(c => c.nombreCarrera == carrera.nombreCarrera))
                    {
                        ModelState.AddModelError("nombreCarrera", "Ya existe una carrera con este nombre.");
                        return View(carrera);
                    }
                    // Crear una nueva entidad CIUDAD a partir del modelo ciudadCLS
                    CARRERA nuevaCarrera = new CARRERA()
                    {
                        nombreCarrera = carrera.nombreCarrera,
                        cantidadCiclo  = carrera.cantidadCiclo,
                    };
                    // Agregar la nueva ciudad a la base de datos
                    db.CARRERA.Add(nuevaCarrera);
                    // Guardar los cambios en la base de datos
                    db.SaveChanges();
                    // Agregar la nueva ciudad a la base de datos
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción y agregar el mensaje de error al modelo
                    ModelState.AddModelError("", "Error al agregar la carrera: " + ex.Message);
                    // Volver a mostrar la vista con el modelo ciudadCLS y los errores
                    return View(carrera);
                }
            }
            // Si el modelo no es válido, volver a mostrar la vista con el modelo ciudadCLS
            return View(carrera);

        }

        public ActionResult EditarCarrera(int idCarrera)
        {
            try
            {
                using (var db = new UniversidadContext())
                {
                    // Obtener la ciudad a editar
                    var carrera = db.CARRERA.Find(idCarrera);

                    // Si la ciudad no existe, redirigir al índice
                    if (carrera == null)
                    {
                        return RedirectToAction("Index");
                    }

                    // Devolver la vista con la ciudad a editar
                    return View(carrera);
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
        public ActionResult EditarCarrera(carreraCLS carrera)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    
                    return View(carrera);
                }

                using (var db = new UniversidadContext())
                {
                    
                    if (db.CARRERA.Any(c => c.nombreCarrera == carrera.nombreCarrera && c.idCarrera != carrera.idCarrera))
                    {
                        ModelState.AddModelError("nombreCarrera", "Ya existe una carrera con este nombre.");
                        return View(carrera);
                    }

                   
                    var carreraOriginal = db.CARRERA.Find(carrera.idCarrera);

                    carreraOriginal.nombreCarrera = carrera.nombreCarrera;
                    carreraOriginal.cantidadCiclo = carrera.cantidadCiclo;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult EliminarCarrera(int idCarrera)
        {
            try
            {
                using (UniversidadContext db = new UniversidadContext())
                {

                    CARRERA carrera = db.CARRERA.Find(idCarrera);
                    if (carrera == null)
                    {

                        return HttpNotFound();
                    }

                    db.CARRERA.Remove(carrera);
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar la ciudad: " + ex.Message);
                return RedirectToAction("Index");

            }
        }


    }
}
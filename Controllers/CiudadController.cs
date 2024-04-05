using SistemaUniversidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace SistemaUniversidad.Controllers
{
    public class CiudadController : Controller
    {
        private UniversidadContext db = new UniversidadContext();

        // GET: Ciudad
        public ActionResult Index()
        {
            //UniversidadContext db = new UniversidadContext(); 

            return View(db.CIUDAD.ToList());
        }

        [HttpGet]
        public ActionResult AgregarCiudad()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]// Añadido por seguridad
        public ActionResult AgregarCiudad(ciudadCLS ciudad)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe una ciudad con el mismo nombre
                    if (db.CIUDAD.Any(c => c.nombreCiudad == ciudad.nombreCiudad))
                    {
                        ModelState.AddModelError("nombreCiudad", "Ya existe una ciudad con este nombre.");
                        return View(ciudad);
                    }
                    // Crear una nueva entidad CIUDAD a partir del modelo ciudadCLS
                    CIUDAD nuevaCiudad = new CIUDAD()
                    {
                        nombreCiudad = ciudad.nombreCiudad,
                    };
                    // Agregar la nueva ciudad a la base de datos
                    db.CIUDAD.Add(nuevaCiudad);
                    // Guardar los cambios en la base de datos
                    db.SaveChanges();
                    // Agregar la nueva ciudad a la base de datos
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción y agregar el mensaje de error al modelo
                    ModelState.AddModelError("", "Error al agregar la ciudad: " + ex.Message);
                    // Volver a mostrar la vista con el modelo ciudadCLS y los errores
                    return View(ciudad);
                }
            }
            // Si el modelo no es válido, volver a mostrar la vista con el modelo ciudadCLS
            return View(ciudad);

        }

        public ActionResult EditarCiudad(int idCiudad)
        {
            try
            {
                using (var db = new UniversidadContext())
                {
                    // Obtener la ciudad a editar
                    var ciudad = db.CIUDAD.Find(idCiudad);

                    // Si la ciudad no existe, redirigir al índice
                    if (ciudad == null)
                    {
                        return RedirectToAction("Index");
                    }

                    // Devolver la vista con la ciudad a editar
                    return View(ciudad);
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
        public ActionResult EditarCiudad(CIUDAD ciudad)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Si el modelo no es válido, devolver la vista con errores
                    return View(ciudad);
                }

                using (var db = new UniversidadContext())
                {
                    // Buscar la ciudad a editar en la base de datos
                    var ciudadOriginal = db.CIUDAD.Find(ciudad.idCiudad);

                    // Actualizar los datos de la ciudad
                    ciudadOriginal.nombreCiudad = ciudad.nombreCiudad;

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


        public ActionResult EliminarCiudad(int idCiudad)
        {
            try
            {
                using (UniversidadContext db = new UniversidadContext()) {

                    CIUDAD ciudad = db.CIUDAD.Find(idCiudad);
                    if (ciudad == null)
                    {

                        return HttpNotFound();
                    }

                    db.CIUDAD.Remove(ciudad);
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


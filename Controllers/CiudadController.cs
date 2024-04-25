// Importación del espacio de nombres para los modelos
using SistemaUniversidad.Models;

// Importación de los espacios de nombres necesarios
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// Espacio de nombres del controlador
namespace SistemaUniversidad.Controllers
{
    // Definición de la clase del controlador
    public class CiudadController : Controller
    {
        // Creación de una instancia del contexto de la base de datos
        private UniversidadContext db = new UniversidadContext();

        // Método para manejar la petición GET de la vista Index
        public ActionResult Index()
        {
            // Retornar la vista Index con la lista de todas las ciudades en la base de datos
            return View(db.CIUDAD.ToList());
        }

        // Método para manejar la petición GET de la vista para agregar una nueva ciudad
        [HttpGet]
        public ActionResult AgregarCiudad()
        {
            // Retornar la vista para agregar una nueva ciudad
            return View();
        }

        // Método para manejar la petición POST de la vista para agregar una nueva ciudad
        [HttpPost]
        [ValidateAntiForgeryToken] // Añadido por seguridad
        public ActionResult AgregarCiudad(ciudadCLS ciudad)
        {
            // Verificar si el modelo es válido
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe una ciudad con el mismo nombre en la base de datos
                    if (db.CIUDAD.Any(c => c.nombreCiudad == ciudad.nombreCiudad))
                    {
                        ModelState.AddModelError("nombreCiudad", "Ya existe una ciudad con este nombre.");
                        return View(ciudad);
                    }
                    // Crear una nueva entidad de ciudad a partir del modelo recibido
                    CIUDAD nuevaCiudad = new CIUDAD()
                    {
                        nombreCiudad = ciudad.nombreCiudad,
                    };
                    // Agregar la nueva ciudad a la base de datos
                    db.CIUDAD.Add(nuevaCiudad);
                    // Guardar los cambios en la base de datos
                    db.SaveChanges();
                    // Redireccionar al índice después de agregar la ciudad exitosamente
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción y agregar el mensaje de error al modelo
                    ModelState.AddModelError("", "Error al agregar la ciudad: " + ex.Message);
                    // Volver a mostrar la vista con el modelo y los errores
                    return View(ciudad);
                }
            }
            // Si el modelo no es válido, volver a mostrar la vista con el modelo
            return View(ciudad);
        }

        // Método para manejar la petición GET de la vista para editar una ciudad
        public ActionResult EditarCiudad(int idCiudad)
        {
            try
            {
                using (var db = new UniversidadContext())
                {
                    // Obtener la ciudad a editar de la base de datos
                    var ciudad = db.CIUDAD.Find(idCiudad);

                    // Si la ciudad no existe, redirigir al índice
                    if (ciudad == null)
                    {
                        return RedirectToAction("Index");
                    }

                    // Retornar la vista con la ciudad a editar
                    return View(ciudad);
                }
            }
            catch (Exception)
            {
                // Manejar la excepción
                throw;
            }

        }

        // Método para manejar la petición POST de la vista para editar una ciudad
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCiudad(CIUDAD ciudad)
        {
            try
            {
                // Verificar si el modelo no es válido
                if (!ModelState.IsValid)
                {
                    return View(ciudad);
                }

                using (var db = new UniversidadContext())
                {
                    // Buscar la ciudad original en la base de datos
                    var ciudadOriginal = db.CIUDAD.Find(ciudad.idCiudad);

                    // Verificar si ya existe una ciudad con el mismo nombre en la base de datos, excluyendo la ciudad actual
                    if (db.CIUDAD.Any(c => c.nombreCiudad == ciudad.nombreCiudad && c.idCiudad != ciudad.idCiudad))
                    {
                        ModelState.AddModelError("nombreCiudad", "Ya existe una ciudad con este nombre.");
                        return View(ciudad);
                    }

                    // Actualizar los datos de la ciudad original con los datos recibidos
                    ciudadOriginal.nombreCiudad = ciudad.nombreCiudad;

                    // Guardar los cambios en la base de datos
                    db.SaveChanges();

                    // Redireccionar al índice después de editar la ciudad exitosamente
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                // Manejar la excepción
                throw;
            }
        }

        // Método para manejar la petición GET de la vista para eliminar una ciudad
        public ActionResult EliminarCiudad(int idCiudad)
        {
            try
            {
                using (UniversidadContext db = new UniversidadContext())
                {
                    // Obtener la ciudad a eliminar de la base de datos
                    CIUDAD ciudad = db.CIUDAD.Find(idCiudad);
                    // Si la ciudad no existe, retornar un error 404
                    if (ciudad == null)
                    {
                        return HttpNotFound();
                    }
                    // Eliminar la ciudad de la base de datos
                    db.CIUDAD.Remove(ciudad);
                    // Guardar los cambios en la base de datos
                    db.SaveChanges();
                    // Redireccionar al índice después de eliminar la ciudad exitosamente
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y agregar el mensaje de error al modelo
                ModelState.AddModelError("", "Error al eliminar la ciudad: " + ex.Message);
                // Redireccionar al índice
                return RedirectToAction("Index");
            }
        }
    }
}

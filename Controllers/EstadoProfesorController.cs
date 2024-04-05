using SistemaUniversidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaUniversidad.Controllers
{
    public class EstadoProfesorController : Controller
    {
        private UniversidadContext db = new UniversidadContext();

        // GET: EstadoProfesor
        public ActionResult Index()
        {


            return View(db.ESTADOPROFESOR.ToList());

        }

        public ActionResult AgregarEstadoProfesor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarEstadoProfesor(EstadoProfesorCLS estadoProfesor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe una ciudad con el mismo nombre
                    if (db.ESTADOPROFESOR.Any(ep => ep.nombreEstadoProfesor == estadoProfesor.nombreEstadoProfesor))
                    {
                        ModelState.AddModelError("nombreEstadoProfesor", "Ya existe un Estado con este nombre.");
                        return View(estadoProfesor);
                    }
                    // Crear una nueva entidad CIUDAD a partir del modelo ciudadCLS
                    ESTADOPROFESOR nuevoEstadoProfesor = new ESTADOPROFESOR()
                    {
                        nombreEstadoProfesor = estadoProfesor.nombreEstadoProfesor,
                        estado = estadoProfesor.estado
                    };
                    // Agregar la nueva ciudad a la base de datos
                    db.ESTADOPROFESOR.Add(nuevoEstadoProfesor);
                    // Guardar los cambios en la base de datos
                    db.SaveChanges();
                    // cuando guarda los cambio te redirecciona al index
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción y agregar el mensaje de error al modelo
                    ModelState.AddModelError("", "Error al agregar el Estado: " + ex.Message);
                    // Volver a mostrar la vista con el modelo ciudadCLS y los errores
                    return View(estadoProfesor);
                }
            }
            // Si el modelo no es válido, volver a mostrar la vista con el modelo ciudadCLS
            return View(estadoProfesor);
        }

        public ActionResult EditarEstadoProfesor(int idEstadoProfesor)
        {
            try
            {
                using (var db = new UniversidadContext())
                {
                    // Obtener la ciudad a editar
                    var estadoProfesor = db.ESTADOPROFESOR.Find(idEstadoProfesor);

                    // Si la ciudad no existe, redirigir al índice
                    if (estadoProfesor == null)
                    {
                        return RedirectToAction("Index");
                    }

                    // Devolver la vista con la ciudad a editar
                    return View(estadoProfesor);
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
        public ActionResult EditarEstadoProfesor(ESTADOPROFESOR estadoProfesor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Si el modelo no es válido, devolver la vista con errores
                    return View(estadoProfesor);
                }

                using (var db = new UniversidadContext())
                {
                    // Buscar la ciudad a editar en la base de datos
                    var estadoProfesorOriginal = db.ESTADOPROFESOR.Find(estadoProfesor.idEstadoProfesor);

                    // Actualizar los datos del ESTADOAUXILIAR
                    estadoProfesorOriginal.nombreEstadoProfesor = estadoProfesor.nombreEstadoProfesor;
                    estadoProfesorOriginal.estado = estadoProfesor.estado;

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

        public ActionResult EliminarEstadoProfesor(int idEstadoProfesor)
        {
            try
            {
                using (UniversidadContext db = new UniversidadContext())
                {

                    ESTADOPROFESOR estadoProfesor = db.ESTADOPROFESOR.Find(idEstadoProfesor);
                    if (estadoProfesor == null)
                    {

                        return HttpNotFound();
                    }

                    db.ESTADOPROFESOR.Remove(estadoProfesor);
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar el Estado: " + ex.Message);
                return RedirectToAction("Index");

            }
        }

    }
}
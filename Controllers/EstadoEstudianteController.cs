using SistemaUniversidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaUniversidad.Controllers
{
    public class EstadoEstudianteController : Controller
    {
        private UniversidadContext db = new UniversidadContext();

        // GET: EstadoEstudiante
        public ActionResult Index()
        {
            return View(db.ESTADOESTUDIANTE.ToList());
        }

        public ActionResult AgregarEstadoEstudiante()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarEstadoEstudiante(EstadoEstudianteCLS estadoEstudiante)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe una estado con el mismo nombre
                    if (db.ESTADOESTUDIANTE.Any(ee => ee.nombreEstadoEstudiante  == estadoEstudiante.nombreEstadoEstudiante))
                    {
                        ModelState.AddModelError("nombreEstadoEstudiante", "Ya existe un Estado con este nombre.");
                        return View(estadoEstudiante);
                    }
                    // Crear una nueva entidad nuevoEstadoAuxiliar a partir del modelo ciudadCLS
                    ESTADOESTUDIANTE nuevoEstadoEstudiante = new ESTADOESTUDIANTE()
                    {
                        nombreEstadoEstudiante = estadoEstudiante.nombreEstadoEstudiante,
                        estado = estadoEstudiante.estado
                    };
                    // Agregar el nuevo estado a la base de datos
                    db.ESTADOESTUDIANTE.Add(nuevoEstadoEstudiante);
                    // Guardar los cambios en la base de datos
                    db.SaveChanges();
                    // Cuando guarda los cambios te redirecciona al index
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción y agregar el mensaje de error al modelo
                    ModelState.AddModelError("", "Error al agregar el Estado: " + ex.Message);
                    // Volver a mostrar la vista con el modelo ciudadCLS y los errores
                    return View(estadoEstudiante);
                }
            }
            // Si el modelo no es válido, volver a mostrar la vista con el modelo ciudadCLS
            return View(estadoEstudiante);
        }

        public ActionResult EditarEstadoEstudiante(int idEstadoEstudiante)
        {
            try
            {
                using (var db = new UniversidadContext())
                {
                    // Obtener la ciudad a editar
                    var estadoEstudiante = db.ESTADOESTUDIANTE.Find(idEstadoEstudiante);

                    // Si la ciudad no existe, redirigir al índice
                    if (estadoEstudiante == null)
                    {
                        return RedirectToAction("Index");
                    }

                    // Devolver la vista con la ciudad a editar
                    return View(estadoEstudiante);
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
        public ActionResult EditarEstadoEstudiante(ESTADOESTUDIANTE estadoEstudiante)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Si el modelo no es válido, devolver la vista con errores
                    return View(estadoEstudiante);
                }

                using (var db = new UniversidadContext())
                {
                    // Buscar el estado original en la base de datos
                    var estadoEstudianteOriginal = db.ESTADOESTUDIANTE.Find(estadoEstudiante.idEstadoEstudiante);

                    // Verificar si el nuevo nombre del estado es igual al nombre de otro estado existente
                    if (db.ESTADOESTUDIANTE.Any(ee => ee.nombreEstadoEstudiante == estadoEstudiante.nombreEstadoEstudiante && ee.idEstadoEstudiante != estadoEstudiante.idEstadoEstudiante))
                    {
                        ModelState.AddModelError("nombreEstadoEstudiante", "Ya existe un Estado con este nombre.");
                        return View(estadoEstudiante);
                    }

                    // Actualizar los datos del estado
                    estadoEstudianteOriginal.nombreEstadoEstudiante = estadoEstudiante.nombreEstadoEstudiante;
                    estadoEstudianteOriginal.estado = estadoEstudiante.estado;

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

        public ActionResult EliminarEstadoEstudiante(int idEstadoEstudiante)
        {
            try
            {
                using (UniversidadContext db = new UniversidadContext())
                {

                    ESTADOESTUDIANTE estadoEstudiante = db.ESTADOESTUDIANTE.Find(idEstadoEstudiante);
                    if (estadoEstudiante == null)
                    {

                        return HttpNotFound();
                    }

                    db.ESTADOESTUDIANTE.Remove(estadoEstudiante);
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



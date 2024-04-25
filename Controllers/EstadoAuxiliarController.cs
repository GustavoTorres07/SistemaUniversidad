using SistemaUniversidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaUniversidad.Controllers
{
    public class EstadoAuxiliarController : Controller
    {
        private UniversidadContext db = new UniversidadContext();

        // GET: EstadoAuxiliar
        public ActionResult Index()
        {
            return View(db.ESTADOAUXILIAR.ToList());
        }

        public ActionResult AgregarEstadoAuxiliar()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarEstadoAuxiliar(EstadoAuxiliarCLS estadoAuxiliar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe una estado con el mismo nombre
                    if (db.ESTADOAUXILIAR.Any(ea => ea.nombreEstadoAuxiliar == estadoAuxiliar.nombreEstadoAuxiliar))
                    {
                        ModelState.AddModelError("nombreEstadoAuxiliar", "Ya existe un Estado con este nombre.");
                        return View(estadoAuxiliar);
                    }
                    // Crear una nueva entidad nuevoEstadoAuxiliar a partir del modelo ciudadCLS
                    ESTADOAUXILIAR nuevoEstadoAuxiliar = new ESTADOAUXILIAR()
                    {
                        nombreEstadoAuxiliar = estadoAuxiliar.nombreEstadoAuxiliar,
                        estado = estadoAuxiliar.estado
                    };
                    // Agregar el nuevo estado a la base de datos
                    db.ESTADOAUXILIAR.Add(nuevoEstadoAuxiliar);
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
                    return View(estadoAuxiliar);
                }
            }
            // Si el modelo no es válido, volver a mostrar la vista con el modelo ciudadCLS
            return View(estadoAuxiliar);
        }


        public ActionResult EditarEstadoAuxiliar(int idEstadoAuxiliar)
        {
            try
            {
                using (var db = new UniversidadContext())
                {
                    // Obtener la ciudad a editar
                    var estadoAuxiliar = db.ESTADOAUXILIAR.Find(idEstadoAuxiliar);

                    // Si la ciudad no existe, redirigir al índice
                    if (estadoAuxiliar == null)
                    {
                        return RedirectToAction("Index");
                    }

                    // Devolver la vista con la ciudad a editar
                    return View(estadoAuxiliar);
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
        public ActionResult EditarEstadoAuxiliar(ESTADOAUXILIAR estadoAuxiliar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Si el modelo no es válido, devolver la vista con errores
                    return View(estadoAuxiliar);
                }

                using (var db = new UniversidadContext())
                {
                    // Buscar el estado original en la base de datos
                    var estadoAuxiliarOriginal = db.ESTADOAUXILIAR.Find(estadoAuxiliar.idEstadoAuxiliar);

                    // Verificar si el nuevo nombre del estado es igual al nombre de otro estado existente
                    if (db.ESTADOAUXILIAR.Any(ea => ea.nombreEstadoAuxiliar == estadoAuxiliar.nombreEstadoAuxiliar && ea.idEstadoAuxiliar != estadoAuxiliar.idEstadoAuxiliar))
                    {
                        ModelState.AddModelError("nombreEstadoAuxiliar", "Ya existe un Estado con este nombre.");
                        return View(estadoAuxiliar);
                    }

                    // Actualizar los datos del estado
                    estadoAuxiliarOriginal.nombreEstadoAuxiliar = estadoAuxiliar.nombreEstadoAuxiliar;
                    estadoAuxiliarOriginal.estado = estadoAuxiliar.estado;

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



        public ActionResult EliminarEstadoAuxiliar(int idEstadoAuxiliar)
        {
            try
            {
                using (UniversidadContext db = new UniversidadContext())
                {

                    ESTADOAUXILIAR estadoAuxiliar = db.ESTADOAUXILIAR.Find(idEstadoAuxiliar);
                    if (estadoAuxiliar == null)
                    {

                        return HttpNotFound();
                    }

                    db.ESTADOAUXILIAR.Remove(estadoAuxiliar);
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
using SistemaUniversidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaUniversidad.Controllers
{
    public class EstadoUsuarioController : BaseController
    {
        private UniversidadContext db = new UniversidadContext();
        // GET: EstadoUsuario
        public ActionResult Index()
        {
            return View(db.ESTADOUSUARIO.ToList());
        }
        public ActionResult AgregarEstadoUsuario()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarEstadoUsuario(EstadoUsuarioCLS estadoUsuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe un estado con el mismo nombre
                    if (db.ESTADOUSUARIO.Any(eu => eu.nombreEstadoUsuario == estadoUsuario.nombreEstadoUsuario))
                    {
                        ModelState.AddModelError("nombreEstadoUsuario", "Ya existe un Estado con este nombre.");
                        return View(estadoUsuario);
                    }

                    // Crear una nueva entidad ESTADOPROFESOR a partir del modelo EstadoProfesorCLS
                    ESTADOUSUARIO nuevoEstadoUsuario = new ESTADOUSUARIO()
                    {
                        nombreEstadoUsuario = estadoUsuario.nombreEstadoUsuario,
                        activo = estadoUsuario.activo
                    };

                    // Agregar el nuevo estado a la base de datos
                    db.ESTADOUSUARIO.Add(nuevoEstadoUsuario);
                    db.SaveChanges();

                    // Redirigir al índice después de agregar el estado exitosamente
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción y agregar el mensaje de error al modelo
                    ModelState.AddModelError("", "Error al agregar el Estado: " + ex.Message);
                    return View(estadoUsuario);
                }
            }

            return View(estadoUsuario);
        }


        public ActionResult EditarEstadoUsuario(int idEstadoUsuario)
        {
            try
            {
                // Obtener el estado profesor a editar usando el contexto existente
                var estadoUsuario = db.ESTADOUSUARIO.Find(idEstadoUsuario);

                // Si el estado profesor no existe, redirigir al índice
                if (estadoUsuario == null)
                {
                    return RedirectToAction("Index");
                }

                // Mapear el modelo a EstadoUsuarioCLS
                EstadoUsuarioCLS estadoUsuarioCLS = new EstadoUsuarioCLS
                {
                    idEstadoUsuario = estadoUsuario.idEstadoUsuario,
                    nombreEstadoUsuario = estadoUsuario.nombreEstadoUsuario,
                    activo = estadoUsuario.activo
                };

                return View(estadoUsuarioCLS);
            }
            catch (Exception)
            {
                // Manejar la excepción
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEstadoUsuario(EstadoUsuarioCLS estadoUsuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(estadoUsuario);
                }

                // Buscar el estado profesor original en la base de datos
                var estadoUsuarioOriginal = db.ESTADOUSUARIO.Find(estadoUsuario.idEstadoUsuario);

                // Verificar si el nuevo nombre del estado es igual al nombre de otro estado existente
                if (db.ESTADOUSUARIO.Any(eu => eu.nombreEstadoUsuario == estadoUsuario.nombreEstadoUsuario && eu.idEstadoUsuario != estadoUsuario.idEstadoUsuario))
                {
                    ModelState.AddModelError("nombreEstadoUsuario", "Ya existe un Estado con este nombre.");
                    return View(estadoUsuario);
                }

                // Actualizar los datos del estado profesor
                estadoUsuarioOriginal.nombreEstadoUsuario = estadoUsuario.nombreEstadoUsuario;
                estadoUsuarioOriginal.activo = estadoUsuario.activo;

                // Guardar los cambios en la base de datos
                db.SaveChanges();

                // Redirigir al índice después de la edición exitosa
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                // Manejar la excepción
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarEstadoUsuario(int idEstadoUsuario)
        {
            try
            {
                // Obtener el estado profesor a eliminar usando el contexto existente
                var estadoUsuario = db.ESTADOUSUARIO.Find(idEstadoUsuario);

                // Si el estado profesor no existe, retornar un error 404
                if (estadoUsuario == null)
                {
                    return HttpNotFound();
                }

                db.ESTADOUSUARIO.Remove(estadoUsuario);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y agregar el mensaje de error al modelo
                ModelState.AddModelError("", "Error al eliminar el Estado: " + ex.Message);
                return RedirectToAction("Index");
            }
        }









    }
}



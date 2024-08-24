using SistemaUniversidad.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SistemaUniversidad.Controllers
{
    public class RolController : Controller
    {
        private UniversidadContext db = new UniversidadContext();

        // Acción para mostrar la lista de roles
        public ActionResult Index()
        {


            return View(db.ROL.ToList());
        }

        // Acción para mostrar la vista de creación de rol
        [HttpGet]
        public ActionResult AgregarRol()
        {
            var rol = new RolCLS();
            return View(rol);
        }

        // Acción para manejar la creación de un nuevo rol
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarRol(RolCLS rol)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe un rol con el mismo nombre
                    if (db.ROL.Any(r => r.nombreRol == rol.nombreRol))
                    {
                        ModelState.AddModelError("nombreRol", "Ya existe un rol con este nombre.");
                        return View(rol);
                    }

                    var nuevoRol = new ROL
                    {
                        nombreRol = rol.nombreRol,
                        estadoRol = rol.estadoRol
                    };

                    db.ROL.Add(nuevoRol);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al crear el rol: " + ex.Message);
                }
            }
            return View(rol);
        }

        // Acción para mostrar la vista de edición de rol
        [HttpGet]
        public ActionResult EditarRol(int idRol)
        {
            var rol = db.ROL.Find(idRol);
            if (rol == null)
            {
                return HttpNotFound();
            }

            var rolCLS = new RolCLS
            {
                idRol = rol.idRol,
                nombreRol = rol.nombreRol,
                estadoRol = rol.estadoRol
            };

            return View(rolCLS);
        }

        // Acción para manejar la edición de un rol
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarRol(RolCLS rol)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe un rol con el mismo nombre (exceptuando el rol actual)
                    if (db.ROL.Any(r => r.nombreRol == rol.nombreRol && r.idRol != rol.idRol))
                    {
                        ModelState.AddModelError("nombreRol", "Ya existe un rol con este nombre.");
                        return View(rol);
                    }

                    var rolExistente = db.ROL.Find(rol.idRol);
                    rolExistente.nombreRol = rol.nombreRol;
                    rolExistente.estadoRol = rol.estadoRol;

                    db.Entry(rolExistente).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al actualizar el rol: " + ex.Message);
                }
            }
            return View(rol);
        }

        // Acción para manejar la eliminación de un rol
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarRol(int idRol)
        {
            try
            {
                var rol = db.ROL.Find(idRol);
                if (rol == null)
                {
                    return HttpNotFound();
                }
                db.ROL.Remove(rol);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar el rol: " + ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}

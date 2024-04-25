using SistemaUniversidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaUniversidad.Controllers
{
    

    public class SexoController : Controller
    {
        private UniversidadContext db = new UniversidadContext();
        // GET: Sexo
        public ActionResult Index()
        {
            return View(db.SEXO.ToList());
        }

        [HttpGet]
        public ActionResult AgregarSexo()
        {

            return View (); 

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarSexo(sexoCLS sexo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe una ciudad con el mismo nombre
                    if (db.SEXO.Any(s => s.nombreSexo == sexo.nombreSexo))
                    {
                        ModelState.AddModelError("nombreSexo", "Ya existe un Sexo con este nombre.");
                        return View(sexo);
                    }
                    // Crear una nueva entidad CIUDAD a partir del modelo ciudadCLS
                    SEXO nuevoSexo = new SEXO()
                    {
                        nombreSexo = sexo.nombreSexo,
                    };
                    // Agregar la nueva ciudad a la base de datos
                    db.SEXO.Add(nuevoSexo);
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
                    return View(sexo);
                }
            }
            // Si el modelo no es válido, volver a mostrar la vista con el modelo ciudadCLS
            return View(sexo);

        }

        [HttpGet]
        public ActionResult EditarSexo(int idSexo)
        {
            try
            {
                using (var db = new UniversidadContext())
                {
                    // Obtener la ciudad a editar
                    var sexo = db.SEXO.Find(idSexo);

                    // Si la ciudad no existe, redirigir al índice
                    if (sexo == null)
                    {
                        return RedirectToAction("Index");
                    }

                    // Devolver la vista con la ciudad a editar
                    return View(sexo);
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
        public ActionResult EditarSexo(SEXO sexo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // If the model is not valid, return the view with errors
                    return View(sexo);
                }

                using (var db = new UniversidadContext())
                {
                    // Check if the new sex name already exists in the database
                    if (db.SEXO.Any(s => s.nombreSexo == sexo.nombreSexo && s.idSexo != sexo.idSexo))
                    {
                        ModelState.AddModelError("nombreSexo", "Ya existe un sexo con este nombre.");
                        return View(sexo);
                    }

                    // Find the original sex to edit in the database
                    var sexoOriginal = db.SEXO.Find(sexo.idSexo);

                    // Update the sex data
                    sexoOriginal.nombreSexo = sexo.nombreSexo;

                    // Save changes to the database
                    db.SaveChanges();

                    // Redirect to the index after successful editing
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                // Handle the exception
                throw;
            }
        }




        public ActionResult EliminarSexo(int idSexo)
        {
            try
            {
                using (UniversidadContext db = new UniversidadContext())
                {

                    SEXO sexo = db.SEXO.Find(idSexo);
                    if (sexo == null)
                    {

                        return HttpNotFound();
                    }

                    db.SEXO.Remove(sexo);
                    db.SaveChanges();

                    return RedirectToAction("Index");

                }

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar el sexo: " + ex.Message);
                return RedirectToAction("Index");

            }
        }

    }

    
}
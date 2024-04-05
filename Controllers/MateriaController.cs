using SistemaUniversidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaUniversidad.Controllers
{
    public class MateriaController : Controller
    {
        private UniversidadContext db = new UniversidadContext();

        // GET: Materia
        public ActionResult Index()
        {
            // Cargar los ciclos y carreras para usar en el formulario de agregar materia
            ViewBag.Ciclos = new SelectList(db.CICLO.ToList(), "idCiclo", "nombreCiclo");
            ViewBag.Carreras = new SelectList(db.CARRERA.ToList(), "idCarrera", "nombreCarrera");

            return View();
        }

        [HttpGet]
        public ActionResult AgregarMateria()
        {
            // Al acceder a la vista de agregar materia, cargar los ciclos y carreras
            ViewBag.Ciclos = new SelectList(db.CICLO.ToList(), "idCiclo", "nombreCiclo");
            ViewBag.Carreras = new SelectList(db.CARRERA.ToList(), "idCarrera", "nombreCarrera");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Protege el formulario contra ataques CSRF
        public ActionResult AgregarMateria(materiaCLS materia)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si ya existe una materia con el mismo nombre y pertenece a la misma carrera
                    if (db.MATERIA.Any(m => m.nombreMateria == materia.nombreMateria && m.carrera_id == materia.carrera_id))
                    {
                        ModelState.AddModelError("nombreMateria", "Ya existe una materia con este nombre en esta carrera.");
                        // Recargar los ciclos y carreras en caso de error
                        ViewBag.Ciclos = new SelectList(db.CICLO.ToList(), "idCiclo", "nombreCiclo");
                        ViewBag.Carreras = new SelectList(db.CARRERA.ToList(), "idCarrera", "nombreCarrera");
                        return View(materia);
                    }

                    // Crear una nueva entidad MATERIA a partir del modelo materiaCLS
                    MATERIA nuevaMateria = new MATERIA()
                    {
                        nombreMateria = materia.nombreMateria,
                        ciclo_id = materia.ciclo_id,
                        carrera_id = materia.carrera_id,
                        codigoMateria = materia.codigoMateria,
                        correlativas = materia.correlativas,
                    };
                    // Agregar la nueva materia a la base de datos
                    db.MATERIA.Add(nuevaMateria);
                    // Guardar los cambios en la base de datos
                    db.SaveChanges();
                    // Redireccionar al índice después de agregar exitosamente
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Manejar cualquier excepción y agregar el mensaje de error al modelo
                    ModelState.AddModelError("", "Error al agregar la Materia: " + ex.Message);
                    // Recargar los ciclos y carreras en caso de error
                    ViewBag.Ciclos = new SelectList(db.CICLO.ToList(), "idCiclo", "nombreCiclo");
                    ViewBag.Carreras = new SelectList(db.CARRERA.ToList(), "idCarrera", "nombreCarrera");
                    return View(materia);
                }
            }
            // Si el modelo no es válido, volver a mostrar la vista con el modelo materiaCLS
            // Recargar los ciclos y carreras en caso de error
            ViewBag.Ciclos = new SelectList(db.CICLO.ToList(), "idCiclo", "nombreCiclo");
            ViewBag.Carreras = new SelectList(db.CARRERA.ToList(), "idCarrera", "nombreCarrera");
            return View(materia);
        }
    }






















    //public class MateriaController : Controller
    //{
    //    private UniversidadContext db = new UniversidadContext();
    //    // GET: Materia
    //    public ActionResult Index()
    //    {

    //        ViewBag.Ciclos = new SelectList(db.CICLO.ToList(), "idCiclo", "nombreCiclo");
    //        ViewBag.Carreras = new SelectList(db.CARRERA.ToList(), "idCarrera", "nombreCarrera");

    //        //return View(db.MATERIA.ToList());
    //        return View();
    //    }
    //    [HttpGet]
    //    public ActionResult AgregarMateria()
    //    {

    //        return View();

    //    }

    //    [HttpPost]
    //    [ValidateAntiForgeryToken]// Añadido por seguridad
    //    public ActionResult AgregarMateria(materiaCLS materia)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                // Verificar si ya existe una ciudad con el mismo nombre
    //                if (db.MATERIA.Any(m => m.nombreMateria == materia.nombreMateria && m.carrera_id == materia.carrera_id))
    //                {
    //                    ModelState.AddModelError("nombreMateria", "Ya existe una Materia con este nombre y pertenece a una Carrera.");
    //                    return View(materia);
    //                }
    //                // Crear una nueva entidad CIUDAD a partir del modelo ciudadCLS
    //                MATERIA nuevaMateria = new MATERIA()
    //                {
    //                    nombreMateria = materia.nombreMateria,
    //                    ciclo_id = materia.ciclo_id,
    //                    carrera_id = materia.carrera_id,
    //                    codigoMateria = materia.codigoMateria,
    //                    correlativas = materia.correlativas,
    //                };
    //                // Agregar la nueva ciudad a la base de datos
    //                db.MATERIA.Add(nuevaMateria);
    //                // Guardar los cambios en la base de datos
    //                db.SaveChanges();
    //                // Agregar la nueva ciudad a la base de datos
    //                return RedirectToAction("Index");
    //            }
    //            catch (Exception ex)
    //            {
    //                // Manejar cualquier excepción y agregar el mensaje de error al modelo
    //                ModelState.AddModelError("", "Error al agregar la Materia: " + ex.Message);
    //                // Volver a mostrar la vista con el modelo ciudadCLS y los errores
    //                return View(materia);
    //            }
    //        }
    //        // Si el modelo no es válido, volver a mostrar la vista con el modelo ciudadCLS
    //        return View(materia);

    //    }

    //}
}
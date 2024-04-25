using SistemaUniversidad.Models; // Importa el espacio de nombres para acceder a los modelos del sistema.
using System; // Importa el espacio de nombres del sistema para tipos fundamentales y primitivos.
using System.Collections.Generic; // Importa el espacio de nombres para trabajar con colecciones genéricas.
using System.Linq; // Importa el espacio de nombres para utilizar consultas LINQ.
using System.Web; // Importa el espacio de nombres para funcionalidades web.
using System.Web.Mvc; // Importa el espacio de nombres para trabajar con MVC en ASP.NET.

namespace SistemaUniversidad.Controllers // Define el espacio de nombres y el ámbito del controlador.
{
    public class MateriaController : Controller // Define la clase del controlador que hereda de Controller.
    {
        private UniversidadContext db = new UniversidadContext(); // Crea una instancia del contexto de la base de datos de la universidad.

        // GET: Materia
        public ActionResult Index() // Define la acción para mostrar el índice de materias.
        {
            // Prepara los datos necesarios para la vista.
            ViewBag.Ciclos = new SelectList(db.CICLO.ToList(), "idCiclo", "nombreCiclo");
            ViewBag.Carreras = new SelectList(db.CARRERA.ToList(), "idCarrera", "nombreCarrera");

            // Obtiene la lista de todas las materias y muestra la vista correspondiente.
            var materias = db.MATERIA.ToList();
            return View(materias);
        }

        [HttpGet]
        public ActionResult AgregarMateria() // Define la acción para mostrar el formulario de agregar materia.
        {
            // Prepara los datos necesarios para la vista.
            ViewBag.Ciclos = new SelectList(db.CICLO.ToList(), "idCiclo", "nombreCiclo");
            ViewBag.Carreras = new SelectList(db.CARRERA.ToList(), "idCarrera", "nombreCarrera");

            // Muestra el formulario para agregar una nueva materia.
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarMateria(materiaCLS materia) // Define la acción para agregar una nueva materia.
        {
            // Verifica si el modelo es válido.
            if (ModelState.IsValid)
            {
                try
                {
                    // Verifica si ya existe una materia con el mismo código en la misma carrera.
                    if (db.MATERIA.Any(m => m.codigoMateria == materia.codigoMateria && m.carrera_id == materia.carrera_id))
                    {
                        ModelState.AddModelError("codigoMateria", "Ya existe una materia con este código en esta carrera.");
                        ViewBag.Ciclos = new SelectList(db.CICLO.ToList(), "idCiclo", "nombreCiclo");
                        ViewBag.Carreras = new SelectList(db.CARRERA.ToList(), "idCarrera", "nombreCarrera");
                        return View(materia);
                    }

                    // Verifica si ya existe una materia con el mismo nombre, ciclo y carrera.
                    if (db.MATERIA.Any(m => m.nombreMateria == materia.nombreMateria && m.ciclo_id == materia.ciclo_id && m.carrera_id == materia.carrera_id))
                    {
                        ModelState.AddModelError("nombreMateria", "Ya existe una materia con este nombre y ciclo en esta carrera.");
                        ViewBag.Ciclos = new SelectList(db.CICLO.ToList(), "idCiclo", "nombreCiclo");
                        ViewBag.Carreras = new SelectList(db.CARRERA.ToList(), "idCarrera", "nombreCarrera");
                        return View(materia);
                    }

                    // Crea una nueva instancia de materia y la agrega a la base de datos.
                    MATERIA nuevaMateria = new MATERIA()
                    {
                        nombreMateria = materia.nombreMateria,
                        ciclo_id = materia.ciclo_id,
                        carrera_id = materia.carrera_id,
                        codigoMateria = materia.codigoMateria,
                        correlativas = materia.correlativas,
                    };
                    db.MATERIA.Add(nuevaMateria);
                    db.SaveChanges();

                    // Redirige al índice de materias.
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // En caso de error, muestra un mensaje de error y vuelve al formulario.
                    ModelState.AddModelError("", "Error al agregar la Materia: " + ex.Message);
                    ViewBag.Ciclos = new SelectList(db.CICLO.ToList(), "idCiclo", "nombreCiclo");
                    ViewBag.Carreras = new SelectList(db.CARRERA.ToList(), "idCarrera", "nombreCarrera");
                    return View(materia);
                }
            }

            // En caso de modelo no válido, vuelve al formulario con los datos necesarios.
            ViewBag.Ciclos = new SelectList(db.CICLO.ToList(), "idCiclo", "nombreCiclo");
            ViewBag.Carreras = new SelectList(db.CARRERA.ToList(), "idCarrera", "nombreCarrera");
            return View(materia);
        }

        // Define la acción para editar una materia.
        public ActionResult EditarMateria(int idMateria)
        {
            try
            {
                // Intenta cargar la materia desde la base de datos.
                using (var db = new UniversidadContext())
                {
                    var materia = db.MATERIA.Find(idMateria);
                    if (materia == null)
                    {
                        // Si la materia no existe, redirige al índice de materias.
                        return RedirectToAction("Index");
                    }

                    // Obtiene información adicional necesaria para la vista.
                    var nombreCarrera = db.CARRERA.Where(c => c.idCarrera == materia.carrera_id).Select(c => c.nombreCarrera).FirstOrDefault();
                    var ciclos = db.CICLO.Select(c => new SelectListItem { Value = c.idCiclo.ToString(), Text = c.nombreCiclo }).ToList();
                    ViewBag.Ciclos = ciclos;
                    ViewBag.NombreCarrera = nombreCarrera;
                    var carreras = db.CARRERA.Select(c => new SelectListItem { Value = c.idCarrera.ToString(), Text = c.nombreCarrera }).ToList();
                    ViewBag.Carreras = carreras;

                    // Muestra la vista para editar la materia.
                    return View(materia);
                }
            }
            catch (Exception)
            {
                // En caso de error, muestra un mensaje de error y redirige al índice de materias.
                ModelState.AddModelError("", "Ocurrió un error al cargar la materia. Por favor, inténtelo de nuevo.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarMateria(MATERIA materia) // Define la acción para guardar los cambios en una materia editada.
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // Si el modelo no es válido, vuelve al formulario de edición.
                    return View(materia);
                }

                using (var db = new UniversidadContext())
                {
                    // Obtiene la materia original desde la base de datos.
                    var materiaOriginal = db.MATERIA.Find(materia.idMateria);

                    // Verifica si ya existe una materia con el mismo nombre.
                    if (db.MATERIA.Any(m => m.nombreMateria == materia.nombreMateria && m.idMateria != materia.idMateria))
                    {
                        ModelState.AddModelError("nombreMateria", "Ya existe una materia con este nombre.");
                        return View(materia);
                    }

                    // Verifica si ya existe una materia con el mismo código.
                    if (db.MATERIA.Any(m => m.codigoMateria == materia.codigoMateria && m.idMateria != materia.idMateria))
                    {
                        ModelState.AddModelError("codigoMateria", "Ya existe otra materia con este código.");
                        return View(materia);
                    }

                    // Actualiza los datos de la materia original con los nuevos datos y guarda los cambios.
                    materiaOriginal.ciclo_id = materia.ciclo_id;
                    materiaOriginal.nombreMateria = materia.nombreMateria;
                    materiaOriginal.carrera_id = materia.carrera_id;
                    materiaOriginal.codigoMateria = materia.codigoMateria;
                    materiaOriginal.correlativas = materia.correlativas;
                    db.SaveChanges();

                    // Redirige al índice de materias.
                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                // En caso de error, lanza una excepción.
                throw;
            }
        }

        // Define la acción para eliminar una materia.
        public ActionResult EliminarMateria(int idMateria)
        {
            try
            {
                // Intenta eliminar la materia desde la base de datos.
                using (UniversidadContext db = new UniversidadContext())
                {
                    MATERIA materia = db.MATERIA.Find(idMateria);
                    if (materia == null)
                    {
                        // Si la materia no existe, devuelve un error 404.
                        return HttpNotFound();
                    }

                    // Elimina la materia de la base de datos y guarda los cambios.
                    db.MATERIA.Remove(materia);
                    db.SaveChanges();

                    // Redirige al índice de materias.
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // En caso de error, muestra un mensaje de error y redirige al índice de materias.
                ModelState.AddModelError("", "Error al eliminar la materia: " + ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}

using SistemaUniversidad.Models;
using System;
using System.Linq;
using PagedList; // Asegúrate de importar el espacio de nombres PagedList
using System.Web.Mvc;
using System.Data.Entity;

namespace SistemaUniversidad.Controllers
{
    public class EstudianteController : BaseController
    {
        private readonly UniversidadContext db = new UniversidadContext();

        // GET: Estudiante
        public ActionResult Index(string search = "", bool? toggle = false, int? page = 1)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5; // Número de registros por página

            var estudiantes = db.ESTUDIANTE.AsQueryable();

            // Filtrar los estudiantes según la búsqueda
            if (!string.IsNullOrEmpty(search))
            {
                if (toggle.HasValue && toggle.Value)
                {
                    // Buscar por DNI
                    estudiantes = estudiantes.Where(e => e.dniEstudiante.ToString().Contains(search));
                }
                else
                {
                    // Buscar por Legajo
                    estudiantes = estudiantes.Where(e => e.numeroLegajo.ToString().Contains(search));
                }
            }

            // Asegúrate de ordenar los datos antes de paginar
            estudiantes = estudiantes.OrderBy(e => e.numeroLegajo); // O cualquier otra propiedad clave

            // Obtener los estudiantes paginados
            var paginatedEstudiantes = estudiantes.ToPagedList(pageNumber, pageSize);

            // Preparar los datos necesarios para la vista.
            ViewBag.Ciudades = new SelectList(db.CIUDAD, "idCiudad", "nombreCiudad");
            ViewBag.Sexos = new SelectList(db.SEXO, "idSexo", "nombreSexo");
            ViewBag.EstadosEstudiante = new SelectList(db.ESTADOESTUDIANTE, "idEstadoEstudiante", "nombreEstadoEstudiante");
            ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera");

            // Mostrar la vista con los estudiantes paginados.
            return View(paginatedEstudiantes);


        }

        // GET: AgregarEstudiante
        [HttpGet]
        public ActionResult AgregarEstudiante()
        {
            // Prepara los datos necesarios para la vista.
            ViewBag.Ciudades = new SelectList(db.CIUDAD, "idCiudad", "nombreCiudad");
            ViewBag.Sexos = new SelectList(db.SEXO, "idSexo", "nombreSexo");
            ViewBag.EstadosEstudiante = new SelectList(db.ESTADOESTUDIANTE, "idEstadoEstudiante", "nombreEstadoEstudiante");
            ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera");

            return View();
        }

        // POST: AgregarEstudiante
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarEstudiante(EstudianteCLS estudiante)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verifica si ya existe un estudiante con el mismo DNI.
                    if (db.ESTUDIANTE.Any(e => e.dniEstudiante == estudiante.dniEstudiante))
                    {
                        ModelState.AddModelError("dniEstudiante", "Ya existe un estudiante con este DNI.");
                    }
                    else
                    {
                        // Crea y agrega el nuevo estudiante.
                        var nuevoEstudiante = new ESTUDIANTE
                        {
                            numeroLegajo = estudiante.numeroLegajo,
                            nombreEstudiante = estudiante.nombreEstudiante,
                            apellidoEstudiante = estudiante.apellidoEstudiante,
                            edadEstudiante = estudiante.edadEstudiante,
                            dniEstudiante = estudiante.dniEstudiante,
                            ciudad_id = estudiante.ciudad_id,
                            sexo_id = estudiante.sexo_id,
                            estadoEstudiante_id = estudiante.estadoEstudiante_id,
                            correoElectronico = estudiante.correoElectronico,
                            carrera_id = estudiante.carrera_id,
                            fechaNacimientoEstudiante = estudiante.fechaNacimientoEstudiante,
                            fechaRegistroEstudiante = DateTime.Now,
                            activo = estudiante.activo,
                            telefono = estudiante.telefono
                        };
                        db.ESTUDIANTE.Add(nuevoEstudiante);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al agregar el estudiante: " + ex.Message);
                }
            }

            // Si hay errores, rellenar ViewBag y devolver la vista.
            ViewBag.Ciudades = new SelectList(db.CIUDAD, "idCiudad", "nombreCiudad", estudiante.ciudad_id);
            ViewBag.Sexos = new SelectList(db.SEXO, "idSexo", "nombreSexo", estudiante.sexo_id);
            ViewBag.EstadosEstudiante = new SelectList(db.ESTADOESTUDIANTE, "idEstadoEstudiante", "nombreEstadoEstudiante", estudiante.estadoEstudiante_id);
            ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera", estudiante.carrera_id);
            return View(estudiante);
        }

        // GET: EditarEstudiante
        [HttpGet]
        public ActionResult EditarEstudiante(int idEstudiante)
        {
            try
            {
                var estudiante = db.ESTUDIANTE.Find(idEstudiante);
                if (estudiante == null)
                {
                    return HttpNotFound();
                }

                // Prepara los datos necesarios para la vista.
                ViewBag.Ciudades = new SelectList(db.CIUDAD, "idCiudad", "nombreCiudad", estudiante.ciudad_id);
                ViewBag.Sexos = new SelectList(db.SEXO, "idSexo", "nombreSexo", estudiante.sexo_id);
                ViewBag.EstadosEstudiante = new SelectList(db.ESTADOESTUDIANTE, "idEstadoEstudiante", "nombreEstadoEstudiante", estudiante.estadoEstudiante_id);
                ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera", estudiante.carrera_id);

                // Mapear la entidad a la clase modelo.
                var estudianteCLS = new EstudianteCLS
                {
                    idEstudiante = estudiante.idEstudiante,
                    numeroLegajo = estudiante.numeroLegajo,
                    nombreEstudiante = estudiante.nombreEstudiante,
                    apellidoEstudiante = estudiante.apellidoEstudiante,
                    edadEstudiante = estudiante.edadEstudiante,
                    dniEstudiante = estudiante.dniEstudiante,
                    ciudad_id = estudiante.ciudad_id,
                    sexo_id = estudiante.sexo_id,
                    estadoEstudiante_id = estudiante.estadoEstudiante_id,
                    correoElectronico = estudiante.correoElectronico,
                    carrera_id = estudiante.carrera_id,
                    fechaNacimientoEstudiante = estudiante.fechaNacimientoEstudiante,
                    fechaRegistroEstudiante = estudiante.fechaRegistroEstudiante,
                    activo = estudiante.activo,
                    telefono = estudiante.telefono
                };

                return View(estudianteCLS);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error al cargar el estudiante. Por favor, inténtelo de nuevo.");
                return RedirectToAction("Index");
            }
        }

        // POST: EditarEstudiante
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEstudiante(EstudianteCLS estudiante)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var estudianteOriginal = db.ESTUDIANTE.Find(estudiante.idEstudiante);
                    if (estudianteOriginal == null)
                    {
                        return HttpNotFound();
                    }

                    // Verifica si ya existe un estudiante con el mismo DNI.
                    if (db.ESTUDIANTE.Any(e => e.dniEstudiante == estudiante.dniEstudiante && e.idEstudiante != estudiante.idEstudiante))
                    {
                        ModelState.AddModelError("dniEstudiante", "Ya existe un estudiante con este DNI.");
                    }
                    else
                    {
                        // Actualiza los datos del estudiante y guarda los cambios.
                        estudianteOriginal.numeroLegajo = estudiante.numeroLegajo;
                        estudianteOriginal.nombreEstudiante = estudiante.nombreEstudiante;
                        estudianteOriginal.apellidoEstudiante = estudiante.apellidoEstudiante;
                        estudianteOriginal.edadEstudiante = estudiante.edadEstudiante;
                        estudianteOriginal.dniEstudiante = estudiante.dniEstudiante;
                        estudianteOriginal.ciudad_id = estudiante.ciudad_id;
                        estudianteOriginal.sexo_id = estudiante.sexo_id;
                        estudianteOriginal.estadoEstudiante_id = estudiante.estadoEstudiante_id;
                        estudianteOriginal.correoElectronico = estudiante.correoElectronico;
                        estudianteOriginal.fechaNacimientoEstudiante = estudiante.fechaNacimientoEstudiante;
                        estudianteOriginal.carrera_id = estudiante.carrera_id;
                        estudianteOriginal.activo = estudiante.activo;
                        estudianteOriginal.telefono = estudiante.telefono;  

                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    // Agregar detalles del error al ModelState para depuración.
                    ModelState.AddModelError("", $"Ocurrió un error al actualizar el estudiante: {ex.Message}");
                }
            }

            // Re-cargar datos para dropdowns en caso de error.
            ViewBag.Ciudades = new SelectList(db.CIUDAD, "idCiudad", "nombreCiudad", estudiante.ciudad_id);
            ViewBag.Sexos = new SelectList(db.SEXO, "idSexo", "nombreSexo", estudiante.sexo_id);
            ViewBag.EstadosEstudiante = new SelectList(db.ESTADOESTUDIANTE, "idEstadoEstudiante", "nombreEstadoEstudiante", estudiante.estadoEstudiante_id);
            ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera", estudiante.carrera_id);

            return View(estudiante);
        }

        // GET: Edit
        [HttpGet]
        public ActionResult DetalleEstudiante(int idEstudiante)
        {
            try
            {
                var estudiante = db.ESTUDIANTE
                    .Include(e => e.CIUDAD)
                    .Include(e => e.SEXO)
                    .Include(e => e.ESTADOESTUDIANTE)
                    .Include(e => e.CARRERA)
                    .SingleOrDefault(e => e.idEstudiante == idEstudiante);

                if (estudiante == null)
                {
                    return HttpNotFound();
                }

                // Preparar los datos para los campos desplegables
                ViewBag.Ciudades = new SelectList(db.CIUDAD, "idCiudad", "nombreCiudad", estudiante.ciudad_id);
                ViewBag.Sexos = new SelectList(db.SEXO, "idSexo", "nombreSexo", estudiante.sexo_id);
                ViewBag.EstadosEstudiante = new SelectList(db.ESTADOESTUDIANTE, "idEstadoEstudiante", "nombreEstadoEstudiante", estudiante.estadoEstudiante_id);
                ViewBag.Carreras = new SelectList(db.CARRERA, "idCarrera", "nombreCarrera", estudiante.carrera_id);

                // Retorna la entidad `ESTUDIANTE` a la vista
                return View(estudiante);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ocurrió un error al cargar el estudiante: " + ex.Message);
                return RedirectToAction("Index");
            }
        }

        // GET: EliminarEstudiante
        [HttpGet]
        public ActionResult EliminarEstudiante(int idEstudiante)
        {
            try
            {
                var estudiante = db.ESTUDIANTE.Find(idEstudiante);
                if (estudiante == null)
                {
                    return HttpNotFound();
                }

                // Mapear la entidad a la clase modelo.
                var estudianteCLS = new EstudianteCLS
                {
                    idEstudiante = estudiante.idEstudiante,
                    numeroLegajo = estudiante.numeroLegajo,
                    nombreEstudiante = estudiante.nombreEstudiante,
                    apellidoEstudiante = estudiante.apellidoEstudiante,
                    edadEstudiante = estudiante.edadEstudiante,
                    dniEstudiante = estudiante.dniEstudiante,
                    ciudad_id = estudiante.ciudad_id,
                    sexo_id = estudiante.sexo_id,
                    estadoEstudiante_id = estudiante.estadoEstudiante_id,
                    correoElectronico = estudiante.correoElectronico,
                    carrera_id = estudiante.carrera_id,
                    fechaNacimientoEstudiante = estudiante.fechaNacimientoEstudiante,
                    fechaRegistroEstudiante = estudiante.fechaRegistroEstudiante,
                    activo = estudiante.activo,
                    telefono = estudiante.telefono
                };

                return View(estudianteCLS);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Ocurrió un error al cargar el estudiante. Por favor, inténtelo de nuevo.");
                return RedirectToAction("Index");
            }
        }

        // POST: EliminarEstudiante
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarEstudiante(EstudianteCLS estudiante)
        {
            try
            {
                var estudianteOriginal = db.ESTUDIANTE.Find(estudiante.idEstudiante);
                if (estudianteOriginal == null)
                {
                    return HttpNotFound();
                }

                // Elimina el estudiante.
                db.ESTUDIANTE.Remove(estudianteOriginal);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ocurrió un error al eliminar el estudiante: {ex.Message}");
                return View(estudiante);
            }
        }
    }
}

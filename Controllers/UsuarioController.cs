using SistemaUniversidad.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using BCrypt.Net;

namespace SistemaUniversidad.Controllers
{
    public class UsuarioController : Controller
    {
        private UniversidadContext db = new UniversidadContext();

        // GET: Usuario
        public ActionResult Index()
        {
            var usuarios = db.USUARIO
                .Include("CIUDAD")
                .Include("SEXO")
                .Include("ROL")
                .Include("ESTADOUSUARIO")
                .ToList();

            return View(usuarios);
        }

        // GET: AgregarUsuario
        [HttpGet]
        public ActionResult AgregarUsuario()
        {
            RellenarViewBag();
            return View();
        }

        // POST: AgregarUsuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarUsuario(UsuarioCLS usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.USUARIO.Any(u => u.dniUsuario == usuario.dniUsuario))
                    {
                        ModelState.AddModelError("dniUsuario", "Ya existe un usuario con este DNI.");
                    }
                    if (db.USUARIO.Any(u => u.emailUsuario == usuario.emailUsuario))
                    {
                        ModelState.AddModelError("emailUsuario", "Ya existe un usuario con este correo electrónico.");
                    }

                    if (!ModelState.IsValid)
                    {
                        RellenarViewBag(usuario);
                        return View(usuario);
                    }

                    var nuevoUsuario = new USUARIO
                    {
                        nombreUsuario = usuario.nombreUsuario,
                        apellidoUsuario = usuario.apellidoUsuario,
                        edadUsuario = usuario.edadUsuario,
                        dniUsuario = usuario.dniUsuario,
                        ciudad_id = usuario.ciudad_id,
                        sexo_id = usuario.sexo_id,
                        rol_id = usuario.rol_id,
                        estadoUsuario_id = usuario.estadoUsuario_id,
                        emailUsuario = usuario.emailUsuario,
                        usuarioUsuario = usuario.usuarioUsuario,
                        claveUsuario = HashPassword(usuario.claveUsuario),
                        celularUsuario = usuario.celularUsuario,
                        fechaNacimientoUsuario = usuario.fechaNacimientoUsuario.Date,
                        activo = usuario.activo,
                        fechaRegistroUsuario = DateTime.Now
                    };

                    db.USUARIO.Add(nuevoUsuario);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }

                    RellenarViewBag(usuario);
                    return View(usuario);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al agregar el usuario: " + ex.Message);
                    RellenarViewBag(usuario);
                    return View(usuario);
                }
            }

            RellenarViewBag(usuario);
            return View(usuario);
        }

        // GET: EditarUsuario
        [HttpGet]
        public ActionResult EditarUsuario(int idUsuario)
        {
            var usuario = db.USUARIO.Find(idUsuario);
            if (usuario == null)
            {
                return HttpNotFound();
            }

            var usuarioCLS = new UsuarioCLS
            {
                idUsuario = usuario.idUsuario,
                nombreUsuario = usuario.nombreUsuario,
                apellidoUsuario = usuario.apellidoUsuario,
                edadUsuario = usuario.edadUsuario,
                dniUsuario = usuario.dniUsuario,
                ciudad_id = usuario.ciudad_id,
                sexo_id = usuario.sexo_id,
                rol_id = usuario.rol_id,
                estadoUsuario_id = usuario.estadoUsuario_id,
                emailUsuario = usuario.emailUsuario,
                usuarioUsuario = usuario.usuarioUsuario,
                claveUsuario = "",
                celularUsuario = usuario.celularUsuario,
                fechaNacimientoUsuario = usuario.fechaNacimientoUsuario,
                activo = usuario.activo
            };

            RellenarViewBag(usuarioCLS);

            return View(usuarioCLS);
        }

        // POST: EditarUsuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(UsuarioCLS usuario)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = db.USUARIO.Find(usuario.idUsuario);
                if (usuarioExistente == null)
                {
                    return HttpNotFound();
                }

                if (db.USUARIO.Any(u => u.dniUsuario == usuario.dniUsuario && u.idUsuario != usuario.idUsuario))
                {
                    ModelState.AddModelError("dniUsuario", "Ya existe un usuario con este DNI.");
                }
                if (db.USUARIO.Any(u => u.emailUsuario == usuario.emailUsuario && u.idUsuario != usuario.idUsuario))
                {
                    ModelState.AddModelError("emailUsuario", "Ya existe un usuario con este correo electrónico.");
                }

                if (ModelState.IsValid)
                {
                    usuarioExistente.nombreUsuario = usuario.nombreUsuario;
                    usuarioExistente.apellidoUsuario = usuario.apellidoUsuario;
                    usuarioExistente.edadUsuario = usuario.edadUsuario;
                    usuarioExistente.dniUsuario = usuario.dniUsuario;
                    usuarioExistente.ciudad_id = usuario.ciudad_id;
                    usuarioExistente.sexo_id = usuario.sexo_id;
                    usuarioExistente.rol_id = usuario.rol_id;
                    usuarioExistente.estadoUsuario_id = usuario.estadoUsuario_id;
                    usuarioExistente.emailUsuario = usuario.emailUsuario;
                    usuarioExistente.usuarioUsuario = usuario.usuarioUsuario;

                    if (!string.IsNullOrEmpty(usuario.claveUsuario))
                    {
                        usuarioExistente.claveUsuario = HashPassword(usuario.claveUsuario);
                    }

                    usuarioExistente.celularUsuario = usuario.celularUsuario;
                    usuarioExistente.fechaNacimientoUsuario = usuario.fechaNacimientoUsuario.Date;
                    usuarioExistente.activo = usuario.activo;

                    db.Entry(usuarioExistente).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            RellenarViewBag(usuario);
            return View(usuario);
        }

        // POST: EliminarUsuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUsuario(int idUsuario)
        {
            var usuario = db.USUARIO.Find(idUsuario);

            if (usuario == null)
            {
                return HttpNotFound();
            }

            db.USUARIO.Remove(usuario);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Método privado para rellenar los ViewBag para los dropdowns en las vistas
        private void RellenarViewBag(UsuarioCLS usuario = null)
        {
            ViewBag.Ciudades = new SelectList(db.CIUDAD, "idCiudad", "nombreCiudad", usuario?.ciudad_id);
            ViewBag.Sexos = new SelectList(db.SEXO, "idSexo", "nombreSexo", usuario?.sexo_id);

            var rolesActivos = db.ROL.Where(r => r.estadoRol).ToList();
            ViewBag.Roles = new SelectList(rolesActivos, "idRol", "nombreRol", usuario?.rol_id);

            ViewBag.Estados = new SelectList(db.ESTADOUSUARIO, "idEstadoUsuario", "nombreEstadoUsuario", usuario?.estadoUsuario_id);
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("La contraseña no puede ser nula o vacía.", nameof(password));
            }

            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public ActionResult FiltroRoles()
        {
            var rolesActivos = db.ROL.Where(r => r.estadoRol).ToList();
            ViewBag.Roles = new SelectList(rolesActivos, "idRol", "nombreRol");
            return View();
        }
    }
}

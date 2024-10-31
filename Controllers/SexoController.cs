using SistemaUniversidad.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SistemaUniversidad.Filters;

namespace SistemaUniversidad.Controllers
{
    [CustomAuthorize("Administrador","Auxiliar")]
    public class SexoController : BaseController
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarSexo(sexoCLS sexo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.SEXO.Any(s => s.nombreSexo == sexo.nombreSexo))
                    {
                        ModelState.AddModelError("nombreSexo", "Ya existe un Sexo con este nombre.");
                        return View(sexo);
                    }
                    SEXO nuevoSexo = new SEXO()
                    {
                        nombreSexo = sexo.nombreSexo,
                    };
                    db.SEXO.Add(nuevoSexo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al agregar el sexo: " + ex.Message);
                    return View(sexo);
                }
            }
            return View(sexo);
        }

        [HttpGet]
        public ActionResult EditarSexo(int idSexo)
        {
            try
            {
                var sexo = db.SEXO.Find(idSexo);

                if (sexo == null)
                {
                    return RedirectToAction("Index");
                }

                return View(sexo);
            }
            catch (Exception)
            {
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
                    return View(sexo);
                }

                if (db.SEXO.Any(s => s.nombreSexo == sexo.nombreSexo && s.idSexo != sexo.idSexo))
                {
                    ModelState.AddModelError("nombreSexo", "Ya existe un sexo con este nombre.");
                    return View(sexo);
                }

                var sexoOriginal = db.SEXO.Find(sexo.idSexo);
                sexoOriginal.nombreSexo = sexo.nombreSexo;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult EliminarSexo(int idSexo)
        {
            try
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
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar el sexo: " + ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}

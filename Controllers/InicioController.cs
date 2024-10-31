using System.Linq;
using System.Web.Mvc;
using System.Web.Security; // Asegúrate de incluir esto para usar FormsAuthentication
using SistemaUniversidad.Models;

namespace SistemaUniversidad.Controllers
{
    public class InicioController : Controller
    {
        private UniversidadContext db = new UniversidadContext();

        // GET: Inicio
        public ActionResult Index()
        {
            // Obtener el nombre del usuario actual desde la autenticación
            var usuarioNombre = User.Identity.Name;

            if (string.IsNullOrEmpty(usuarioNombre))
            {
                // Redirigir al login si el usuario no está autenticado
                return RedirectToAction("Login", "Acceso");
            }

            // Obtener la información del usuario desde la base de datos
            var usuario = db.USUARIO.SingleOrDefault(u => u.usuarioUsuario == usuarioNombre);

            if (usuario == null)
            {
                // Manejar el caso en que el usuario no se encuentra
                return HttpNotFound();
            }

            // Pasar la información a la vista usando ViewBag
            ViewBag.Nombre = usuario.nombreUsuario;
            ViewBag.Apellido = usuario.apellidoUsuario;
            ViewBag.NombreUsuario = usuario.usuarioUsuario;
            ViewBag.Rol = usuario.ROL.nombreRol;

            return View();
        }
    }
}

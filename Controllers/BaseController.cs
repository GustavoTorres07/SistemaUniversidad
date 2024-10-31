using System.Linq;
using System.Web.Mvc;
using SistemaUniversidad.Models;

namespace SistemaUniversidad.Controllers
{
    public class BaseController : Controller
    {
        private UniversidadContext db = new UniversidadContext();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // Obtener el nombre del usuario actual desde la autenticación
            var usuarioNombre = User.Identity.Name;

            if (string.IsNullOrEmpty(usuarioNombre))
            {
                // No hacer nada si el usuario no está autenticado
                ViewBag.NombreUsuario = "Usuario no autenticado";
                ViewBag.Rol = "Sin rol";
                ViewBag.Nombre = "Nombre no disponible";
                ViewBag.Apellido = "Apellido no disponible";
                ViewBag.EstadoOnline = false;
                return;
            }

            // Obtener la información del usuario desde la base de datos
            var usuario = db.USUARIO.SingleOrDefault(u => u.usuarioUsuario == usuarioNombre);

            if (usuario == null)
            {
                // Manejar el caso en que el usuario no se encuentra
                ViewBag.NombreUsuario = "Usuario no encontrado";
                ViewBag.Rol = "Sin rol";
                ViewBag.Nombre = "Nombre no disponible";
                ViewBag.Apellido = "Apellido no disponible";
                ViewBag.EstadoOnline = false;
                return;
            }

            // Pasar la información a la vista usando ViewBag
            ViewBag.NombreUsuario = usuario.usuarioUsuario;
            ViewBag.Rol = usuario.ROL.nombreRol; // Asegúrate de que la propiedad ROL y nombreRol existen en tu modelo
            ViewBag.Nombre = usuario.nombreUsuario;
            ViewBag.Apellido = usuario.apellidoUsuario;
            ViewBag.EstadoOnline = true; // Asumimos que el usuario está online si está autenticado
        }
    }
}

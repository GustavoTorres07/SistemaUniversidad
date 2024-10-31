using System.Web;
using System.Web.Mvc;
using System.Linq;
using SistemaUniversidad.Models;

namespace SistemaUniversidad.Filters
{
    // Define un atributo de autorización personalizado que hereda de AuthorizeAttribute
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        // Array de roles permitidos para el acceso
        private readonly string[] rolesPermitidos;

        // Constructor que recibe los roles permitidos como parámetros
        public CustomAuthorizeAttribute(params string[] roles)
        {
            // Inicializa el array de rolesPermitidos con los roles proporcionados
            this.rolesPermitidos = roles;
        }

        // Método que verifica si el usuario tiene permiso para acceder a la acción
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Variable para determinar si el usuario está autorizado
            bool autorizado = false;

            // Obtiene el nombre de usuario del contexto HTTP (se asume que es el identificador del usuario)
            var idUsuario = httpContext.User.Identity.Name;

            // Verifica si el nombre de usuario no es nulo o vacío
            if (!string.IsNullOrEmpty(idUsuario))
            {
                // Crea una instancia del contexto de base de datos
                using (var db = new UniversidadContext())
                {
                    // Busca al usuario en la base de datos usando el nombre de usuario
                    var usuario = db.USUARIO.FirstOrDefault(u => u.usuarioUsuario == idUsuario);

                    // Verifica si el usuario fue encontrado y tiene un rol válido (mayor que 0)
                    if (usuario != null && usuario.rol_id > 0)
                    {
                        // Busca el rol del usuario en la base de datos usando el ID del rol
                        var rolUsuario = db.ROL.FirstOrDefault(r => r.idRol == usuario.rol_id);

                        // Verifica si el rol del usuario está en la lista de roles permitidos
                        if (rolUsuario != null && this.rolesPermitidos.Contains(rolUsuario.nombreRol))
                        {
                            // Si el rol está permitido, autoriza el acceso
                            autorizado = true;
                        }
                    }
                }
            }

            // Devuelve el resultado de la autorización
            return autorizado;
        }

        // Método que maneja las solicitudes no autorizadas
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Redirige al usuario a la página de acceso denegado si no está autorizado
            filterContext.Result = new RedirectResult("~/Acceso/AccesoDenegado");
        }
    }
}

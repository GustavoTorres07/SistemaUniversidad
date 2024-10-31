using System.Web;
// Importa el espacio de nombres System.Web, que proporciona clases relacionadas con el contexto HTTP en aplicaciones web.

using System.Web.Mvc;
// Importa el espacio de nombres System.Web.Mvc, necesario para trabajar con el framework MVC de ASP.NET.

using SistemaUniversidad.Models;
// Importa el espacio de nombres donde están definidas las clases del modelo de datos, como UniversidadContext y USUARIO.

using System.Linq;
// Importa el espacio de nombres System.Linq, que proporciona métodos para consultas en colecciones de datos.

using System.Data.Entity;
// Importa el espacio de nombres System.Data.Entity, necesario para trabajar con Entity Framework y sus funcionalidades, como el método Include.

namespace SistemaUniversidad.Helpers
{
    public static class RoleHelper
    {
        // Define una clase estática RoleHelper en el espacio de nombres SistemaUniversidad.Helpers. Las clases estáticas no pueden ser instanciadas y contienen métodos estáticos.

        public static bool UsuarioTieneRol(string[] rolesPermitidos)
        {
            // Define un método público y estático que verifica si el usuario tiene uno de los roles permitidos. Retorna un valor booleano.

            var idUsuario = HttpContext.Current.User.Identity.Name;
            // Obtiene el nombre del usuario actualmente autenticado desde el contexto HTTP. Este valor se usa para identificar al usuario en la base de datos.

            if (!string.IsNullOrEmpty(idUsuario))
            {
                // Verifica que el nombre de usuario no sea nulo o vacío antes de proceder con la consulta a la base de datos.

                using (var db = new UniversidadContext())
                {
                    // Crea una instancia del contexto de la base de datos. El uso de `using` garantiza que el contexto se dispose correctamente al finalizar.

                    var usuario = db.USUARIO
                                    .Include(u => u.ROL)
                                    .FirstOrDefault(u => u.usuarioUsuario == idUsuario);
                    // Consulta la base de datos para encontrar el primer usuario que coincida con el nombre de usuario proporcionado. También incluye la información del rol asociado al usuario en la consulta.

                    if (usuario != null && usuario.ROL != null)
                    {
                        // Verifica si el usuario fue encontrado y tiene un rol asociado. Si es así, continúa con la verificación de roles.

                        return rolesPermitidos.Contains(usuario.ROL.nombreRol);
                        // Verifica si el rol del usuario está en la lista de roles permitidos. Retorna `true` si el rol está en la lista; de lo contrario, `false`.
                    }
                }
            }

            return false;
            // Si el usuario no fue encontrado o no tiene un rol, retorna `false`.
        }
    }
}

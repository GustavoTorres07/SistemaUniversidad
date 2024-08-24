using SistemaUniversidad.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BCrypt.Net;

public class AccesoController : Controller
{
    private readonly UniversidadContext _context;

    // Constructor para inyectar el contexto de la base de datos
    public AccesoController()
    {
        _context = new UniversidadContext();
    }

    // Acción para mostrar la vista de inicio de sesión
    public ActionResult Login()
    {
        return View();
    }

    // Acción para procesar el inicio de sesión
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(USUARIO model)
    {
        if (ModelState.IsValid)
        {
            // Buscar el usuario en la base de datos
            var usuario = _context.USUARIO
                .FirstOrDefault(u => u.usuarioUsuario == model.usuarioUsuario);

            if (usuario != null && BCrypt.Net.BCrypt.Verify(model.claveUsuario, usuario.claveUsuario))
            {
                // Usuario autenticado con éxito
                FormsAuthentication.SetAuthCookie(model.usuarioUsuario, false); // false indica que no se recordará la sesión
                return RedirectToAction("Index", "Inicio"); // Redirige a la página principal después del inicio de sesión
            }
            else
            {
                // Usuario o clave incorrectos
                ModelState.AddModelError("", "Usuario o clave incorrectos.");
            }
        }
        return View(model);
    }

    // Acción para mostrar la vista de recuperación de clave
    public ActionResult RecuperarClave()
    {
        return View();
    }

    // Acción para procesar la solicitud de recuperación de clave
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult RecuperarClave(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            ModelState.AddModelError("", "Por favor ingresa un correo electrónico.");
            return View();
        }

        // Aquí iría la lógica para enviar un correo electrónico con la clave de recuperación
        // Por ejemplo, generar un token de recuperación y enviarlo al usuario

        // Si la lógica de recuperación es exitosa
        TempData["Mensaje"] = "Se ha enviado un enlace para recuperar tu clave a tu correo electrónico.";
        return RedirectToAction("Login");
    }

    public ActionResult CerrarSesion()
    {
        // Elimina la cookie de autenticación y redirige a la página de inicio de sesión
        FormsAuthentication.SignOut();
        return RedirectToAction("Login");
    }
}

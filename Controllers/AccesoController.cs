using SistemaUniversidad.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BCrypt.Net;
using System.Web;
using System;

public class AccesoController : Controller
{
    private readonly UniversidadContext _context;

    public AccesoController()
    {
        _context = new UniversidadContext();
    }

    public ActionResult Login()
    {
        // Si hay un mensaje de error en TempData, lo asigna a ViewBag para mostrar en la vista
        ViewBag.LoginError = TempData["LoginError"];
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(USUARIO model)
    {
        // Si el modelo no es válido, redirige al usuario de vuelta a la vista de login
        if (!ModelState.IsValid)
        {
            TempData["LoginError"] = "Datos de inicio de sesión inválidos.";
            return RedirectToAction("Login"); // Redirige para mostrar el modal con TempData
        }

        // Busca al usuario en la base de datos por el nombre de usuario
        var usuario = _context.USUARIO.FirstOrDefault(u => u.usuarioUsuario == model.usuarioUsuario);

        // Verifica si el usuario existe y si la contraseña proporcionada es correcta
        if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.claveUsuario, usuario.claveUsuario))
        {
            TempData["LoginError"] = "Usuario o clave incorrectos.";
            return RedirectToAction("Login"); // Redirige para mostrar el modal con TempData
        }

        // Obtiene el rol del usuario
        var rol = _context.ROL.FirstOrDefault(r => r.idRol == usuario.rol_id)?.nombreRol;

        // Verifica si el rol es válido
        if (string.IsNullOrEmpty(rol))
        {
            TempData["LoginError"] = "Rol del usuario no encontrado.";
            return RedirectToAction("Login"); // Redirige para mostrar el modal con TempData
        }

        // Crea un ticket de autenticación y una cookie para el usuario autenticado
        var authTicket = new FormsAuthenticationTicket(
            1,
            usuario.usuarioUsuario,
            DateTime.Now,
            DateTime.Now.AddMinutes(30),
            false,
            rol,
            FormsAuthentication.FormsCookiePath
        );

        // Encripta el ticket y lo agrega a la cookie de autenticación
        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        Response.Cookies.Add(authCookie);

        // Redirige al usuario a la página de inicio después de iniciar sesión correctamente
        return RedirectToAction("Index", "Inicio");
    }

    [Authorize]
    public ActionResult CerrarSesion()
    {
        // Cierra la sesión del usuario
        FormsAuthentication.SignOut();
        return RedirectToAction("Login");
    }
}

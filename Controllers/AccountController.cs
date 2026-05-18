using EnergySaver.Data;
using EnergySaver.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace EnergySaver.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // LOGIN GET
        // =========================
        public IActionResult Login()
        {
            return View();
        }

        // =========================
        // LOGIN POST
        // =========================
        [HttpPost]
        public IActionResult Login(string correo, string password)
        {
            var user = _context.Usuarios
                .FirstOrDefault(u => u.Correo == correo && u.Password == password);

            if (user != null)
            {

                HttpContext.Session.SetInt32("UsuarioId", user.IdUsuario);
                HttpContext.Session.SetString("UsuarioNombre", user.Nombre);
                HttpContext.Session.SetString("UsuarioRol", user.Rol);
                if (
                    user.Rol.Trim().ToLower() == "admin" ||
                    user.Rol.Trim().ToLower() == "administrador"
                   )
                {
                    return RedirectToAction("Dashboard", "Admin");
                }

                return RedirectToAction("Usuario", "Home");
            }

            ViewBag.Error = "Correo o contraseña incorrectos, inténtelo nuevamente";

            ViewBag.Correo = correo;

            return View();
        }

        // =========================
        // REGISTER GET
        // =========================
        public IActionResult Register()
        {
            return View();
        }

        // =========================
        // REGISTER POST
        // =========================
        [HttpPost]
        public IActionResult Register(Usuario u)
        {
            // VALIDAR CORREO DUPLICADO
            var existeCorreo = _context.Usuarios
                .Any(x => x.Correo == u.Correo);

            if (existeCorreo)
            {
                TempData["Error"] = "El correo ya está registrado";

                return RedirectToAction("Register");
            }

            // GUARDAR USUARIO
            _context.Usuarios.Add(u);

            _context.SaveChanges();

            TempData["Success"] = "Registro exitoso";

            return RedirectToAction("Register");
        }
        // =========================
        // LOGOUT               ← AGREGA ESTE MÉTODO
        // =========================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


    }


}
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
                if (user.Rol == "Admin")
                    return RedirectToAction("Dashboard", "Admin");
                else
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
    }
}
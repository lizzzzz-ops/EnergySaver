using Microsoft.AspNetCore.Mvc;
using EnergySaver.Data;
using EnergySaver.Models;
using System.Linq;

namespace EnergySaver.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // DASHBOARD
        // =========================

        public IActionResult Dashboard()
        {
            return View();
        }

        // =========================
        // LISTA USUARIOS
        // =========================

        public IActionResult Usuarios()
        {
            var lista = _context.Usuarios.ToList();

            return View(lista);
        }

        // =========================
        // CREAR
        // =========================

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario u)
        {
            try
            {
                _context.Usuarios.Add(u);

                _context.SaveChanges();

                TempData["Success"] = "Usuario creado correctamente ✅";
            }
            catch
            {
                TempData["Error"] = "No se pudo crear el usuario ❌";
            }

            return RedirectToAction("Usuarios");
        }

        // =========================
        // EDITAR GET
        // =========================

        public IActionResult Edit(int id)
        {
            var user = _context.Usuarios.Find(id);

            if (user == null)
            {
                TempData["Error"] = "Usuario no encontrado ❌";

                return RedirectToAction("Usuarios");
            }

            return View(user);
        }

        // =========================
        // EDITAR POST
        // =========================

        [HttpPost]
        public IActionResult Edit(Usuario u)
        {
            var user = _context.Usuarios.Find(u.IdUsuario);

            if (user != null)
            {
                try
                {
                    user.Nombre = u.Nombre;
                    user.Correo = u.Correo;
                    user.Password = u.Password;
                    user.Rol = u.Rol;

                    _context.SaveChanges();

                    TempData["Success"] = "Cambios guardados correctamente ✅";
                }
                catch
                {
                    TempData["Error"] = "Error al actualizar ❌";
                }
            }
            else
            {
                TempData["Error"] = "Usuario no encontrado ❌";
            }

            return RedirectToAction("Edit", new { id = u.IdUsuario });
        }

        // =========================
        // ELIMINAR
        // =========================

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            var user = _context.Usuarios.Find(id);

            if (user != null)
            {
                try
                {
                    _context.Usuarios.Remove(user);

                    _context.SaveChanges();

                    TempData["Success"] = "Usuario eliminado correctamente ✅";
                }
                catch
                {
                    TempData["Error"] = "No se pudo eliminar el usuario ❌";
                }
            }
            else
            {
                TempData["Error"] = "Usuario no encontrado ❌";
            }

            return RedirectToAction("Usuarios");
        }

        // =========================
        // RESTABLECER PASSWORD
        // =========================

        public IActionResult ResetPassword(int id)
        {
            var user = _context.Usuarios.Find(id);

            if (user != null)
            {
                try
                {
                    user.Password = "1234";

                    _context.SaveChanges();

                    TempData["Success"] = "Contraseña restablecida a 1234 🔑";
                }
                catch
                {
                    TempData["Error"] = "No se pudo restablecer la contraseña ❌";
                }
            }
            else
            {
                TempData["Error"] = "Usuario no encontrado ❌";
            }

            return RedirectToAction("Usuarios");
        }
    }
}
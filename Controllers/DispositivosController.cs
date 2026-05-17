using Microsoft.AspNetCore.Mvc;
using EnergySaver.Data;
using EnergySaver.Models;
using System.Linq;

namespace EnergySaver.Controllers
{
    public class DispositivosController : Controller
    {
        private readonly AppDbContext _context;

        public DispositivosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var lista = _context.Dispositivos.ToList();
            return View(lista);
        }

        public IActionResult Create()
        {
            int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
            if (usuarioId == 0)
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        public IActionResult Create(Dispositivo d)
        {
            int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
            string rol = HttpContext.Session.GetString("UsuarioRol") ?? "";

            if (usuarioId == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            // 1. Asignar el usuario que tiene la sesión activa
            d.id_usuario = usuarioId;

            // 2. CORRECCIÓN: Si el estado llega vacío desde el formulario, ponerle uno por defecto
            if (string.IsNullOrEmpty(d.estado))
            {
                d.estado = "Activo"; 
            }

            _context.Dispositivos.Add(d);
            _context.SaveChanges();

            // 3. Redirigir asegurando que solo el que sea "Admin" vaya al Index corporativo
            if (rol.Trim().Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Usuario", "Home");
            }

        }

        // GET: Muestra el formulario con los datos actuales
        public IActionResult Edit(int id)
        {
            // Busca el dispositivo por su ID en la base de datos
            var dispositivo = _context.Dispositivos.Find(id);

            if (dispositivo == null)
            {
                return NotFound();
            }

            return View(dispositivo);
        }

        // Muestra la pantalla roja de advertencia
        public IActionResult Delete(int id)
        {
            var dispositivo = _context.Dispositivos.Find(id);
            if (dispositivo == null) return NotFound();
            return View(dispositivo);
        }

        // Borra el registro al presionar el botón rojo
       
        [HttpPost]
        public IActionResult ConfirmDelete(int id_dispositivo)
        {
            var dispositivo = _context.Dispositivos.Find(id_dispositivo);
            if (dispositivo != null)
            {
                _context.Dispositivos.Remove(dispositivo);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // Cambiar estado Activo/Inactivo desde el panel de usuario
        [HttpPost]
        public IActionResult CambiarEstado(int id, string estado)
        {
            var dispositivo = _context.Dispositivos.FirstOrDefault(d => d.id_dispositivo == id);
            if (dispositivo == null) return Json(new { ok = false });
            dispositivo.estado = estado;
            _context.SaveChanges();
            return Json(new { ok = true });
        }


        // Eliminar dispositivo desde el panel de usuario
        [HttpPost]
        public IActionResult EliminarAjax(int id)
        {
            var dispositivo = _context.Dispositivos.FirstOrDefault(d => d.id_dispositivo == id);
            if (dispositivo != null)
            {
                _context.Dispositivos.Remove(dispositivo);
                _context.SaveChanges();
            }
            return RedirectToAction("Usuario", "Home");
        }


    }
}
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

            // Asignar el usuario al dispositivo
            d.id_usuario = usuarioId;

            _context.Dispositivos.Add(d);
            _context.SaveChanges();

            // Redirigir según el rol
            if (rol == "Admin")
                return RedirectToAction("Index");
            else
                return RedirectToAction("Usuario", "Home");

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
    }
}
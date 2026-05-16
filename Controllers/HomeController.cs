using EnergySaver.Models;
using Microsoft.AspNetCore.Mvc;
using EnergySaver.Data;
using System.Diagnostics;

namespace EnergySaver.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _db;

        public HomeController(ILogger<HomeController> logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Usuario()
        {
            int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;

            if (usuarioId == 0)
                return RedirectToAction("Login", "Account");

            var usuario = _db.Usuarios.FirstOrDefault(u => u.IdUsuario == usuarioId);

            if (usuario == null)
                return RedirectToAction("Login", "Account");

            var dispositivos = _db.Dispositivos
                                  .Where(d => d.id_usuario == usuarioId)
                                  .ToList();

            ViewBag.NombreUsuario = usuario.Nombre;
            ViewBag.EmailUsuario = usuario.Correo;
            ViewBag.Dispositivos = dispositivos;
            ViewBag.TotalDispositivos = dispositivos.Count;
            ViewBag.DispositivosActivos = dispositivos.Count(d => d.estado == "Activo");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
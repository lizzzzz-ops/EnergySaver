using Microsoft.AspNetCore.Mvc;

namespace EnergySaver.Controllers
{
    public class MantenimientoController : Controller
    {
        // =========================
        // REINICIAR
        // =========================

        public IActionResult Reiniciar()
        {
            return View();
        }

        // =========================
        // MEDIDORES
        // =========================

        public IActionResult Medidores()
        {
            return View();
        }

        // =========================
        // ACTUALIZAR
        // =========================

        public IActionResult Actualizar()
        {
            return View();
        }
    }
}
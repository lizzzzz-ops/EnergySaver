using Microsoft.AspNetCore.Mvc;

namespace EnergySaver.Controllers
{
    public class AlertasController : Controller
    {
        // =========================
        // VER ALERTAS
        // =========================

        public IActionResult VerAlertas()
        {
            return View();
        }

        // =========================
        // FALLAS TECNICAS
        // =========================

        public IActionResult FallasTecnicas()
        {
            return View();
        }
    }
}
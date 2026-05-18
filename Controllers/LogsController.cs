using Microsoft.AspNetCore.Mvc;

namespace EnergySaver.Controllers
{
    public class LogsController : Controller
    {
        // =========================
        // ACTIVIDAD
        // =========================

        public IActionResult Actividad()
        {
            return View();
        }

        // =========================
        // INTENTOS LOGIN
        // =========================

        public IActionResult IntentosLogin()
        {
            return View();
        }
    }
}
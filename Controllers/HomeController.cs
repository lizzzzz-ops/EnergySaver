<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Usuario()
    {
        return View();
    }
}
=======
using System.Diagnostics;
using EnergySaver.Models;
using Microsoft.AspNetCore.Mvc;

namespace EnergySaver.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
>>>>>>> 7065bafb3b2de88c4632b6748e8432d70a0a39bc

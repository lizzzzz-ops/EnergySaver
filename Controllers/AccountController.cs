using Login_EnergySaver.Data;
using Login_EnergySaver.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

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

        ViewBag.Error = "Datos incorrectos";
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(Usuario u)
    {
        _context.Usuarios.Add(u);
        _context.SaveChanges();

        TempData["Mensaje"] = "Registro exitoso ✅";

        return RedirectToAction("Login");
    }
}
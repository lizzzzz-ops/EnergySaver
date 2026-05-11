using Microsoft.AspNetCore.Mvc;
using EnergySaver.Models;
using System.Linq;
using EnergySaver.Data;

public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult Usuarios()
    {
        var lista = _context.Usuarios.ToList();
        return View(lista);
    }

    // CREAR
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Usuario u)
    {
        _context.Usuarios.Add(u);
        _context.SaveChanges();

        TempData["Success"] = "Usuario creado correctamente ✅";

        return RedirectToAction("Usuarios");
    }

    // EDITAR (GET)
    public IActionResult Edit(int id)
    {
        var user = _context.Usuarios.Find(id);
        return View(user);
    }

    // EDITAR (POST)
    [HttpPost]
    public IActionResult Edit(Usuario u)
    {
        var user = _context.Usuarios.Find(u.IdUsuario);

        if (user != null)
        {
            user.Nombre = u.Nombre;
            user.Correo = u.Correo;
            user.Rol = u.Rol;

            _context.SaveChanges();

            TempData["Success"] = "Usuario actualizado ✏️";
        }
        else
        {
            TempData["Error"] = "Error al actualizar ❌";
        }

        return RedirectToAction("Usuarios");
    }

    // ELIMINAR
    [HttpPost]
    public IActionResult Eliminar(int id)
    {
        var user = _context.Usuarios.Find(id);

        if (user != null)
        {
            _context.Usuarios.Remove(user);
            _context.SaveChanges();
            TempData["Success"] = "Usuario eliminado correctamente ❌";
        }
        else
        {
            TempData["Error"] = "Usuario no encontrado";
        }

        return RedirectToAction("Usuarios");
    }

    // RESETEAR PASSWORD
    public IActionResult ResetPassword(int id)
    {
        var user = _context.Usuarios.Find(id);

        if (user != null)
        {
            user.Password = "1234";
            _context.SaveChanges();

            TempData["Success"] = "Contraseña reseteada a 1234 🔑";
        }
        else
        {
            TempData["Error"] = "Usuario no encontrado";
        }

        return RedirectToAction("Usuarios");
    }
}
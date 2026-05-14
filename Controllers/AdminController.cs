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
        // CREAR USUARIO
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
        // EDITAR USUARIO GET
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
        // EDITAR USUARIO POST
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
        // ELIMINAR USUARIO
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

        // =========================
        // CONFIGURACIÓN
        // =========================

        public IActionResult Configuracion()
        {
            return View();
        }

        // =========================
        // TARIFA
        // =========================

        public IActionResult Tarifa()
        {
            return View();
        }

        // =========================
        // IMPUESTOS
        // =========================

        public IActionResult Impuestos()
        {
            return View();
        }

        // =========================
        // HORARIOS
        // =========================

        public IActionResult Horarios()
        {
            return View();
        }

        // =========================
        // GUARDAR TARIFA
        // =========================

        [HttpPost]
        public IActionResult GuardarTarifa(decimal tarifa)
        {
            try
            {
                var config = _context.Configuracion.FirstOrDefault();

                if (config == null)
                {
                    config = new Configuracion
                    {
                        TarifaCFE = tarifa,
                        Impuesto = 0,
                        HoraInicio = "",
                        HoraFin = ""
                    };

                    _context.Configuracion.Add(config);
                }
                else
                {
                    config.TarifaCFE = tarifa;
                }

                _context.SaveChanges();

                TempData["Success"] = "Nueva tarifa agregada exitosamente ✅";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Tarifa");
        }

        // =========================
        // GUARDAR HORARIOS
        // =========================

        [HttpPost]
        public IActionResult GuardarHorarios(string horaInicio, string horaFin)
        {
            try
            {
                var config = _context.Configuracion.FirstOrDefault();

                if (config == null)
                {
                    config = new Configuracion
                    {
                        TarifaCFE = 0,
                        Impuesto = 0,
                        HoraInicio = horaInicio,
                        HoraFin = horaFin
                    };

                    _context.Configuracion.Add(config);
                }
                else
                {
                    config.HoraInicio = horaInicio;
                    config.HoraFin = horaFin;
                }

                _context.SaveChanges();

                TempData["Success"] = "Nuevo horario agregado exitosamente ✅";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Horarios");
        }

        // =========================
        // CONSULTAR TARIFAS
        // =========================

        public IActionResult ConsultarTarifas()
        {
            var datos = _context.Configuracion.ToList();

            return View(datos);
        }

        // =========================
        // CONSULTAR HORARIOS
        // =========================

        public IActionResult ConsultarHorarios()
        {
            var datos = _context.Configuracion.ToList();

            return View(datos);
        }

        // =========================
        // EDITAR TARIFA
        // =========================

        public IActionResult EditarTarifa(int id)
        {
            var tarifa = _context.Configuracion.Find(id);

            return View(tarifa);
        }

        [HttpPost]
        public IActionResult EditarTarifa(Configuracion c)
        {
            var tarifa = _context.Configuracion.Find(c.Id);

            if (tarifa != null)
            {
                tarifa.TarifaCFE = c.TarifaCFE;

                _context.SaveChanges();

                TempData["Success"] = "Tarifa editada correctamente ✅";
            }

            return RedirectToAction("ConsultarTarifas");
        }

        // =========================
        // ELIMINAR TARIFA
        // =========================

        public IActionResult EliminarTarifa(int id)
        {
            var tarifa = _context.Configuracion.Find(id);

            if (tarifa != null)
            {
                _context.Configuracion.Remove(tarifa);

                _context.SaveChanges();

                TempData["Success"] = "Tarifa eliminada correctamente 🗑";
            }

            return RedirectToAction("ConsultarTarifas");
        }
        // =========================
        // EDITAR HORARIO
        // =========================

        public IActionResult EditarHorario(int id)
        {
            var horario = _context.Configuracion.Find(id);

            return View(horario);
        }

        [HttpPost]
        public IActionResult EditarHorario(Configuracion c)
        {
            var horario = _context.Configuracion.Find(c.Id);

            if (horario != null)
            {
                horario.HoraInicio = c.HoraInicio;
                horario.HoraFin = c.HoraFin;

                _context.SaveChanges();

                TempData["Success"] = "Horario editado correctamente ✅";
            }

            return RedirectToAction("ConsultarHorarios");
        }

        // =========================
        // ELIMINAR HORARIO
        // =========================

        public IActionResult EliminarHorario(int id)
        {
            var horario = _context.Configuracion.Find(id);

            if (horario != null)
            {
                _context.Configuracion.Remove(horario);

                _context.SaveChanges();

                TempData["Success"] = "Horario eliminado correctamente 🗑";
            }

            return RedirectToAction("ConsultarHorarios");
        }
    }
}
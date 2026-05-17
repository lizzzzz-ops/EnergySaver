using EnergySaver.Models;
using Microsoft.AspNetCore.Mvc;
using EnergySaver.Data;
using System.Diagnostics;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Commons.Bouncycastle.Cert;
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




        // ========================================
        // Dashboard
        // =========================================
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

            // Fechas
            var hace7dias = DateTime.Now.AddDays(-7);
            var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var inicioMesAnterior = inicioMes.AddMonths(-1);

            // Consumo 7 días agrupado por día
            var consumosSemana = _db.Consumos
                .Where(c => c.id_usuario == usuarioId && c.fecha >= hace7dias)
                .AsEnumerable()
                .GroupBy(c => c.fecha.Date)
                .OrderBy(g => g.Key)
                .Select(g => new
                {
                    Fecha = g.Key,
                    Valor = g.Sum(c => c.valor)
                })
                .ToList();

            // Consumo mes actual
            var consumoMes = _db.Consumos
                .Where(c => c.id_usuario == usuarioId && c.fecha >= inicioMes)
                .Sum(c => (double?)c.valor) ?? 0;

            // Consumo mes anterior
            var consumoMesAnterior = _db.Consumos
                .Where(c => c.id_usuario == usuarioId
                         && c.fecha >= inicioMesAnterior
                         && c.fecha < inicioMes)
                .Sum(c => (double?)c.valor) ?? 0;

            // Labels y valores para gráfica
            List<string> labels;
            List<double> valores;

            if (consumosSemana.Any())
            {
                labels = consumosSemana.Select(c => c.Fecha.ToString("ddd")).ToList();
                valores = consumosSemana.Select(c => c.Valor).ToList();
            }
            else
            {
                labels = new List<string> { "Lun", "Mar", "Mié", "Jue", "Vie", "Sáb", "Dom" };
                valores = new List<double> { 0, 0, 0, 0, 0, 0, 0 };
            }

            ViewBag.ConsumoMes = Math.Round(consumoMes, 2);
            ViewBag.ConsumoMesAnterior = Math.Round(consumoMesAnterior, 2);
            ViewBag.GraficaLabels = System.Text.Json.JsonSerializer.Serialize(labels);
            ViewBag.GraficaValores = System.Text.Json.JsonSerializer.Serialize(valores);
            ViewBag.NombreUsuario = usuario.Nombre;
            ViewBag.EmailUsuario = usuario.Correo;
            ViewBag.Dispositivos = dispositivos;
            ViewBag.TotalDispositivos = dispositivos.Count;
            ViewBag.DispositivosActivos = dispositivos.Count(d => d.estado == "Activo");

            return View();
        }



        public IActionResult DescargarPDF()
        {
            iText.Bouncycastleconnector.BouncyCastleFactoryCreator.SetFactory(new iText.Bouncycastle.BouncyCastleFactory());

            int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;
            var usuario = _db.Usuarios.FirstOrDefault(u => u.IdUsuario == usuarioId);
            var dispositivos = _db.Dispositivos.Where(d => d.id_usuario == usuarioId).ToList();

            var inicioMes = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var consumoMes = _db.Consumos
                .Where(c => c.id_usuario == usuarioId && c.fecha >= inicioMes)
                .Sum(c => (double?)c.valor) ?? 0;

            using var stream = new MemoryStream();
            var writer = new PdfWriter(stream);
            var pdf = new PdfDocument(writer);
            var doc = new Document(pdf);

            // Título
            var titulo = new Paragraph("EnergySaver - Reporte de Consumo");
            titulo.SetFontSize(20);
            titulo.SetFontColor(new DeviceRgb(26, 91, 62));
            doc.Add(titulo);

            doc.Add(new Paragraph($"Usuario: {usuario?.Nombre ?? "N/A"}").SetFontSize(12));
            doc.Add(new Paragraph($"Correo: {usuario?.Correo ?? "N/A"}").SetFontSize(12));
            doc.Add(new Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}").SetFontSize(12));
            doc.Add(new Paragraph(" "));

            // Resumen
            var subtitulo1 = new Paragraph("Resumen del mes");
            subtitulo1.SetFontSize(15);
            subtitulo1.SetFontColor(new DeviceRgb(26, 91, 62));
            doc.Add(subtitulo1);

            doc.Add(new Paragraph($"Consumo total: {Math.Round(consumoMes, 2)} kWh").SetFontSize(12));
            doc.Add(new Paragraph($"Gasto estimado: ${Math.Round(consumoMes * 2.5, 2)}").SetFontSize(12));
            doc.Add(new Paragraph(" "));

            // Dispositivos
            var subtitulo2 = new Paragraph("Dispositivos registrados");
            subtitulo2.SetFontSize(15);
            subtitulo2.SetFontColor(new DeviceRgb(26, 91, 62));
            doc.Add(subtitulo2);

            var tabla = new Table(new float[] { 3, 2, 2, 2, 2 }).UseAllAvailableWidth();

            foreach (var header in new[] { "Nombre", "Tipo", "Watts", "Ubicacion", "Estado" })
            {
                var celda = new Cell();
                celda.Add(new Paragraph(header));
                celda.SetBackgroundColor(new DeviceRgb(26, 91, 62));
                celda.SetFontColor(ColorConstants.WHITE);
                tabla.AddHeaderCell(celda);
            }

            foreach (var d in dispositivos)
            {
                tabla.AddCell(new Paragraph(d.nombre ?? ""));
                tabla.AddCell(new Paragraph(d.tipo ?? ""));
                tabla.AddCell(new Paragraph($"{d.consumoWatts ?? 0} W"));
                tabla.AddCell(new Paragraph(d.ubicacion ?? ""));
                tabla.AddCell(new Paragraph(d.estado ?? ""));
            }

            doc.Add(tabla);
            doc.Close();

            return File(stream.ToArray(), "application/pdf",
                $"Reporte_EnergySaver_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public IActionResult EnviarReporte()
        {
            TempData["Mensaje"] = "Funcionalidad de correo próximamente.";
            return RedirectToAction("Usuario");
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
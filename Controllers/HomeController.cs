using EnergySaver.Data;
using EnergySaver.Models;

using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;
using System.Net;
using System.Net.Mail;

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
        // Dashboard Usuario
        // ========================================
        public IActionResult Usuario()
        {
            int usuarioId = HttpContext.Session.GetInt32("UsuarioId") ?? 0;

            if (usuarioId == 0)
                return RedirectToAction("Login", "Account");

            var usuario = _db.Usuarios
                .FirstOrDefault(u => u.IdUsuario == usuarioId);

            if (usuario == null)
                return RedirectToAction("Login", "Account");

            var dispositivos = _db.Dispositivos
                .Where(d => d.id_usuario == usuarioId)
                .ToList();

            // Fechas
            var hace7dias = DateTime.Now.AddDays(-7);

            var inicioMes =
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var inicioMesAnterior = inicioMes.AddMonths(-1);

            // Consumos últimos 7 días
            var consumosSemana = _db.Consumos
                .Where(c => c.id_usuario == usuarioId
                         && c.fecha >= hace7dias)
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
                .Where(c => c.id_usuario == usuarioId
                         && c.fecha >= inicioMes)
                .Sum(c => (double?)c.valor) ?? 0;

            // Consumo mes anterior
            var consumoMesAnterior = _db.Consumos
                .Where(c => c.id_usuario == usuarioId
                         && c.fecha >= inicioMesAnterior
                         && c.fecha < inicioMes)
                .Sum(c => (double?)c.valor) ?? 0;

            // Datos gráfica
            List<string> labels;
            List<double> valores;

            if (consumosSemana.Any())
            {
                labels = consumosSemana
                    .Select(c => c.Fecha.ToString("ddd"))
                    .ToList();

                valores = consumosSemana
                    .Select(c => c.Valor)
                    .ToList();
            }
            else
            {
                labels = new List<string>
                {
                    "Lun","Mar","Mié","Jue","Vie","Sáb","Dom"
                };

                valores = new List<double>
                {
                    0,0,0,0,0,0,0
                };
            }

            ViewBag.ConsumoMes = Math.Round(consumoMes, 2);

            ViewBag.ConsumoMesAnterior =
                Math.Round(consumoMesAnterior, 2);

            ViewBag.GraficaLabels =
                System.Text.Json.JsonSerializer.Serialize(labels);

            ViewBag.GraficaValores =
                System.Text.Json.JsonSerializer.Serialize(valores);

            ViewBag.NombreUsuario = usuario.Nombre;

            ViewBag.EmailUsuario = usuario.Correo;

            ViewBag.Dispositivos = dispositivos;

            ViewBag.TotalDispositivos = dispositivos.Count;

            ViewBag.DispositivosActivos =
                dispositivos.Count(d => d.estado == "Activo");

            return View();
        }

        // ========================================
        // Descargar PDF
        // ========================================
        public IActionResult DescargarPDF()
        {
            int usuarioId =
                HttpContext.Session.GetInt32("UsuarioId") ?? 0;

            var usuario = _db.Usuarios
                .FirstOrDefault(u => u.IdUsuario == usuarioId);

            var dispositivos = _db.Dispositivos
                .Where(d => d.id_usuario == usuarioId)
                .ToList();

            var inicioMes =
                new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var consumoMes = _db.Consumos
                .Where(c => c.id_usuario == usuarioId
                         && c.fecha >= inicioMes)
                .Sum(c => (double?)c.valor) ?? 0;

            using var stream = new MemoryStream();

            var writer = new PdfWriter(stream);

            var pdf = new PdfDocument(writer);

            var doc = new Document(pdf);

            // =========================
            // Título
            // =========================
            var titulo =
                new Paragraph("EnergySaver - Reporte de Consumo");

            titulo.SetFontSize(20);

            titulo.SetFontColor(new DeviceRgb(26, 91, 62));

            titulo.SetTextAlignment(TextAlignment.CENTER);

            doc.Add(titulo);

            doc.Add(new Paragraph(" "));

            // =========================
            // Datos usuario
            // =========================
            doc.Add(new Paragraph(
                $"Usuario: {usuario?.Nombre ?? "N/A"}"
            ));

            doc.Add(new Paragraph(
                $"Correo: {usuario?.Correo ?? "N/A"}"
            ));

            doc.Add(new Paragraph(
                $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}"
            ));

            doc.Add(new Paragraph(" "));

            // =========================
            // Resumen
            // =========================
            var subtitulo =
                new Paragraph("Resumen del Mes");

            subtitulo.SetFontSize(16);

            subtitulo.SetFontColor(new DeviceRgb(26, 91, 62));

            doc.Add(subtitulo);

            doc.Add(new Paragraph(
                $"Consumo total: {Math.Round(consumoMes, 2)} kWh"
            ));

            doc.Add(new Paragraph(
                $"Gasto estimado: ${Math.Round(consumoMes * 2.5, 2)}"
            ));

            doc.Add(new Paragraph(" "));

            // =========================
            // Tabla dispositivos
            // =========================
            var subtitulo2 =
                new Paragraph("Dispositivos Registrados");

            subtitulo2.SetFontSize(16);

            subtitulo2.SetFontColor(new DeviceRgb(26, 91, 62));

            doc.Add(subtitulo2);

            var tabla = new Table(new float[] { 3, 2, 2, 2, 2 });

            tabla.UseAllAvailableWidth();

            string[] headers =
            {
                "Nombre",
                "Tipo",
                "Watts",
                "Ubicación",
                "Estado"
            };

            foreach (var h in headers)
            {
                var celda = new Cell();

                celda.Add(new Paragraph(h));

                celda.SetBackgroundColor(
                    new DeviceRgb(26, 91, 62)
                );

                celda.SetFontColor(ColorConstants.WHITE);

                tabla.AddHeaderCell(celda);
            }

            foreach (var d in dispositivos)
            {
                tabla.AddCell(
                    new Paragraph(d.nombre ?? "")
                );

                tabla.AddCell(
                    new Paragraph(d.tipo ?? "")
                );

                tabla.AddCell(
                    new Paragraph($"{d.consumoWatts ?? 0} W")
                );

                tabla.AddCell(
                    new Paragraph(d.ubicacion ?? "")
                );

                tabla.AddCell(
                    new Paragraph(d.estado ?? "")
                );
            }

            doc.Add(tabla);

            doc.Close();

            return File(
                stream.ToArray(),
                "application/pdf",
                $"Reporte_EnergySaver_{DateTime.Now:yyyyMMdd}.pdf"
            );
        }

        public IActionResult EnviarReporte()
        {
            try
            {
                int usuarioId =
                    HttpContext.Session.GetInt32("UsuarioId") ?? 0;

                var usuario = _db.Usuarios
                    .FirstOrDefault(u => u.IdUsuario == usuarioId);

                if (usuario == null)
                {
                    TempData["Error"] = "Usuario no encontrado";

                    return RedirectToAction("Usuario");
                }

                // =====================================
                // CONSUMO DEL USUARIO
                // =====================================

                var consumoMes = _db.Consumos
                    .Where(c => c.id_usuario == usuarioId)
                    .Sum(c => (double?)c.valor) ?? 0;

                // =====================================
                // CREAR PDF EN MEMORIA
                // =====================================

                byte[] pdfBytes;

                using (MemoryStream ms = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(ms);

                    PdfDocument pdf = new PdfDocument(writer);

                    Document doc = new Document(pdf);

                    // TITULO
                    Paragraph titulo =
                        new Paragraph("Reporte EnergySaver");

                    titulo.SetFontSize(20);

                    titulo.SetTextAlignment(TextAlignment.CENTER);

                    titulo.SetFontColor(
                        new DeviceRgb(26, 91, 62)
                    );

                    doc.Add(titulo);

                    doc.Add(new Paragraph(" "));

                    // DATOS
                    doc.Add(new Paragraph(
                        $"Usuario: {usuario.Nombre}"
                    ));

                    doc.Add(new Paragraph(
                        $"Correo: {usuario.Correo}"
                    ));

                    doc.Add(new Paragraph(
                        $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}"
                    ));

                    doc.Add(new Paragraph(" "));

                    // CONSUMO
                    doc.Add(new Paragraph(
                        $"Consumo total: {Math.Round(consumoMes, 2)} kWh"
                    ));

                    doc.Add(new Paragraph(
                        $"Costo estimado: ${Math.Round(consumoMes * 2.5, 2)}"
                    ));

                    doc.Add(new Paragraph(" "));

                    doc.Add(new Paragraph(
                        "Gracias por usar EnergySaver."
                    ));

                    doc.Close();

                    pdfBytes = ms.ToArray();
                }

                // =====================================
                // CONFIG GMAIL
                // =====================================

                var email =
                    "michellecarrazco70@gmail.com";

                var password =
                    "fbdgcahhkdxfleae";

                MailMessage mensaje =
                    new MailMessage();

                mensaje.From =
                    new MailAddress(email);

                // CORREO DEL USUARIO
                mensaje.To.Add(usuario.Correo);

                mensaje.Subject =
                    "Reporte de Consumo EnergySaver";

                mensaje.Body =
                    $"Hola {usuario.Nombre},\n\n" +
                    "Adjuntamos tu reporte personalizado de consumo.\n\n" +
                    "Gracias por usar EnergySaver.";

                // =====================================
                // ADJUNTAR PDF
                // =====================================

                Attachment adjunto =
                    new Attachment(
                        new MemoryStream(pdfBytes),
                        "ReporteEnergySaver.pdf",
                        "application/pdf"
                    );

                mensaje.Attachments.Add(adjunto);

                // =====================================
                // SMTP
                // =====================================

                SmtpClient smtp =
                    new SmtpClient("smtp.gmail.com", 587);

                smtp.Credentials =
                    new NetworkCredential(email, password);

                smtp.EnableSsl = true;

                smtp.Send(mensaje);

                TempData["Success"] =
                    "Correo enviado correctamente";
            }
            catch (Exception ex)
            {
                TempData["Error"] =
                    "Error: " + ex.Message;
            }

            return RedirectToAction("Usuario");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using EnergySaver.Data;
using System.Linq;
using ClosedXML.Excel;
using System.IO;
using System;

namespace EnergySaver.Controllers
{
    public class ReportesController : Controller
    {
        private readonly AppDbContext _context;

        public ReportesController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // CONSUMO TOTAL
        // =========================

        public IActionResult ConsumoTotal()
        {
            var datos = _context.Consumo.ToList();

            return View(datos);
        }

        // =========================
        // COMPARAR MESES
        // =========================

        public IActionResult CompararMeses()
        {
            int mesActual = DateTime.Now.Month;

            int mesAnterior = DateTime.Now.AddMonths(-1).Month;

            // SUMA MES ACTUAL

            var actual =
                _context.Consumo
                .Where(c => c.fecha.Month == mesActual)
                .Sum(c => (double?)c.valor) ?? 0;

            // SUMA MES ANTERIOR

            var anterior =
                _context.Consumo
                .Where(c => c.fecha.Month == mesAnterior)
                .Sum(c => (double?)c.valor) ?? 0;

            ViewBag.MesActual = actual;

            ViewBag.MesAnterior = anterior;

            return View();
        }
        public IActionResult ExportarExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet =
                    workbook.Worksheets.Add("Consumos");

                // TITULOS

                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Usuario";
                worksheet.Cell(1, 3).Value = "Dispositivo";
                worksheet.Cell(1, 4).Value = "Fecha";
                worksheet.Cell(1, 5).Value = "Consumo";

                // DATOS

                var lista =
                    _context.Consumo.ToList();

                int fila = 2;

                foreach (var item in lista)
                {
                    worksheet.Cell(fila, 1).Value =
                        item.id_consumo;

                    worksheet.Cell(fila, 2).Value =
                        item.id_usuario;

                    worksheet.Cell(fila, 3).Value =
                        item.id_dispositivo;

                    worksheet.Cell(fila, 4).Value =
                        item.fecha.ToString();

                    worksheet.Cell(fila, 5).Value =
                        item.valor;

                    fila++;
                }

                // AJUSTAR COLUMNAS

                worksheet.Columns().AdjustToContents();

                // MEMORIA

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);

                    var content =
                        stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "ReporteConsumo.xlsx"
                    );
                }
            }
        }

    }
}
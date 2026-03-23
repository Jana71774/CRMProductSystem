using Microsoft.AspNetCore.Mvc;
using CRMProductSystem.Services;
using CRMProductSystem.ViewModels;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Drawing;
using PdfDocument = Syncfusion.Pdf.PdfDocument;
using PdfPage = Syncfusion.Pdf.PdfPage;


namespace CRMProductSystem.Controllers
{
    public class ReportsController : Controller
    {
        private readonly OrderService _orderService;
        private readonly ReportService _reportService;

        public ReportsController(OrderService orderService,ReportService reportService)
        {
            _orderService = orderService;
            _reportService = reportService;

        }

        // ================= SALES REPORT PAGE =================

        public IActionResult SalesReport(DateTime? fromDate, DateTime? toDate)
        {
            var orders = _orderService.GetAllOrders();

            if (fromDate.HasValue)
                orders = orders.Where(o => o.OrderDate >= fromDate.Value).ToList();

            if (toDate.HasValue)
                orders = orders.Where(o => o.OrderDate <= toDate.Value).ToList();

            var model = new SalesReportVM
            {
                FromDate = fromDate,
                ToDate = toDate,
                Orders = orders,
                TotalOrders = orders.Count,
                TotalRevenue = orders.Sum(o => o.TotalAmount)
            };

            return View(model);
        }

        // ================= EXPORT TO EXCEL =================

        public IActionResult ExportToExcel(DateTime? fromDate, DateTime? toDate)
        {
            var orders = _orderService.GetAllOrders();

            if (fromDate.HasValue)
                orders = orders.Where(o => o.OrderDate >= fromDate.Value).ToList();

            if (toDate.HasValue)
                orders = orders.Where(o => o.OrderDate <= toDate.Value).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sales Report");

                worksheet.Cell(1, 1).Value = "Order ID";
                worksheet.Cell(1, 2).Value = "Customer Name";
                worksheet.Cell(1, 3).Value = "Order Date";
                worksheet.Cell(1, 4).Value = "Total Amount";

                int row = 2;

                foreach (var order in orders)
                {
                    worksheet.Cell(row, 1).Value = order.OrderId;
                    worksheet.Cell(row, 2).Value = order.CustomerName;
                    worksheet.Cell(row, 3).Value = order.OrderDate.ToShortDateString();
                    worksheet.Cell(row, 4).Value = order.TotalAmount;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "SalesReport.xlsx");
                }
            }
        }
        public IActionResult ExportToPdf(DateTime? fromDate, DateTime? toDate)
        {
            var orders = _reportService.GetSalesReport(fromDate, toDate);

            if (orders == null || !orders.Any())
                return Content("No orders found for the selected date range.");

            using (PdfDocument document = new PdfDocument())
            {
                PdfPage page = document.Pages.Add();
                PdfGraphics graphics = page.Graphics;

                // 1️⃣ Title
                PdfFont titleFont = new PdfStandardFont(PdfFontFamily.Helvetica, 18, PdfFontStyle.Bold);
                graphics.DrawString("CRM Sales Report", titleFont, PdfBrushes.Black, new PointF(0, 0));

                // 2️⃣ Table
                PdfGrid grid = new PdfGrid();
                grid.DataSource = orders.Select(o => new
                {
                    OrderID = o.OrderId,
                    Customer = o.CustomerName,
                    Date = o.OrderDate.ToString("dd-MM-yyyy"),
                    Total = o.TotalAmount.ToString("0.00")
                }).ToList();

                // 3️⃣ Table styling
                grid.Style.CellPadding = new PdfPaddings(5,5,5,5);  // ✅ Correct type
                grid.Style.Font = new PdfStandardFont(PdfFontFamily.Helvetica, 12);
                grid.Style.CellSpacing = 0.5f;               // Simulated borders
                grid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1);

                // 4️⃣ Draw table
                grid.Draw(page, new PointF(0, 40));

                // 5️⃣ Grand total
                decimal grandTotal = orders.Sum(o => o.TotalAmount);
                graphics.DrawString($"Grand Total: {grandTotal:0.00}",
                    new PdfStandardFont(PdfFontFamily.Helvetica, 14, PdfFontStyle.Bold),
                    PdfBrushes.Black,
                    new PointF(350, page.GetClientSize().Height - 30));

                // 6️⃣ Save PDF and return
                using (var stream = new MemoryStream())
                {
                    document.Save(stream);
                    return File(stream.ToArray(), "application/pdf", "SalesReport.pdf");
                }
            }
        }
    }
}
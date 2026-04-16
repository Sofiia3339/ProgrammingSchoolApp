using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammingSchoolApp.Models;
using ClosedXML.Excel;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace ProgrammingSchoolApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ProgrammingSchoolContext _context;

        public CoursesController(ProgrammingSchoolContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }
        
        public IActionResult Create()
        {
            return View();
}

public async Task<IActionResult> Details(int? id)
{
    if (id == null) return NotFound();
    var course = await _context.Courses.FirstOrDefaultAsync(m => m.Id == id);
    if (course == null) return NotFound();
    return View(course);
}

public async Task<IActionResult> Edit(int? id)
{
    if (id == null) return NotFound();
    var course = await _context.Courses.FindAsync(id);
    if (course == null) return NotFound();
    return View(course);
}

[HttpPost]
public async Task<IActionResult> Edit(int id, Course course)
{
    if (id != course.Id) return NotFound();

    if (ModelState.IsValid)
    {
        try
        {
            var originalCourse = await _context.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

            if (originalCourse != null && originalCourse.Price != course.Price)
            {
                var priceHistory = new Coursepricehistory
                {
                    Courseid = course.Id,
                    Oldprice = originalCourse.Price,
                    Newprice = course.Price,
                    Changedat = DateTime.Now,
                    Adminid = 1 
                };
                
                _context.Add(priceHistory);
            }

            _context.Update(course);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ПОМИЛКА ЗБЕРЕЖЕННЯ: {ex.Message}");
            if (ex.InnerException != null) Console.WriteLine($"ДЕТАЛІ: {ex.InnerException.Message}");
        }
    }
    return View(course);
}

public async Task<IActionResult> Delete(int? id)
{
    if (id == null) return NotFound();
    var course = await _context.Courses.FirstOrDefaultAsync(m => m.Id == id);
    if (course == null) return NotFound();
    return View(course);
}

[HttpPost, ActionName("Delete")]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var course = await _context.Courses.FindAsync(id);
    if (course != null) _context.Courses.Remove(course);
    await _context.SaveChangesAsync();
    return RedirectToAction(nameof(Index));
}

[HttpPost]
public async Task<IActionResult> Create(Course course)
{
    if (ModelState.IsValid)
    {
        _context.Add(course);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    return View(course);
}

[HttpGet]
public JsonResult GetChartData()
{
    var data = _context.Courses
        .Select(c => new { title = c.Title, price = c.Price })
        .ToList();
    return Json(data);
}

[HttpGet]
public async Task<IActionResult> ExportPriceHistoryToExcel()
{
    try
    {
        var history = await _context.Coursepricehistories
            .OrderByDescending(h => h.Changedat)
            .ToListAsync();

        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Динаміка цін");
        var currentRow = 1;

        worksheet.Cell(currentRow, 1).Value = "ID Курсу";
        worksheet.Cell(currentRow, 2).Value = "Стара ціна";
        worksheet.Cell(currentRow, 3).Value = "Нова ціна";
        worksheet.Cell(currentRow, 4).Value = "Дата зміни";

        foreach (var item in history)
        {
            currentRow++;
            worksheet.Cell(currentRow, 1).Value = item.Courseid;
            worksheet.Cell(currentRow, 2).Value = item.Oldprice ?? 0;
            worksheet.Cell(currentRow, 3).Value = item.Newprice;
            worksheet.Cell(currentRow, 4).Value = item.Changedat.HasValue ? item.Changedat.Value.ToString("dd.MM.yyyy HH:mm") : "";
        }

        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0; 
        
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PriceDynamics.xlsx");
    }
    catch (Exception ex)
    {
        return Content($"ОЙ, СТАЛАСЯ ПОМИЛКА!\n\nГоловна причина: {ex.Message}\n\nДеталі: {ex.InnerException?.Message}");
    }
}
[HttpGet]
public async Task<IActionResult> ExportPriceHistoryToWord()
{
    var history = await _context.Coursepricehistories
        .Include(h => h.Course)
        .OrderByDescending(h => h.Changedat)
        .ToListAsync();

    var stream = new MemoryStream();
    using (var document = DocX.Create(stream))
    {
        document.InsertParagraph("Звіт: Динаміка цін на курси").FontSize(16).Bold().Alignment = Alignment.center;
        document.InsertParagraph("");

        var table = document.AddTable(history.Count + 1, 4);
        table.Rows[0].Cells[0].Paragraphs.First().Append("Курс").Bold();
        table.Rows[0].Cells[1].Paragraphs.First().Append("Стара ціна").Bold();
        table.Rows[0].Cells[2].Paragraphs.First().Append("Нова ціна").Bold();
        table.Rows[0].Cells[3].Paragraphs.First().Append("Дата").Bold();

        for (int i = 0; i < history.Count; i++)
        {
            table.Rows[i + 1].Cells[0].Paragraphs.First().Append(history[i].Course?.Title ?? "");
            table.Rows[i + 1].Cells[1].Paragraphs.First().Append(history[i].Oldprice?.ToString() ?? "");
            table.Rows[i + 1].Cells[2].Paragraphs.First().Append(history[i].Newprice.ToString());
            table.Rows[i + 1].Cells[3].Paragraphs.First().Append(history[i].Changedat.HasValue ? history[i].Changedat.Value.ToString("dd.MM.yyyy") : "");
        }

        document.InsertTable(table);
        document.Save();
    }
    
    stream.Position = 0; 
    return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "PriceDynamicsReport.docx");
}
[HttpPost]
public async Task<IActionResult> ImportFromExcel(IFormFile fileExcel)
{
    if (fileExcel != null && fileExcel.Length > 0)
    {
        using (var stream = new MemoryStream())
        {
            await fileExcel.CopyToAsync(stream);
            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheet(1); 
                var rowCount = worksheet.RowsUsed().Count();

                for (int row = 2; row <= rowCount; row++)
                {
                    var course = new Course
                    {
                        Title = worksheet.Cell(row, 1).GetValue<string>(),
                        Description = worksheet.Cell(row, 2).GetValue<string>(),
                        Price = worksheet.Cell(row, 3).GetValue<decimal>()
                    };

                    _context.Add(course);
                }
                await _context.SaveChangesAsync();
            }
        }
    }
    return RedirectToAction(nameof(Index));
}
    }
}
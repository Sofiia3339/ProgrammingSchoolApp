using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammingSchoolApp.Models;

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
        _context.Update(course);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
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
    }
}
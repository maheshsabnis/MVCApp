using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCApp.Data;
using MVCApp.Models;

namespace MVCApp.Controllers;

public class DepartmentsController : Controller
{
    private readonly AppDbContext _db;

    public DepartmentsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() =>
        View(await _db.Departments.AsNoTracking().OrderBy(d => d.DeptNo).ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.DeptNo == id);
        return item is null ? NotFound() : View(item);
    }

    public IActionResult Create() => View(new Department());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Department department)
    {
        if (ModelState.IsValid && await _db.Departments.AnyAsync(d => d.DeptNo == department.DeptNo))
            ModelState.AddModelError(nameof(Department.DeptNo), "A department with this Dept No already exists.");
        if (!ModelState.IsValid) return View(department);

        _db.Departments.Add(department);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.Departments.FindAsync(id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Department department)
    {
        if (id != department.DeptNo) return BadRequest();
        if (!ModelState.IsValid) return View(department);

        var existing = await _db.Departments.FindAsync(id);
        if (existing is null) return NotFound();

        existing.DeptName = department.DeptName;
        existing.Location = department.Location;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Departments.AsNoTracking().FirstOrDefaultAsync(d => d.DeptNo == id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.Departments.FindAsync(id);
        if (item is not null)
        {
            _db.Departments.Remove(item);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}

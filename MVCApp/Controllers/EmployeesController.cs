using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCApp.Data;
using MVCApp.Models;

namespace MVCApp.Controllers;

public class EmployeesController : Controller
{
    private readonly AppDbContext _db;

    public EmployeesController(AppDbContext db) => _db = db;

    private async Task LoadDepartmentsAsync(string? selected = null)
    {
        var departments = await _db.Departments.AsNoTracking().OrderBy(d => d.DeptName).ToListAsync();
        ViewBag.Departments = new SelectList(departments, nameof(Department.DeptName), nameof(Department.DeptName), selected);
    }

    public async Task<IActionResult> Index() =>
        View(await _db.Employees.AsNoTracking().OrderBy(e => e.EmpNo).ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var item = await _db.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.EmpNo == id);
        return item is null ? NotFound() : View(item);
    }

    public async Task<IActionResult> Create()
    {
        await LoadDepartmentsAsync();
        return View(new Employee());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Employee employee)
    {
        if (ModelState.IsValid && await _db.Employees.AnyAsync(e => e.EmpNo == employee.EmpNo))
            ModelState.AddModelError(nameof(Employee.EmpNo), "An employee with this Emp No already exists.");
        if (!ModelState.IsValid)
        {
            await LoadDepartmentsAsync(employee.DeptName);
            return View(employee);
        }

        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var item = await _db.Employees.FindAsync(id);
        if (item is null) return NotFound();
        await LoadDepartmentsAsync(item.DeptName);
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Employee employee)
    {
        if (id != employee.EmpNo) return BadRequest();
        if (!ModelState.IsValid)
        {
            await LoadDepartmentsAsync(employee.DeptName);
            return View(employee);
        }

        var existing = await _db.Employees.FindAsync(id);
        if (existing is null) return NotFound();

        existing.EmpName = employee.EmpName;
        existing.Salary = employee.Salary;
        existing.DeptName = employee.DeptName;
        existing.Designation = employee.Designation;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.EmpNo == id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _db.Employees.FindAsync(id);
        if (item is not null)
        {
            _db.Employees.Remove(item);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}

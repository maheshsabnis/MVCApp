using Microsoft.AspNetCore.Mvc;
using MVCApp.Models;
using MVCApp.Services;

namespace MVCApp.Controllers;

/// <summary>
/// Customer
/// </summary>
public class CustomersController : Controller
{
    private readonly ICustomerStore _store;

    public CustomersController(ICustomerStore store) => _store = store;

    public IActionResult Index() => View(_store.GetAll());

    public IActionResult Details(int id)
    {
        var customer = _store.Get(id);
        return customer is null ? NotFound() : View(customer);
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Create(Customer customer)
    {
        if (!ModelState.IsValid) return View(customer);
        _store.Add(customer);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Edit(int id)
    {
        var customer = _store.Get(id);
        return customer is null ? NotFound() : View(customer);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Customer customer)
    {
        if (id != customer.Id) return BadRequest();
        if (!ModelState.IsValid) return View(customer);
        return _store.Update(customer) ? RedirectToAction(nameof(Index)) : NotFound();
    }

    public IActionResult Delete(int id)
    {
        var customer = _store.Get(id);
        return customer is null ? NotFound() : View(customer);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        _store.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}

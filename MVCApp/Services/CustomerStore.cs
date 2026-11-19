using System.Collections.Concurrent;
using MVCApp.Models;

namespace MVCApp.Services;

public interface ICustomerStore
{
    IReadOnlyList<Customer> GetAll();
    Customer? Get(int id);
    Customer Add(Customer customer);
    bool Update(Customer customer);
    bool Delete(int id);
}

/// <summary>Thread-safe in-memory customer collection. Register as a singleton.</summary>
public class CustomerStore : ICustomerStore
{
    private readonly ConcurrentDictionary<int, Customer> _customers = new();
    private int _nextId;

    public CustomerStore()
    {
        Add(new Customer { Name = "Alice Johnson", Email = "alice@example.com", Phone = "555-0101", City = "Chicago" });
        Add(new Customer { Name = "Bob Smith", Email = "bob@example.com", Phone = "555-0102", City = "Denver" });
        Add(new Customer { Name = "Carol Lee", Email = "carol@example.com", Phone = "555-0103", City = "Austin" });
    }

    public IReadOnlyList<Customer> GetAll() =>
        _customers.Values.OrderBy(c => c.Id).ToList();

    public Customer? Get(int id) =>
        _customers.TryGetValue(id, out var c) ? c : null;

    public Customer Add(Customer customer)
    {
        customer.Id = Interlocked.Increment(ref _nextId);
        _customers[customer.Id] = customer;
        return customer;
    }

    public bool Update(Customer customer)
    {
        if (!_customers.ContainsKey(customer.Id)) return false;
        _customers[customer.Id] = customer;
        return true;
    }

    public bool Delete(int id) => _customers.TryRemove(id, out _);
}

using System.Collections.Concurrent;
using System.Threading;
using SalesApi.Models;

namespace SalesApi.Services;

public interface ISalesRepository
{
    IEnumerable<Sale> GetAll();
    Sale? Get(int id);
    Sale Create(SaleCreate create);
    bool Update(int id, SaleUpdate update);
    bool Delete(int id);
}

public class SalesRepository : ISalesRepository
{
    private readonly ConcurrentDictionary<int, Sale> _store = new();
    private int _currentId = 0;

    public SalesRepository()
    {
        // seed sample data
        Create(new SaleCreate("Sample Product", 1, 9.99m, DateTime.UtcNow));
    }

    public IEnumerable<Sale> GetAll() => _store.Values.OrderBy(s => s.Id);

    public Sale? Get(int id) => _store.TryGetValue(id, out var sale) ? sale : null;

    public Sale Create(SaleCreate create)
    {
        var id = Interlocked.Increment(ref _currentId);
        var sale = new Sale(id, create.Product, create.Quantity, create.Price, create.Date);
        _store[id] = sale;
        return sale;
    }

    public bool Update(int id, SaleUpdate update)
    {
        if (!_store.ContainsKey(id))
            return false;

        var updated = new Sale(id, update.Product, update.Quantity, update.Price, update.Date);
        _store[id] = updated;
        return true;
    }

    public bool Delete(int id) => _store.TryRemove(id, out _);
}

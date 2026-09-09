namespace SalesApi.Models;

public record Sale(int Id, string Product, int Quantity, decimal Price, DateTime Date);

public record SaleCreate(string Product, int Quantity, decimal Price, DateTime Date);

public record SaleUpdate(string Product, int Quantity, decimal Price, DateTime Date);

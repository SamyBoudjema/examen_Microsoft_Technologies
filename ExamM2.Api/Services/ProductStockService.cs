using ExamM2.Api.Models;

namespace ExamM2.Api.Services;

/// <summary>
/// Service singleton pour la gestion du stock en mémoire
/// </summary>
public class ProductStockService : IProductStockService
{
    private readonly List<Product> _products;

    public ProductStockService()
    {
        _products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 999.99m, Stock = 10 },
            new Product { Id = 2, Name = "Smartphone", Price = 599.99m, Stock = 25 },
            new Product { Id = 3, Name = "Headphones", Price = 89.99m, Stock = 50 },
            new Product { Id = 4, Name = "Keyboard", Price = 49.99m, Stock = 30 },
            new Product { Id = 5, Name = "Mouse", Price = 29.99m, Stock = 40 }
        };
    }

    public List<Product> GetAllProducts()
    {
        return _products;
    }

    public Product? GetProductById(int id)
    {
        return _products.FirstOrDefault(p => p.Id == id);
    }

    public bool IsStockAvailable(int productId, int quantity)
    {
        var product = GetProductById(productId);
        if (product == null)
        {
            return false;
        }
        return product.Stock >= quantity;
    }

    public bool UpdateStock(int productId, int quantity)
    {
        var product = GetProductById(productId);
        if (product == null)
        {
            return false;
        }

        if (product.Stock < quantity)
        {
            return false;
        }

        product.Stock -= quantity;
        return true;
    }
}

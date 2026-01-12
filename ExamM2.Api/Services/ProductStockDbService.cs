using ExamM2.Api.Data;
using ExamM2.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ExamM2.Api.Services;

/// <summary>
/// Service EF Core pour la gestion du stock avec base de données (Exercice 3)
/// </summary>
public class ProductStockDbService : IProductStockService
{
    private readonly ECommerceDbContext _context;

    public ProductStockDbService(ECommerceDbContext context)
    {
        _context = context;
    }

    public List<Product> GetAllProducts()
    {
        return _context.Products
            .Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock
            })
            .ToList();
    }

    public Product? GetProductById(int id)
    {
        var productEntity = _context.Products.Find(id);
        if (productEntity == null)
            return null;

        return new Product
        {
            Id = productEntity.Id,
            Name = productEntity.Name,
            Price = productEntity.Price,
            Stock = productEntity.Stock
        };
    }

    public bool IsStockAvailable(int productId, int quantity)
    {
        var product = _context.Products.Find(productId);
        if (product == null)
            return false;

        return product.Stock >= quantity;
    }

    public bool UpdateStock(int productId, int quantity)
    {
        var product = _context.Products.Find(productId);
        if (product == null)
            return false;

        if (product.Stock < quantity)
            return false;

        product.Stock -= quantity;
        _context.SaveChanges();
        return true;
    }
}

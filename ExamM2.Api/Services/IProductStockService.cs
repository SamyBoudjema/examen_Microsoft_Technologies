using ExamM2.Api.Models;

namespace ExamM2.Api.Services;

public interface IProductStockService
{
    List<Product> GetAllProducts();
    Product? GetProductById(int id);
    bool UpdateStock(int productId, int quantity);
    bool IsStockAvailable(int productId, int quantity);
}

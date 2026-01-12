using ExamM2.Api.Services;

namespace ExamM2.Api.Tests;

public class ProductStockServiceTests
{
    [Fact]
    public void GetAllProducts_ShouldReturn5Products()
    {
        var service = new ProductStockService();

        var products = service.GetAllProducts();

        Assert.NotNull(products);
        Assert.Equal(5, products.Count);
    }

    [Fact]
    public void GetProductById_WithValidId_ShouldReturnProduct()
    {
        var service = new ProductStockService();

        var product = service.GetProductById(1);

        Assert.NotNull(product);
        Assert.Equal(1, product.Id);
        Assert.Equal("Laptop", product.Name);
    }

    [Fact]
    public void GetProductById_WithInvalidId_ShouldReturnNull()
    {
        var service = new ProductStockService();

        var product = service.GetProductById(999);

        Assert.Null(product);
    }

    [Fact]
    public void IsStockAvailable_WithSufficientStock_ShouldReturnTrue()
    {
        var service = new ProductStockService();

        var isAvailable = service.IsStockAvailable(1, 5);

        Assert.True(isAvailable);
    }

    [Fact]
    public void IsStockAvailable_WithInsufficientStock_ShouldReturnFalse()
    {
        var service = new ProductStockService();

        var isAvailable = service.IsStockAvailable(1, 100);

        Assert.False(isAvailable);
    }

    [Fact]
    public void UpdateStock_WithValidQuantity_ShouldDecreaseStock()
    {
        var service = new ProductStockService();
        var initialStock = service.GetProductById(1)!.Stock;

        var result = service.UpdateStock(1, 3);
        var newStock = service.GetProductById(1)!.Stock;

        Assert.True(result);
        Assert.Equal(initialStock - 3, newStock);
    }

    [Fact]
    public void UpdateStock_WithInsufficientStock_ShouldReturnFalse()
    {
        var service = new ProductStockService();

        var result = service.UpdateStock(1, 1000);

        Assert.False(result);
    }
}

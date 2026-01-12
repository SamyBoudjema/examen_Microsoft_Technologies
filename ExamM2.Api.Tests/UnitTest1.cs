using ExamM2.Api.Services;
using ExamM2.Api.Models;

namespace ExamM2.Api.Tests;

public class ProductStockServiceTests
{
    [Fact]
    public void GetAllProducts_ShouldReturn5Products()
    {
        // Arrange
        var service = new ProductStockService();

        // Act
        var products = service.GetAllProducts();

        // Assert
        Assert.NotNull(products);
        Assert.Equal(5, products.Count);
    }

    [Fact]
    public void GetProductById_WithValidId_ShouldReturnProduct()
    {
        // Arrange
        var service = new ProductStockService();

        // Act
        var product = service.GetProductById(1);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(1, product.Id);
        Assert.Equal("Laptop", product.Name);
    }

    [Fact]
    public void GetProductById_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var service = new ProductStockService();

        // Act
        var product = service.GetProductById(999);

        // Assert
        Assert.Null(product);
    }

    [Fact]
    public void IsStockAvailable_WithSufficientStock_ShouldReturnTrue()
    {
        // Arrange
        var service = new ProductStockService();

        // Act
        var isAvailable = service.IsStockAvailable(1, 5);

        // Assert
        Assert.True(isAvailable);
    }

    [Fact]
    public void IsStockAvailable_WithInsufficientStock_ShouldReturnFalse()
    {
        // Arrange
        var service = new ProductStockService();

        // Act
        var isAvailable = service.IsStockAvailable(1, 100);

        // Assert
        Assert.False(isAvailable);
    }

    [Fact]
    public void UpdateStock_WithValidQuantity_ShouldDecreaseStock()
    {
        // Arrange
        var service = new ProductStockService();
        var initialStock = service.GetProductById(1)!.Stock;

        // Act
        var result = service.UpdateStock(1, 3);
        var newStock = service.GetProductById(1)!.Stock;

        // Assert
        Assert.True(result);
        Assert.Equal(initialStock - 3, newStock);
    }

    [Fact]
    public void UpdateStock_WithInsufficientStock_ShouldReturnFalse()
    {
        // Arrange
        var service = new ProductStockService();

        // Act
        var result = service.UpdateStock(1, 1000);

        // Assert
        Assert.False(result);
    }
}

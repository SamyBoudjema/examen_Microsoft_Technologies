using ExamM2.Api.DTOs;
using ExamM2.Api.Services;

namespace ExamM2.Api.Tests;

public class OrderServiceTests
{
    private IProductStockService CreateFreshStockService()
    {
        return new ProductStockService();
    }

    [Fact]
    public void CreateOrder_WithSimpleOrder_ShouldSucceed()
    {
        // Arrange
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 1, Quantity = 2 }
            }
        };

        // Act
        var (response, errors) = orderService.CreateOrder(request);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(errors);
        Assert.Single(response.Products);
        // 999.99 * 2 = 1999.98, mais total > 100 donc -5% = 1899.98
        Assert.Equal(1899.98m, response.Total);
    }

    [Fact]
    public void CreateOrder_WithQuantityGreaterThan5_ShouldApply10PercentDiscount()
    {
        // Arrange
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 5, Quantity = 6 } // Mouse: 29.99 * 6 = 179.94, avec -10% = 161.946
            }
        };

        // Act
        var (response, errors) = orderService.CreateOrder(request);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(errors);
        Assert.Equal(161.95m, response.Products[0].Total); // 179.94 * 0.9 = 161.946 arrondi à 161.95
    }

    [Fact]
    public void CreateOrder_WithTotalGreaterThan100_ShouldApply5PercentOrderDiscount()
    {
        // Arrange
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 3, Quantity = 2 } // 89.99 * 2 = 179.98
            }
        };

        // Act
        var (response, errors) = orderService.CreateOrder(request);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(errors);
        Assert.Contains(response.Discounts, d => d.Type == "order" && d.Value == 5m);
        Assert.Equal(170.98m, response.Total); // 179.98 * 0.95
    }

    [Fact]
    public void CreateOrder_WithValidPromoCode_ShouldApplyDiscount()
    {
        // Arrange
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 3, Quantity = 1 } // 89.99
            },
            PromoCode = "DISCOUNT20"
        };

        // Act
        var (response, errors) = orderService.CreateOrder(request);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(errors);
        Assert.Contains(response.Discounts, d => d.Type == "promo" && d.Value == 20m);
        Assert.Equal(71.99m, response.Total); // 89.99 * 0.8
    }

    [Fact]
    public void CreateOrder_WithPromoCodeBelow50Euros_ShouldFail()
    {
        // Arrange
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 5, Quantity = 1 } // 29.99
            },
            PromoCode = "DISCOUNT20"
        };

        // Act
        var (response, errors) = orderService.CreateOrder(request);

        // Assert
        Assert.Null(response);
        Assert.Single(errors);
        Assert.Contains("les codes promos ne sont valables qu'a partir de 50e d'achat", errors);
    }

    [Fact]
    public void CreateOrder_WithInvalidPromoCode_ShouldFail()
    {
        // Arrange
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 3, Quantity = 1 } // 89.99
            },
            PromoCode = "INVALID"
        };

        // Act
        var (response, errors) = orderService.CreateOrder(request);

        // Assert
        Assert.Null(response);
        Assert.Single(errors);
        Assert.Contains("le code promo est invalide", errors);
    }

    [Fact]
    public void CreateOrder_WithCumulativeDiscounts_ShouldApplyAdditively()
    {
        // Arrange
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 1, Quantity = 1 } // 999.99
            },
            PromoCode = "DISCOUNT10"
        };

        // Act
        var (response, errors) = orderService.CreateOrder(request);

        // Assert
        Assert.NotNull(response);
        Assert.Empty(errors);
        // Total > 100 donc -5% order + -10% promo = -15% total
        // 999.99 * 0.85 = 849.99
        Assert.Equal(2, response.Discounts.Count);
        Assert.Equal(849.99m, response.Total);
    }

    [Fact]
    public void CreateOrder_WithNonExistentProduct_ShouldFail()
    {
        // Arrange
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 999, Quantity = 1 }
            }
        };

        // Act
        var (response, errors) = orderService.CreateOrder(request);

        // Assert
        Assert.Null(response);
        Assert.Single(errors);
        Assert.Contains("le produit avec l'identifiant 999 n'existe pas", errors);
    }

    [Fact]
    public void CreateOrder_WithInsufficientStock_ShouldFail()
    {
        // Arrange
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 1, Quantity = 100 } // Stock = 10
            }
        };

        // Act
        var (response, errors) = orderService.CreateOrder(request);

        // Assert
        Assert.Null(response);
        Assert.Single(errors);
        Assert.Contains("il ne reste que 10 exemplaire pour le produit Laptop", errors);
    }

    [Fact]
    public void CreateOrder_WithMultipleErrors_ShouldReturnAllErrors()
    {
        // Arrange
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 999, Quantity = 1 }, // Produit inexistant
                new OrderProductDto { Id = 1, Quantity = 100 }  // Stock insuffisant
            }
        };

        // Act
        var (response, errors) = orderService.CreateOrder(request);

        // Assert
        Assert.Null(response);
        Assert.Equal(2, errors.Count);
        Assert.Contains("le produit avec l'identifiant 999 n'existe pas", errors);
        Assert.Contains("il ne reste que 10 exemplaire pour le produit Laptop", errors);
    }

    [Fact]
    public void CreateOrder_ShouldUpdateStock()
    {
        // Arrange
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var initialStock = stockService.GetProductById(1)!.Stock;
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 1, Quantity = 2 }
            }
        };

        // Act
        var (response, errors) = orderService.CreateOrder(request);
        var newStock = stockService.GetProductById(1)!.Stock;

        // Assert
        Assert.NotNull(response);
        Assert.Empty(errors);
        Assert.Equal(initialStock - 2, newStock);
    }
}

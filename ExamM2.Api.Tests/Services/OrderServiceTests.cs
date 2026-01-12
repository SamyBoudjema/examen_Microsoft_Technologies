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
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 1, Quantity = 2 }
            }
        };

        var (response, errors) = orderService.CreateOrder(request);

        Assert.NotNull(response);
        Assert.Empty(errors);
        Assert.Single(response.Products);
        Assert.Equal(1899.98m, response.Total);
    }

    [Fact]
    public void CreateOrder_WithQuantityGreaterThan5_ShouldApply10PercentDiscount()
    {
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 5, Quantity = 6 }
            }
        };

        var (response, errors) = orderService.CreateOrder(request);

        Assert.NotNull(response);
        Assert.Empty(errors);
        Assert.Equal(161.95m, response.Products[0].Total);
    }

    [Fact]
    public void CreateOrder_WithTotalGreaterThan100_ShouldApply5PercentOrderDiscount()
    {
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 3, Quantity = 2 }
            }
        };

        var (response, errors) = orderService.CreateOrder(request);

        Assert.NotNull(response);
        Assert.Empty(errors);
        Assert.Contains(response.Discounts, d => d.Type == "order" && d.Value == 5m);
        Assert.Equal(170.98m, response.Total);
    }

    [Fact]
    public void CreateOrder_WithValidPromoCode_ShouldApplyDiscount()
    {
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 3, Quantity = 1 }
            },
            PromoCode = "DISCOUNT20"
        };

        var (response, errors) = orderService.CreateOrder(request);

        Assert.NotNull(response);
        Assert.Empty(errors);
        Assert.Contains(response.Discounts, d => d.Type == "promo" && d.Value == 20m);
        Assert.Equal(71.99m, response.Total);
    }

    [Fact]
    public void CreateOrder_WithPromoCodeBelow50Euros_ShouldFail()
    {
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 5, Quantity = 1 }
            },
            PromoCode = "DISCOUNT20"
        };

        var (response, errors) = orderService.CreateOrder(request);

        Assert.Null(response);
        Assert.Single(errors);
        Assert.Contains("les codes promos ne sont valables qu'a partir de 50e d'achat", errors);
    }

    [Fact]
    public void CreateOrder_WithInvalidPromoCode_ShouldFail()
    {
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 3, Quantity = 1 }
            },
            PromoCode = "INVALID"
        };

        var (response, errors) = orderService.CreateOrder(request);

        Assert.Null(response);
        Assert.Single(errors);
        Assert.Contains("le code promo est invalide", errors);
    }

    [Fact]
    public void CreateOrder_WithCumulativeDiscounts_ShouldApplyAdditively()
    {
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 1, Quantity = 1 }
            },
            PromoCode = "DISCOUNT10"
        };

        var (response, errors) = orderService.CreateOrder(request);

        Assert.NotNull(response);
        Assert.Empty(errors);
        Assert.Equal(2, response.Discounts.Count);
        Assert.Equal(849.99m, response.Total);
    }

    [Fact]
    public void CreateOrder_WithNonExistentProduct_ShouldFail()
    {
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 999, Quantity = 1 }
            }
        };

        var (response, errors) = orderService.CreateOrder(request);

        Assert.Null(response);
        Assert.Single(errors);
        Assert.Contains("le produit avec l'identifiant 999 n'existe pas", errors);
    }

    [Fact]
    public void CreateOrder_WithInsufficientStock_ShouldFail()
    {
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 1, Quantity = 100 }
            }
        };

        var (response, errors) = orderService.CreateOrder(request);

        Assert.Null(response);
        Assert.Single(errors);
        Assert.Contains("il ne reste que 10 exemplaire pour le produit Laptop", errors);
    }

    [Fact]
    public void CreateOrder_WithMultipleErrors_ShouldReturnAllErrors()
    {
        var stockService = CreateFreshStockService();
        var orderService = new OrderService(stockService);
        var request = new OrderRequestDto
        {
            Products = new List<OrderProductDto>
            {
                new OrderProductDto { Id = 999, Quantity = 1 },
                new OrderProductDto { Id = 1, Quantity = 100 }
            }
        };

        var (response, errors) = orderService.CreateOrder(request);

        Assert.Null(response);
        Assert.Equal(2, errors.Count);
        Assert.Contains("le produit avec l'identifiant 999 n'existe pas", errors);
        Assert.Contains("il ne reste que 10 exemplaire pour le produit Laptop", errors);
    }

    [Fact]
    public void CreateOrder_ShouldUpdateStock()
    {
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

        var (response, errors) = orderService.CreateOrder(request);
        var newStock = stockService.GetProductById(1)!.Stock;

        Assert.NotNull(response);
        Assert.Empty(errors);
        Assert.Equal(initialStock - 2, newStock);
    }
}

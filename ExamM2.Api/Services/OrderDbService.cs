using ExamM2.Api.DTOs;
using ExamM2.Api.Models;

namespace ExamM2.Api.Services;

/// <summary>
/// Service pour les commandes utilisant la base de données (Exercice 3)
/// </summary>
public class OrderDbService
{
    private readonly ProductStockDbService _productStockService;
    private readonly PromoCodeDbService _promoCodeService;

    public OrderDbService(ProductStockDbService productStockService, PromoCodeDbService promoCodeService)
    {
        _productStockService = productStockService;
        _promoCodeService = promoCodeService;
    }

    public (OrderResponseDto? response, List<string> errors) CreateOrder(OrderRequestDto orderRequest)
    {
        var errors = new List<string>();
        
        var validatedProducts = ValidateProducts(orderRequest.Products, errors);
        
        if (errors.Any())
        {
            return (null, errors);
        }

        var productDetails = CalculateProductDetails(validatedProducts);
        var subtotal = productDetails.Sum(p => p.Total);
        
        decimal? promoDiscount = ValidatePromoCode(orderRequest.PromoCode, subtotal, errors);
        
        if (errors.Any())
        {
            return (null, errors);
        }

        var discounts = CalculateDiscounts(subtotal, promoDiscount);
        var total = CalculateFinalTotal(subtotal, discounts);

        UpdateStocks(validatedProducts);

        var response = new OrderResponseDto
        {
            Products = productDetails,
            Discounts = discounts,
            Total = Math.Round(total, 2)
        };

        return (response, errors);
    }

    private List<(Product product, int quantity)> ValidateProducts(List<OrderProductDto> orderProducts, List<string> errors)
    {
        var validatedProducts = new List<(Product product, int quantity)>();

        foreach (var orderProduct in orderProducts)
        {
            var product = _productStockService.GetProductById(orderProduct.Id);
            
            if (product == null)
            {
                errors.Add($"le produit avec l'identifiant {orderProduct.Id} n'existe pas");
                continue;
            }

            if (!_productStockService.IsStockAvailable(orderProduct.Id, orderProduct.Quantity))
            {
                errors.Add($"il ne reste que {product.Stock} exemplaire pour le produit {product.Name}");
                continue;
            }

            validatedProducts.Add((product, orderProduct.Quantity));
        }

        return validatedProducts;
    }

    private List<OrderProductDetailDto> CalculateProductDetails(List<(Product product, int quantity)> validatedProducts)
    {
        var productDetails = new List<OrderProductDetailDto>();

        foreach (var (product, quantity) in validatedProducts)
        {
            decimal unitPrice = product.Price;
            decimal discount = 0m;

            if (quantity > 5)
            {
                discount = unitPrice * 0.10m;
                unitPrice -= discount;
            }

            var total = unitPrice * quantity;

            productDetails.Add(new OrderProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = quantity,
                PricePerUnit = Math.Round(unitPrice, 2),
                Total = Math.Round(total, 2)
            });
        }

        return productDetails;
    }

    private decimal? ValidatePromoCode(string? promoCode, decimal subtotal, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(promoCode))
        {
            return null;
        }

        if (!_promoCodeService.IsValidPromoCode(promoCode))
        {
            errors.Add($"le code promo {promoCode} n'est pas valide");
            return null;
        }

        return _promoCodeService.GetDiscountPercentage(promoCode);
    }

    private List<DiscountDto> CalculateDiscounts(decimal subtotal, decimal? promoDiscount)
    {
        var discounts = new List<DiscountDto>();

        if (subtotal > 100m)
        {
            var autoDiscount = subtotal * 0.05m;
            discounts.Add(new DiscountDto
            {
                Type = "auto",
                Value = Math.Round(autoDiscount, 2)
            });
        }

        if (promoDiscount.HasValue)
        {
            var promoAmount = subtotal * (promoDiscount.Value / 100m);
            discounts.Add(new DiscountDto
            {
                Type = "promo_code",
                Value = Math.Round(promoAmount, 2)
            });
        }

        return discounts;
    }

    private decimal CalculateFinalTotal(decimal subtotal, List<DiscountDto> discounts)
    {
        var totalDiscount = discounts.Sum(d => d.Value);
        return subtotal - totalDiscount;
    }

    private void UpdateStocks(List<(Product product, int quantity)> validatedProducts)
    {
        foreach (var (product, quantity) in validatedProducts)
        {
            _productStockService.UpdateStock(product.Id, quantity);
        }
    }
}

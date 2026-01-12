using ExamM2.Api.DTOs;
using ExamM2.Api.Models;

namespace ExamM2.Api.Services;

public class OrderService : IOrderService
{
    private readonly IProductStockService _productStockService;
    private readonly Dictionary<string, decimal> _promoCodes = new()
    {
        { "DISCOUNT20", 20m },
        { "DISCOUNT10", 10m }
    };

    public OrderService(IProductStockService productStockService)
    {
        _productStockService = productStockService;
    }

    public (OrderResponseDto? response, List<string> errors) CreateOrder(OrderRequestDto orderRequest)
    {
        var errors = new List<string>();
        
        // Validation des produits et stocks
        var validatedProducts = ValidateProducts(orderRequest.Products, errors);
        
        if (errors.Any())
        {
            return (null, errors);
        }

        // Calcul du total et des remises
        var productDetails = CalculateProductDetails(validatedProducts);
        var subtotal = productDetails.Sum(p => p.Total);
        
        // Validation du code promo
        decimal? promoDiscount = ValidatePromoCode(orderRequest.PromoCode, subtotal, errors);
        
        if (errors.Any())
        {
            return (null, errors);
        }

        // Calcul des remises
        var discounts = CalculateDiscounts(subtotal, promoDiscount);
        var total = CalculateFinalTotal(subtotal, discounts);

        // Mise à jour des stocks
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
            var subtotal = product.Price * quantity;
            
            // Règle 1: Remise automatique de 10% si quantité > 5
            if (quantity > 5)
            {
                subtotal = subtotal * 0.9m;
            }

            productDetails.Add(new OrderProductDetailDto
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = quantity,
                PricePerUnit = product.Price,
                Total = Math.Round(subtotal, 2)
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

        // Règle 1: Le code promo doit exister
        if (!_promoCodes.ContainsKey(promoCode))
        {
            errors.Add("le code promo est invalide");
            return null;
        }

        // Règle 2: Les codes promos sont valides seulement si la commande dépasse 50 euros
        if (subtotal < 50)
        {
            errors.Add("les codes promos ne sont valables qu'a partir de 50e d'achat");
            return null;
        }

        return _promoCodes[promoCode];
    }

    private List<DiscountDto> CalculateDiscounts(decimal subtotal, decimal? promoDiscount)
    {
        var discounts = new List<DiscountDto>();

        // Règle 2: Remise automatique de 5% si total > 100 euros
        if (subtotal > 100)
        {
            discounts.Add(new DiscountDto
            {
                Type = "order",
                Value = 5m
            });
        }

        // Ajout du code promo
        if (promoDiscount.HasValue)
        {
            discounts.Add(new DiscountDto
            {
                Type = "promo",
                Value = promoDiscount.Value
            });
        }

        return discounts;
    }

    private decimal CalculateFinalTotal(decimal subtotal, List<DiscountDto> discounts)
    {
        // Règle 3: Les remises se cumulent de façon additive
        var totalDiscountPercent = discounts.Sum(d => d.Value);
        var total = subtotal * (100 - totalDiscountPercent) / 100;
        return total;
    }

    private void UpdateStocks(List<(Product product, int quantity)> validatedProducts)
    {
        foreach (var (product, quantity) in validatedProducts)
        {
            _productStockService.UpdateStock(product.Id, quantity);
        }
    }
}

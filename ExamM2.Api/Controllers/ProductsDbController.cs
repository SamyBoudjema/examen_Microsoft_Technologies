using ExamM2.Api.DTOs;
using ExamM2.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamM2.Api.Controllers;

/// <summary>
/// Controller pour les produits utilisant EF Core (Exercice 3)
/// Endpoints: /api/productsdb
/// </summary>
[ApiController]
[Route("api/productsdb")]
public class ProductsDbController : ControllerBase
{
    private readonly ProductStockDbService _productStockDbService;

    public ProductsDbController(ProductStockDbService productStockDbService)
    {
        _productStockDbService = productStockDbService;
    }

    [HttpGet]
    public ActionResult<List<ProductDto>> GetProducts()
    {
        var products = _productStockDbService.GetAllProducts();
        var productDtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Stock = p.Stock
        }).ToList();

        return Ok(productDtos);
    }
}

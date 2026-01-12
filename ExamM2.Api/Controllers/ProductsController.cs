using ExamM2.Api.DTOs;
using ExamM2.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamM2.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductStockService _productStockService;

    public ProductsController(IProductStockService productStockService)
    {
        _productStockService = productStockService;
    }

    [HttpGet]
    public ActionResult<List<ProductDto>> GetProducts()
    {
        var products = _productStockService.GetAllProducts();
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

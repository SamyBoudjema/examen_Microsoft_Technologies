using ExamM2.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamM2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DebugController : ControllerBase
{
    private readonly ECommerceDbContext _context;

    public DebugController(ECommerceDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Affiche l'état complet de la base de données (debug uniquement)
    /// </summary>
    [HttpGet("database")]
    public async Task<IActionResult> GetDatabaseState()
    {
        var products = await _context.Products.ToListAsync();
        var promoCodes = await _context.PromoCodes.ToListAsync();

        return Ok(new
        {
            products = products.Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                p.Stock
            }),
            promoCodes = promoCodes.Select(pc => new
            {
                pc.Id,
                pc.Code,
                pc.DiscountPercentage,
                pc.IsActive
            })
        });
    }
}

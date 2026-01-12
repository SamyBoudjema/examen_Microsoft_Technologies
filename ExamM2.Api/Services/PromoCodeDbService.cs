using ExamM2.Api.Data;
using ExamM2.Api.Models.Entities;

namespace ExamM2.Api.Services;

/// <summary>
/// Service EF Core pour la gestion des codes promotionnels (Exercice 3)
/// </summary>
public class PromoCodeDbService
{
    private readonly ECommerceDbContext _context;

    public PromoCodeDbService(ECommerceDbContext context)
    {
        _context = context;
    }

    public PromoCodeEntity? GetPromoCodeByCode(string code)
    {
        return _context.PromoCodes.FirstOrDefault(p => p.Code == code && p.IsActive);
    }

    public decimal GetDiscountPercentage(string code)
    {
        var promoCode = GetPromoCodeByCode(code);
        return promoCode?.DiscountPercentage ?? 0m;
    }

    public bool IsValidPromoCode(string code)
    {
        return GetPromoCodeByCode(code) != null;
    }
}

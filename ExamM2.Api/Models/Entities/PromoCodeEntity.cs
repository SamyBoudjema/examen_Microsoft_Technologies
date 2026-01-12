namespace ExamM2.Api.Models.Entities;

/// <summary>
/// Entité EF Core pour les codes promotionnels (Exercice 3)
/// </summary>
public class PromoCodeEntity
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public decimal DiscountPercentage { get; set; }
    public bool IsActive { get; set; }
}

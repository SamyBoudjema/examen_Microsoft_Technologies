namespace ExamM2.Api.Models.Entities;

/// <summary>
/// Entité EF Core pour le catalogue des produits (Exercice 3)
/// </summary>
public class ProductEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

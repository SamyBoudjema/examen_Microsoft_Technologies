using ExamM2.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamM2.Api.Data;

/// <summary>
/// DbContext EF Core pour l'API e-commerce (Exercice 3)
/// </summary>
public class ECommerceDbContext : DbContext
{
    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
    {
    }

    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<PromoCodeEntity> PromoCodes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed des produits (identiques à l'exercice 1)
        modelBuilder.Entity<ProductEntity>().HasData(
            new ProductEntity { Id = 1, Name = "Product A", Price = 100m, Stock = 10 },
            new ProductEntity { Id = 2, Name = "Product B", Price = 200m, Stock = 5 },
            new ProductEntity { Id = 3, Name = "Product C", Price = 50m, Stock = 20 }
        );

        // Seed des codes promo (identiques à l'exercice 1)
        modelBuilder.Entity<PromoCodeEntity>().HasData(
            new PromoCodeEntity { Id = 1, Code = "DISCOUNT10", DiscountPercentage = 10m, IsActive = true },
            new PromoCodeEntity { Id = 2, Code = "DISCOUNT20", DiscountPercentage = 20m, IsActive = true },
            new PromoCodeEntity { Id = 3, Code = "EXPIRED", DiscountPercentage = 15m, IsActive = false }
        );
    }
}

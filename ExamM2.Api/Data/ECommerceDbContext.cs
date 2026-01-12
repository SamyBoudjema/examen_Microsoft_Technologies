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

        // Seed des produits IT
        modelBuilder.Entity<ProductEntity>().HasData(
            new ProductEntity { Id = 1, Name = "RAM Corsair Vengeance 32GB DDR5", Price = 1000m, Stock = 25 },
            new ProductEntity { Id = 2, Name = "SSD Samsung 980 PRO 2TB NVMe", Price = 250m, Stock = 15 },
            new ProductEntity { Id = 3, Name = "iPhone 15 Pro 256GB", Price = 1200m, Stock = 8 }
        );

        // Seed des codes promo
        modelBuilder.Entity<PromoCodeEntity>().HasData(
            new PromoCodeEntity { Id = 1, Code = "DISCOUNT10", DiscountPercentage = 10m, IsActive = true },
            new PromoCodeEntity { Id = 2, Code = "DISCOUNT20", DiscountPercentage = 20m, IsActive = true },
            new PromoCodeEntity { Id = 3, Code = "EXPIRED", DiscountPercentage = 15m, IsActive = false }
        );
    }
}

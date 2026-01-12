using ExamM2.Api.Data;
using ExamM2.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==================== EXERCICE 1 : Services Singleton ====================
// Services en mémoire pour l'exercice 1 (NE PAS MODIFIER)
builder.Services.AddSingleton<ProductStockService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Résolveur IProductStockService pour l'exercice 1
builder.Services.AddSingleton<IProductStockService>(sp => sp.GetRequiredService<ProductStockService>());

// ==================== EXERCICE 3 : Services EF Core ====================
// Configuration EF Core InMemory
builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseInMemoryDatabase("ECommerceDb"));

// Services DB pour l'exercice 3 (nouveaux endpoints /productsdb et /ordersdb)
builder.Services.AddScoped<ProductStockDbService>();
builder.Services.AddScoped<PromoCodeDbService>();
builder.Services.AddScoped<OrderDbService>();

// ==================== Health Checks ====================
builder.Services.AddHealthChecks();

builder.Services.AddControllers();

// ==================== OpenAPI Configuration ====================
builder.Services.AddOpenApi();

var app = builder.Build();

// ==================== EXERCICE 3 : Seed de la base de données ====================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
    context.Database.EnsureCreated(); // Crée la DB et applique le seed
}

// ==================== OpenAPI Documentation ====================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Disponible sur /openapi/v1.json
}

// ==================== Health Checks ====================
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

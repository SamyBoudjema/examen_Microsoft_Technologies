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

// ==================== Swagger Configuration ====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ==================== EXERCICE 3 : Seed de la base de données ====================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
    context.Database.EnsureCreated(); // Crée la DB et applique le seed
}

// ==================== Swagger UI ====================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ExamM2 API v1");
        options.RoutePrefix = string.Empty; // Swagger à la racine (http://localhost:5149/)
        options.DocumentTitle = "ExamM2 - API E-Commerce";
    });
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

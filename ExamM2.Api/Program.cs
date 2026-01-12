using ExamM2.Api.Data;
using ExamM2.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Exercice 1 : Services Singleton (NE PAS MODIFIER)
builder.Services.AddSingleton<ProductStockService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddSingleton<IProductStockService>(sp => sp.GetRequiredService<ProductStockService>());

// Exercice 3 : EF Core InMemory
builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseInMemoryDatabase("ECommerceDb"));
builder.Services.AddScoped<ProductStockDbService>();
builder.Services.AddScoped<PromoCodeDbService>();
builder.Services.AddScoped<OrderDbService>();

// Health Checks & OpenAPI
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Seed DB (Exercice 3)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ECommerceDbContext>();
    context.Database.EnsureCreated();
}

// Swagger UI & OpenAPI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseDefaultFiles();
app.UseStaticFiles();

// Health endpoint
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResponseWriter = HealthChecks.UI.Client.UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

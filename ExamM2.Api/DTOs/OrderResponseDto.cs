namespace ExamM2.Api.DTOs;

public class OrderResponseDto
{
    public List<OrderProductDetailDto> Products { get; set; } = new();
    public List<DiscountDto> Discounts { get; set; } = new();
    public decimal Total { get; set; }
}

public class OrderProductDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal PricePerUnit { get; set; }
    public decimal Total { get; set; }
}

public class DiscountDto
{
    public string Type { get; set; } = string.Empty;
    public decimal Value { get; set; }
}

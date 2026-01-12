namespace ExamM2.Api.DTOs;

public class OrderRequestDto
{
    public List<OrderProductDto> Products { get; set; } = new();
    public string? PromoCode { get; set; }
}

public class OrderProductDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
}

using ExamM2.Api.DTOs;

namespace ExamM2.Api.Services;

public interface IOrderService
{
    (OrderResponseDto? response, List<string> errors) CreateOrder(OrderRequestDto orderRequest);
}

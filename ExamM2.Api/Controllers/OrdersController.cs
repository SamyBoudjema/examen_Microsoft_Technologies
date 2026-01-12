using ExamM2.Api.DTOs;
using ExamM2.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamM2.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public ActionResult<OrderResponseDto> CreateOrder([FromBody] OrderRequestDto orderRequest)
    {
        var (response, errors) = _orderService.CreateOrder(orderRequest);

        if (errors.Any())
        {
            var errorResponse = new ErrorResponseDto
            {
                Errors = errors
            };
            return BadRequest(errorResponse);
        }

        return Ok(response);
    }
}

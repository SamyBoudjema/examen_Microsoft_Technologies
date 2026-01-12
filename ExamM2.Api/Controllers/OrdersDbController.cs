using ExamM2.Api.DTOs;
using ExamM2.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamM2.Api.Controllers;

/// <summary>
/// Controller pour les commandes utilisant EF Core (Exercice 3)
/// Endpoints: /api/ordersdb
/// </summary>
[ApiController]
[Route("api/ordersdb")]
public class OrdersDbController : ControllerBase
{
    private readonly OrderDbService _orderDbService;

    public OrdersDbController(OrderDbService orderDbService)
    {
        _orderDbService = orderDbService;
    }

    [HttpPost]
    public ActionResult<OrderResponseDto> CreateOrder([FromBody] OrderRequestDto orderRequest)
    {
        var (response, errors) = _orderDbService.CreateOrder(orderRequest);

        if (errors.Any())
        {
            return BadRequest(new ErrorResponseDto
            {
                Errors = errors
            });
        }

        return Ok(response);
    }
}

using BusinessLogicLayer;
using BusinessLogicLayer.Dtos.Orders;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders(CancellationToken cancellationToken = default)
        {
            var orders = await _orderService.GetAllOrdersAsync(cancellationToken);
            return Ok(orders);
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken = default)
        {
            var order = await _orderService.GetOrderByIdAsync(id, cancellationToken);
            return Ok(order);
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id, CancellationToken cancellationToken = default)
        {
            var deletedOrder = await _orderService.DeleteOrderByIdAsync(id, cancellationToken);
            return Ok(deletedOrder);
        }

        [Authorize]
        [HttpPost("api/v1/user/orders")]
        public async Task<IActionResult> CreateOrder(OrderCreateDto orderCreateDto, CancellationToken cancellationToken = default)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            orderCreateDto.UserName = userName;
            var order = await _orderService.CreateOrderAsync(orderCreateDto, cancellationToken);
            return Ok(order);
        }


        [Authorize]
        [HttpGet("api/v1/user/orders")]
        public async Task<IActionResult> GetUserOrders(CancellationToken cancellationToken = default)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var orders = await _orderService.GetUserOrders(userName, cancellationToken);
            return Ok(orders);
        }

        [Authorize]
        [HttpDelete("api/v1/user/orders/{id}")]
        public async Task<IActionResult> DeleteUserOrder(Guid id, CancellationToken cancellationToken = default)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var deletedOrder = await _orderService.DeleteUserOrderByIdAsync(id, userName, cancellationToken);
            return Ok(deletedOrder);
        }
    }
}

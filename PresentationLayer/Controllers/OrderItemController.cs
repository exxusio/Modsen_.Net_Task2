using BusinessLogicLayer;
using BusinessLogicLayer.Dtos.OrderItems;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    [Route("api/v1/order_items")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrderItems(CancellationToken cancellationToken = default)
        {
            var orderItems = await _orderItemService.GetAllOrderItemsAsync(cancellationToken);
            return Ok(orderItems);
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItemById(Guid id, CancellationToken cancellationToken = default)
        {
            var orderItem = await _orderItemService.GetOrderItemByIdAsync(id, cancellationToken);
            return Ok(orderItem);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrderItem(OrderItemCreateDto orderItemCreateDto, CancellationToken cancellationToken = default)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            orderItemCreateDto.UserName = userName;
            var orderItem = await _orderItemService.CreateOrderItemAsync(orderItemCreateDto, cancellationToken);
            return Ok(orderItem);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(Guid id, OrderItemUpdateDto orderItemUpdateDto, CancellationToken cancellationToken = default)
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            orderItemUpdateDto.UserName = userName;
            orderItemUpdateDto.Id = id;
            var orderItem = await _orderItemService.UpdateOrderItemAsync(orderItemUpdateDto, cancellationToken);
            return Ok(orderItem);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(Guid id, CancellationToken cancellationToken = default)
        {
            var deletedOrderItem = await _orderItemService.DeleteOrderItemByIdAsync(id, cancellationToken);
            return Ok(deletedOrderItem);
        }
    }
}

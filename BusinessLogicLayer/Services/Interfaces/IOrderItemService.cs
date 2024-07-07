using BusinessLogicLayer.Dtos.OrderItems;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<IEnumerable<OrderItemReadDto>> GetAllOrderItemsAsync(CancellationToken cancellationToken = default);
        Task<OrderItemReadDto> GetOrderItemByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<OrderItemReadDto> CreateOrderItemAsync(OrderItemCreateDto OrderItemCreateDto, CancellationToken cancellationToken = default);
        Task<OrderItemReadDto> DeleteOrderItemByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<OrderItemReadDto> UpdateOrderItemAsync(OrderItemUpdateDto orderItemUpdateDto, CancellationToken cancellationToken = default);
    }
}

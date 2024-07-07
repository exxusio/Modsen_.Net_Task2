using BusinessLogicLayer.Dtos.Orders;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync(CancellationToken cancellationToken = default);
        Task<OrderReadDto> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<OrderReadDto> CreateOrderAsync(OrderCreateDto OrderCreateDto, CancellationToken cancellationToken = default);
        Task<OrderReadDto> DeleteOrderByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<OrderReadDto>> GetUserOrders(string username, CancellationToken cancellationToken = default);
        Task<OrderReadDto> DeleteUserOrderByIdAsync(Guid id, string userName, CancellationToken cancellationToken = default);
    }
}

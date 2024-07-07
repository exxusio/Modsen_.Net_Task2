using AutoMapper;
using BusinessLogicLayer.Dtos.OrderItems;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Data.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Implementations
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderItemReadDto> CreateOrderItemAsync(OrderItemCreateDto orderItemCreateDto, CancellationToken cancellationToken = default)
        {
            if (orderItemCreateDto == null)
            {
                throw new ArgumentNullException(nameof(orderItemCreateDto));
            }

            var orderItemRepository = _unitOfWork.GetRepository<OrderItem>();
            var productRepository = _unitOfWork.GetRepository<Product>();
            var orderRepository = _unitOfWork.GetRepository<Order>();

            var orderItem = _mapper.Map<OrderItem>(orderItemCreateDto);

            var existingProduct = await productRepository.GetByIdAsync(orderItemCreateDto.ProductId, cancellationToken);
            if (existingProduct == null)
            {
                throw new NotFoundException($"Product not found with id: {orderItemCreateDto.ProductId}");
            }

            var existingOrder = (await orderRepository.GetByPredicateAsync(o => o.User.UserName == orderItemCreateDto.UserName &&
                                                                             o.Id == orderItemCreateDto.OrderId, cancellationToken)).FirstOrDefault();
            if (existingOrder == null)
            {
                throw new NotFoundException($"Order not found with id: {orderItemCreateDto.OrderId}");
            }

            orderItem.Product = existingProduct;
            orderItem.Order = existingOrder;

            await orderItemRepository.AddAsync(orderItem, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<OrderItemReadDto>(orderItem);
        }

        public async Task<OrderItemReadDto> DeleteOrderItemByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var orderItemRepository = _unitOfWork.GetRepository<OrderItem>();

            var orderItem = await orderItemRepository.GetByIdAsync(id, cancellationToken);
            if (orderItem == null)
            {
                throw new NotFoundException($"Order item not found with id: {id}");
            }

            orderItemRepository.Delete(orderItem);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<OrderItemReadDto>(orderItem);
        }

        public async Task<IEnumerable<OrderItemReadDto>> GetAllOrderItemsAsync(CancellationToken cancellationToken = default)
        {
            var orderItemRepository = _unitOfWork.GetRepository<OrderItem>();

            var orderItems = await orderItemRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<OrderItemReadDto>>(orderItems);
        }

        public async Task<OrderItemReadDto> GetOrderItemByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var orderItemRepository = _unitOfWork.GetRepository<OrderItem>();

            var orderItem = await orderItemRepository.GetByIdAsync(id, cancellationToken);
            if (orderItem == null)
            {
                throw new KeyNotFoundException($"Order item not found with id: {id}");
            }

            return _mapper.Map<OrderItemReadDto>(orderItem);
        }

        public async Task<OrderItemReadDto> UpdateOrderItemAsync(OrderItemUpdateDto orderItemUpdateDto, CancellationToken cancellationToken = default)
        {
            if (orderItemUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(orderItemUpdateDto));
            }

            var orderItemRepository = _unitOfWork.GetRepository<OrderItem>();

            var existingOrderItem = (await orderItemRepository.GetByPredicateAsync(oi => oi.Id == orderItemUpdateDto.Id &&
                                            oi.Order.User.UserName == orderItemUpdateDto.UserName, cancellationToken)).FirstOrDefault();
            if (existingOrderItem == null)
            {
                throw new KeyNotFoundException($"Order item not found with id: {orderItemUpdateDto.Id}");
            }

            var newOrderItem = _mapper.Map(orderItemUpdateDto, existingOrderItem);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<OrderItemReadDto>(newOrderItem);
        }
    }
}

using AutoMapper;
using BusinessLogicLayer.Dtos.Orders;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Data.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<OrderReadDto> CreateOrderAsync(OrderCreateDto orderCreateDto, CancellationToken cancellationToken = default)
        {
            if (orderCreateDto == null)
            {
                throw new ArgumentNullException(nameof(orderCreateDto));
            }

            var userRepository = _unitOfWork.GetRepository<User>();
            var orderRepository = _unitOfWork.GetRepository<Order>();

            var user = (await userRepository.GetByPredicateAsync(u => u.UserName == orderCreateDto.UserName)).FirstOrDefault();
            if (orderCreateDto == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var order = _mapper.Map<Order>(orderCreateDto);
            order.User = user;

            await orderRepository.AddAsync(order, cancellationToken);
            await orderRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<OrderReadDto>(order);
        }

        public async Task<OrderReadDto> DeleteOrderByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var orderRepository = _unitOfWork.GetRepository<Order>();

            var order = await orderRepository.GetByIdAsync(id, cancellationToken);
            if (order == null)
            {
                throw new NotFoundException($"Order not found with id: {id}");
            }

            orderRepository.Delete(order);
            await orderRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<OrderReadDto>(order);
        }

        public async Task<OrderReadDto> DeleteUserOrderByIdAsync(Guid id, string userName, CancellationToken cancellationToken = default)
        {
            var orderRepository = _unitOfWork.GetRepository<Order>();

            var order = (await orderRepository.GetByPredicateAsync(o => o.Id == id &&
             o.User.UserName == userName, cancellationToken)).FirstOrDefault();
            if (order == null)
            {
                throw new NotFoundException($"Order not found with id: {id}");
            }

            orderRepository.Delete(order);
            await orderRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<OrderReadDto>(order);
        }

        public async Task<IEnumerable<OrderReadDto>> GetAllOrdersAsync(CancellationToken cancellationToken = default)
        {
            var orderRepository = _unitOfWork.GetRepository<Order>();

            var orders = await orderRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<OrderReadDto>>(orders);
        }

        public async Task<OrderReadDto> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var orderRepository = _unitOfWork.GetRepository<Order>();

            var order = await orderRepository.GetByIdAsync(id, cancellationToken);
            if (order == null)
            {
                throw new NotFoundException($"Order not found with id: {id}");
            }

            return _mapper.Map<OrderReadDto>(order);
        }

        public async Task<IEnumerable<OrderReadDto>> GetUserOrders(string username, CancellationToken cancellationToken = default)
        {
            var userRepository = _unitOfWork.GetRepository<User>();

            var orders = (await userRepository.GetByPredicateAsync(u => u.UserName == username, cancellationToken)).FirstOrDefault().Orders;
            return _mapper.Map<IEnumerable<OrderReadDto>>(orders);
        }
    }
}

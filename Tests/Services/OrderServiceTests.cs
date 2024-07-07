using BusinessLogicLayer.Dtos.OrderItems;
using BusinessLogicLayer.Dtos.Orders;
using BusinessLogicLayer.Dtos.Products;
using System.Linq.Expressions;

namespace Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IOrderService _orderService;
        private readonly CancellationToken cancellationToken;

        public OrderServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _orderService = new OrderService(_mockUnitOfWork.Object, _mockMapper.Object);
            cancellationToken = default;
        }

        [Fact]
        public async Task CreateOrderAsync_ValidDto_ReturnsOrderReadDto()
        {
            var createDto = new OrderCreateDto
            {
                UserName = "testuser",
                OrderItems = new List<OrderItemCreateNewOrderDto>
                {
                    new OrderItemCreateNewOrderDto { Amount = 5, ProductId = Guid.NewGuid() }
                }
            };

            var user = new User { UserName = "testuser", Id = Guid.NewGuid() };
            var order = new Order
            {
                Id = Guid.NewGuid(),
                User = user,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = Guid.NewGuid(), Amount = 5, ProductId = Guid.NewGuid(), OrderId = Guid.NewGuid() }
                }
            };
            var expectedReadDto = new OrderReadDto
            {
                Id = order.Id,
                OrderItems = new List<OrderItemReadDto>
                {
                    new OrderItemReadDto { Amount = 5, Product = new ProductReadDto { Id = Guid.NewGuid(), Name = "Test Product" } }
                }
            };

            var mockUserRepository = new Mock<IRepository<User>>();
            var mockOrderRepository = new Mock<IRepository<Order>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<User>()).Returns(mockUserRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Order>()).Returns(mockOrderRepository.Object);

            mockUserRepository.Setup(repo => repo.GetByPredicateAsync(It.IsAny<Expression<Func<User, bool>>>(), cancellationToken))
                .ReturnsAsync(new List<User> { user });
            _mockMapper.Setup(m => m.Map<Order>(createDto)).Returns(order);
            mockOrderRepository.Setup(repo => repo.AddAsync(order, cancellationToken)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(cancellationToken)).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<OrderReadDto>(order)).Returns(expectedReadDto);

            var result = await _orderService.CreateOrderAsync(createDto);

            result.Should().NotBeNull();
            result.Id.Should().Be(order.Id);
            result.OrderItems.Should().HaveCount(1);
        }

        [Fact]
        public async Task DeleteOrderByIdAsync_ExistingId_ReturnsOrderReadDto()
        {
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = Guid.NewGuid(), Amount = 5, ProductId = Guid.NewGuid(), OrderId = orderId }
                }
            };
            var expectedReadDto = new OrderReadDto
            {
                Id = order.Id,
                OrderItems = new List<OrderItemReadDto>
                {
                    new OrderItemReadDto { Amount = 5, Product = new ProductReadDto { Id = Guid.NewGuid(), Name = "Test Product" } }
                }
            };

            var mockOrderRepository = new Mock<IRepository<Order>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<Order>()).Returns(mockOrderRepository.Object);

            mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId, cancellationToken)).ReturnsAsync(order);
            mockOrderRepository.Setup(repo => repo.Delete(order));
            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(cancellationToken)).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<OrderReadDto>(order)).Returns(expectedReadDto);

            var result = await _orderService.DeleteOrderByIdAsync(orderId);

            result.Should().NotBeNull();
            result.Id.Should().Be(orderId);
        }

        [Fact]
        public async Task GetAllOrdersAsync_NoConditions_ReturnsListOfOrderReadDto()
        {
            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), OrderItems = new List<OrderItem> { new OrderItem { Id = Guid.NewGuid(), Amount = 5, ProductId = Guid.NewGuid(), OrderId = Guid.NewGuid() } } },
                new Order { Id = Guid.NewGuid(), OrderItems = new List<OrderItem> { new OrderItem { Id = Guid.NewGuid(), Amount = 10, ProductId = Guid.NewGuid(), OrderId = Guid.NewGuid() } } }
            };
            var expectedReadDtos = new List<OrderReadDto>
            {
                new OrderReadDto { Id = orders[0].Id, OrderItems = new List<OrderItemReadDto> { new OrderItemReadDto { Amount = 5, Product = new ProductReadDto { Id = Guid.NewGuid(), Name = "Product 1" } } } },
                new OrderReadDto { Id = orders[1].Id, OrderItems = new List<OrderItemReadDto> { new OrderItemReadDto { Amount = 10, Product = new ProductReadDto { Id = Guid.NewGuid(), Name = "Product 2" } } } }
            };

            var mockOrderRepository = new Mock<IRepository<Order>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<Order>()).Returns(mockOrderRepository.Object);

            mockOrderRepository.Setup(repo => repo.GetAllAsync(cancellationToken)).ReturnsAsync(orders);
            _mockMapper.Setup(m => m.Map<IEnumerable<OrderReadDto>>(orders)).Returns(expectedReadDtos);

            var result = await _orderService.GetAllOrdersAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetOrderByIdAsync_ExistingId_ReturnsOrderReadDto()
        {
            var orderId = Guid.NewGuid();
            var order = new Order
            {
                Id = orderId,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { Id = Guid.NewGuid(), Amount = 5, ProductId = Guid.NewGuid(), OrderId = orderId }
                }
            };
            var expectedReadDto = new OrderReadDto
            {
                Id = order.Id,
                OrderItems = new List<OrderItemReadDto>
                {
                    new OrderItemReadDto { Amount = 5, Product = new ProductReadDto { Id = Guid.NewGuid(), Name = "Test Product" } }
                }
            };

            var mockOrderRepository = new Mock<IRepository<Order>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<Order>()).Returns(mockOrderRepository.Object);

            mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId, cancellationToken)).ReturnsAsync(order);
            _mockMapper.Setup(m => m.Map<OrderReadDto>(order)).Returns(expectedReadDto);

            var result = await _orderService.GetOrderByIdAsync(orderId);

            result.Should().NotBeNull();
            result.Id.Should().Be(orderId);
        }

        [Fact]
        public async Task CreateOrderAsync_NullDto_ThrowsArgumentNullException()
        {
            Func<Task> action = async () => await _orderService.CreateOrderAsync(null);

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteOrderByIdAsync_NonExistingId_ThrowsNotFoundException()
        {
            var orderId = Guid.NewGuid();
            var mockOrderRepository = new Mock<IRepository<Order>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<Order>()).Returns(mockOrderRepository.Object);
            mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId, cancellationToken)).ReturnsAsync((Order)null);

            Func<Task> action = async () => await _orderService.DeleteOrderByIdAsync(orderId);

            await action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetOrderByIdAsync_NonExistingId_ThrowsNotFoundException()
        {
            var orderId = Guid.NewGuid();
            var mockOrderRepository = new Mock<IRepository<Order>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<Order>()).Returns(mockOrderRepository.Object);
            mockOrderRepository.Setup(repo => repo.GetByIdAsync(orderId, cancellationToken)).ReturnsAsync((Order)null);

            Func<Task> action = async () => await _orderService.GetOrderByIdAsync(orderId);

            await action.Should().ThrowAsync<NotFoundException>();
        }
    }
}
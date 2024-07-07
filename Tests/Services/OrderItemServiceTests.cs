using BusinessLogicLayer.Dtos.OrderItems;
using BusinessLogicLayer.Dtos.Products;
using System.Linq.Expressions;

namespace Tests.Services
{
    public class OrderItemServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly IOrderItemService _orderItemService;
        private readonly CancellationToken cancellationToken;

        public OrderItemServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _orderItemService = new OrderItemService(_mockUnitOfWork.Object, _mockMapper.Object);
            cancellationToken = default;
        }

        [Fact]
        public async Task CreateOrderItemAsync_ValidDto_ReturnsOrderItemReadDto()
        {
            var createDto = new OrderItemCreateDto { Amount = 5, OrderId = Guid.NewGuid(), ProductId = Guid.NewGuid(), UserName = "testuser" };
            var orderItem = new OrderItem { Id = Guid.NewGuid(), Amount = createDto.Amount, OrderId = createDto.OrderId, ProductId = createDto.ProductId };
            var product = new Product { Id = createDto.ProductId, Name = "Test Product" };
            var order = new Order { Id = createDto.OrderId, User = new User { UserName = createDto.UserName } };
            var expectedReadDto = new OrderItemReadDto { Amount = orderItem.Amount, Product = new ProductReadDto { Id = product.Id, Name = product.Name } };

            var mockOrderItemRepository = new Mock<IRepository<OrderItem>>();
            var mockProductRepository = new Mock<IRepository<Product>>();
            var mockOrderRepository = new Mock<IRepository<Order>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<OrderItem>()).Returns(mockOrderItemRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Product>()).Returns(mockProductRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Order>()).Returns(mockOrderRepository.Object);

            mockProductRepository.Setup(repo => repo.GetByIdAsync(createDto.ProductId, It.IsAny<CancellationToken>())).ReturnsAsync(product);
            mockOrderRepository.Setup(repo => repo.GetByPredicateAsync(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<Order> { order });
            _mockMapper.Setup(m => m.Map<OrderItem>(createDto)).Returns(orderItem);
            mockOrderItemRepository.Setup(repo => repo.AddAsync(orderItem, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<OrderItemReadDto>(orderItem)).Returns(expectedReadDto);

            var result = await _orderItemService.CreateOrderItemAsync(createDto);

            result.Should().NotBeNull();
            result.Amount.Should().Be(createDto.Amount);
            result.Product.Id.Should().Be(createDto.ProductId);
        }


        [Fact]
        public async Task DeleteOrderItemByIdAsync_ExistingId_ReturnsOrderItemReadDto()
        {
            var orderItemId = Guid.NewGuid();
            var orderItem = new OrderItem { Id = orderItemId, Amount = 5, Product = new Product { Id = Guid.NewGuid(), Name = "Test Product" } };
            var expectedReadDto = new OrderItemReadDto { Amount = orderItem.Amount, Product = new ProductReadDto { Id = orderItem.Product.Id, Name = orderItem.Product.Name } };

            var mockOrderItemRepository = new Mock<IRepository<OrderItem>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<OrderItem>()).Returns(mockOrderItemRepository.Object);

            mockOrderItemRepository.Setup(repo => repo.GetByIdAsync(orderItemId, cancellationToken)).ReturnsAsync(orderItem);
            mockOrderItemRepository.Setup(repo => repo.Delete(orderItem));
            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(cancellationToken)).ReturnsAsync(0);
            _mockMapper.Setup(m => m.Map<OrderItemReadDto>(orderItem)).Returns(expectedReadDto);

            var result = await _orderItemService.DeleteOrderItemByIdAsync(orderItemId);

            result.Should().NotBeNull();
            result.Amount.Should().Be(orderItem.Amount);
            result.Product.Id.Should().Be(orderItem.Product.Id);
        }

        [Fact]
        public async Task GetAllOrderItemsAsync_NoConditions_ReturnsListOfOrderItemReadDto()
        {
            var orderItems = new List<OrderItem>
            {
                new OrderItem { Id = Guid.NewGuid(), Amount = 5, Product = new Product { Id = Guid.NewGuid(), Name = "Product 1" } },
                new OrderItem { Id = Guid.NewGuid(), Amount = 10, Product = new Product { Id = Guid.NewGuid(), Name = "Product 2" } }
            };
            var expectedReadDtos = new List<OrderItemReadDto>
            {
                new OrderItemReadDto { Amount = orderItems[0].Amount, Product = new ProductReadDto { Id = orderItems[0].Product.Id, Name = orderItems[0].Product.Name } },
                new OrderItemReadDto { Amount = orderItems[1].Amount, Product = new ProductReadDto { Id = orderItems[1].Product.Id, Name = orderItems[1].Product.Name } }
            };

            var mockOrderItemRepository = new Mock<IRepository<OrderItem>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<OrderItem>()).Returns(mockOrderItemRepository.Object);

            mockOrderItemRepository.Setup(repo => repo.GetAllAsync(cancellationToken)).ReturnsAsync(orderItems);
            _mockMapper.Setup(m => m.Map<IEnumerable<OrderItemReadDto>>(orderItems)).Returns(expectedReadDtos);

            var result = await _orderItemService.GetAllOrderItemsAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainItemsAssignableTo<OrderItemReadDto>();
        }

        [Fact]
        public async Task GetOrderItemByIdAsync_ExistingId_ReturnsOrderItemReadDto()
        {
            var orderItemId = Guid.NewGuid();
            var orderItem = new OrderItem { Id = orderItemId, Amount = 5, Product = new Product { Id = Guid.NewGuid(), Name = "Test Product" } };
            var expectedReadDto = new OrderItemReadDto { Amount = orderItem.Amount, Product = new ProductReadDto { Id = orderItem.Product.Id, Name = orderItem.Product.Name } };

            var mockOrderItemRepository = new Mock<IRepository<OrderItem>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<OrderItem>()).Returns(mockOrderItemRepository.Object);

            mockOrderItemRepository.Setup(repo => repo.GetByIdAsync(orderItemId, cancellationToken)).ReturnsAsync(orderItem);
            _mockMapper.Setup(m => m.Map<OrderItemReadDto>(orderItem)).Returns(expectedReadDto);

            var result = await _orderItemService.GetOrderItemByIdAsync(orderItemId);

            result.Should().NotBeNull();
            result.Amount.Should().Be(orderItem.Amount);
            result.Product.Id.Should().Be(orderItem.Product.Id);
        }

        [Fact]
        public async Task UpdateOrderItemAsync_ValidDto_ReturnsUpdatedOrderItemReadDto()
        {
            var orderItemId = Guid.NewGuid();
            var existingProduct = new Product { Id = Guid.NewGuid(), Name = "Test Product" };
            var existingOrderItem = new OrderItem { Id = orderItemId, Amount = 5, Product = existingProduct };
            var updateDto = new OrderItemUpdateDto { Id = orderItemId, Amount = 10, UserName = "testuser" };
            var updatedOrderItem = new OrderItem { Id = orderItemId, Amount = updateDto.Amount, Product = existingProduct };
            var expectedReadDto = new OrderItemReadDto { Amount = updatedOrderItem.Amount, Product = new ProductReadDto { Id = updatedOrderItem.Product.Id, Name = updatedOrderItem.Product.Name } };

            var mockOrderItemRepository = new Mock<IRepository<OrderItem>>();
            var mockOrderRepository = new Mock<IRepository<Order>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<OrderItem>()).Returns(mockOrderItemRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Order>()).Returns(mockOrderRepository.Object);

            mockOrderItemRepository.Setup(repo => repo.GetByPredicateAsync(It.IsAny<Expression<Func<OrderItem, bool>>>(), It.IsAny<CancellationToken>()))
                                   .ReturnsAsync(new List<OrderItem> { existingOrderItem });
            _mockMapper.Setup(m => m.Map(updateDto, existingOrderItem)).Returns(updatedOrderItem);
            _mockUnitOfWork.Setup(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mockMapper.Setup(m => m.Map<OrderItemReadDto>(updatedOrderItem)).Returns(expectedReadDto);

            var result = await _orderItemService.UpdateOrderItemAsync(updateDto);

            result.Should().NotBeNull();
            result.Amount.Should().Be(updateDto.Amount);
            result.Product.Id.Should().Be(existingProduct.Id);
        }


        [Fact]
        public async Task CreateOrderItemAsync_NullDto_ThrowsArgumentNullException()
        {
            Func<Task> action = async () => await _orderItemService.CreateOrderItemAsync(null);

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteOrderItemByIdAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            var orderItemId = Guid.NewGuid();

            var mockOrderItemRepository = new Mock<IRepository<OrderItem>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<OrderItem>()).Returns(mockOrderItemRepository.Object);

            mockOrderItemRepository.Setup(repo => repo.GetByIdAsync(orderItemId, cancellationToken)).ReturnsAsync((OrderItem)null);

            Func<Task> action = async () => await _orderItemService.DeleteOrderItemByIdAsync(orderItemId);

            await action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetOrderItemByIdAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            var orderItemId = Guid.NewGuid();

            var mockOrderItemRepository = new Mock<IRepository<OrderItem>>();

            _mockUnitOfWork.Setup(uow => uow.GetRepository<OrderItem>()).Returns(mockOrderItemRepository.Object);

            mockOrderItemRepository.Setup(repo => repo.GetByIdAsync(orderItemId, cancellationToken)).ReturnsAsync((OrderItem)null);

            Func<Task> action = async () => await _orderItemService.GetOrderItemByIdAsync(orderItemId);

            await action.Should().ThrowAsync<KeyNotFoundException>();
        }

        [Fact]
        public async Task UpdateOrderItemAsync_NullDto_ThrowsArgumentNullException()
        {
            Func<Task> action = async () => await _orderItemService.UpdateOrderItemAsync(null);

            await action.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
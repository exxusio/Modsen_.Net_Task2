using BusinessLogicLayer.Dtos.OrderItems;
using BusinessLogicLayer.Dtos.Orders;

namespace Tests.Profiles
{
    public class OrderMappingProfileTests
    {
        private IMapper _mapper;

        public OrderMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new OrderMappingProfile());
                cfg.AddProfile(new OrderItemMappingProfile());
                cfg.AddProfile(new ProductMappingProfile());
                cfg.AddProfile(new CategoryMappingProfile());
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void Mapping_Configuration_IsValid()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void OrderCreateDto_To_Order_Mapping()
        {
            var orderItemCreateDto = new OrderItemCreateNewOrderDto
            {
                Amount = 5,
                ProductId = Guid.NewGuid()
            };

            var orderCreateDto = new OrderCreateDto
            {
                OrderItems = new List<OrderItemCreateNewOrderDto> { orderItemCreateDto }
            };

            var order = _mapper.Map<Order>(orderCreateDto);

            Assert.Single(order.OrderItems);
            Assert.Equal(orderItemCreateDto.Amount, order.OrderItems.First().Amount);
            Assert.Equal(orderItemCreateDto.ProductId, order.OrderItems.First().ProductId);
        }

        [Fact]
        public void Order_To_OrderReadDto_Mapping()
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Price = 10.0f
            };

            var orderItem = new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ProductId = product.Id,
                Amount = 5,
                Product = product
            };

            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                OrderItems = new List<OrderItem> { orderItem }
            };

            var orderReadDto = _mapper.Map<OrderReadDto>(order);

            Assert.Equal(order.Id, orderReadDto.Id);
            Assert.Single(orderReadDto.OrderItems);
            Assert.Equal(orderItem.Amount, orderReadDto.OrderItems.First().Amount);
            Assert.Equal(orderItem.Product.Name, orderReadDto.OrderItems.First().Product.Name);
            Assert.Equal(orderItem.Product.Price, orderReadDto.OrderItems.First().Product.Price);
        }
    }
}
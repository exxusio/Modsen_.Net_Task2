using BusinessLogicLayer.Dtos.OrderItems;

namespace Tests.Profiles
{
    public class OrderItemMappingProfileTests
    {
        private readonly IMapper _mapper;

        public OrderItemMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
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
        public void OrderItemCreateDto_To_OrderItem_Mapping()
        {
            var orderItemCreateDto = new OrderItemCreateDto
            {
                Amount = 5,
                OrderId = Guid.NewGuid(),
                ProductId = Guid.NewGuid()
            };

            var orderItem = _mapper.Map<OrderItem>(orderItemCreateDto);

            orderItem.Should().BeEquivalentTo(orderItemCreateDto, options => options
                .ExcludingMissingMembers());
        }

        [Fact]
        public void OrderItem_To_OrderItemReadDto_Mapping()
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

            var orderItemReadDto = _mapper.Map<OrderItemReadDto>(orderItem);

            orderItemReadDto.Should().BeEquivalentTo(orderItem, options => options
                .ExcludingMissingMembers()
                .Excluding(dto => dto.Product));

            orderItemReadDto.Product.Should().BeEquivalentTo(product, options => options
                .ExcludingMissingMembers());
        }
    }
}
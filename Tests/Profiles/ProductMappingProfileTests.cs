using BusinessLogicLayer.Dtos.Products;


namespace Tests.Profiles
{
    public class ProductMappingProfileTests
    {
        private IMapper _mapper;

        public ProductMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
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
        public void ProductCreateDto_To_Product_Mapping()
        {
            var productCreateDto = new ProductCreateDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 100.0f,
                CategoryId = Guid.NewGuid()
            };

            var product = _mapper.Map<Product>(productCreateDto);

            Assert.Equal(productCreateDto.Name, product.Name);
            Assert.Equal(productCreateDto.Description, product.Description);
            Assert.Equal(productCreateDto.Price, product.Price);
            Assert.Equal(productCreateDto.CategoryId, product.CategoryId);
        }

        [Fact]
        public void ProductUpdateDto_To_Product_Mapping()
        {
            var productUpdateDto = new ProductUpdateDto
            {
                Id = Guid.NewGuid(),
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 150.0f,
                CategoryId = Guid.NewGuid()
            };

            var product = _mapper.Map<Product>(productUpdateDto);

            Assert.Equal(productUpdateDto.Id, product.Id);
            Assert.Equal(productUpdateDto.Name, product.Name);
            Assert.Equal(productUpdateDto.Description, product.Description);
            Assert.Equal(productUpdateDto.Price, product.Price);
            Assert.Equal(productUpdateDto.CategoryId, product.CategoryId);
        }

        [Fact]
        public void Product_To_ProductReadDto_Mapping()
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Test Product",
                Description = "Test Description",
                Price = 100.0f,
                CategoryId = Guid.NewGuid()
            };

            var productReadDto = _mapper.Map<ProductReadDto>(product);

            Assert.Equal(product.Id, productReadDto.Id);
            Assert.Equal(product.Name, productReadDto.Name);
            Assert.Equal(product.Price, productReadDto.Price);
        }
    }
}
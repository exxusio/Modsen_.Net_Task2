using BusinessLogicLayer.Dtos.Categories;

namespace Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ICategoryService _categoryService;
        private readonly CancellationToken cancellationToken;

        public CategoryServiceTests()
        {
            _mockRepository = new Mock<ICategoryRepository>();
            _mockMapper = new Mock<IMapper>();
            _categoryService = new CategoryService(_mockRepository.Object, _mockMapper.Object);
            cancellationToken = default;
        }

        [Fact]
        public async Task CreateCategoryAsync_ValidDto_ReturnsCategoryReadDto()
        {
            var createDto = new CategoryCreateDto { Name = "Test Category" };
            var category = new Category { Id = Guid.NewGuid(), Name = createDto.Name };
            var expectedReadDto = new CategoryReadDto { Id = category.Id, Name = category.Name };


            _mockMapper.Setup(m => m.Map<Category>(createDto)).Returns(category);
            _mockMapper.Setup(m => m.Map<CategoryReadDto>(category)).Returns(expectedReadDto);
            _mockRepository.Setup(repo => repo.AddAsync(category, cancellationToken)).Returns(Task.CompletedTask);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            var result = await _categoryService.CreateCategoryAsync(createDto);

            result.Should().NotBeNull();
            result.Name.Should().Be(createDto.Name);
            result.Id.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task DeleteCategoryByIdAsync_ExistingId_ReturnsCategoryReadDto()
        {
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Test Category" };
            var expectedReadDto = new CategoryDetailedReadDto { Id = category.Id, Name = category.Name };

            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId, cancellationToken)).ReturnsAsync(category);
            _mockMapper.Setup(m => m.Map<CategoryDetailedReadDto>(category)).Returns(expectedReadDto);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);

            var result = await _categoryService.DeleteCategoryByIdAsync(categoryId);

            result.Should().NotBeNull();
            result.Id.Should().Be(categoryId);
            result.Name.Should().Be("Test Category");
        }

        [Fact]
        public async Task GetAllCategoriesAsync_NoConditions_ReturnsListOfCategoryReadDto()
        {
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Category 1" },
                new Category { Id = Guid.NewGuid(), Name = "Category 2" }
            };
            var expectedReadDtos = new List<CategoryReadDto>
            {
                new CategoryReadDto { Id = categories[0].Id, Name = categories[0].Name },
                new CategoryReadDto { Id = categories[1].Id, Name = categories[1].Name }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync(cancellationToken)).ReturnsAsync(categories);
            _mockMapper.Setup(m => m.Map<IEnumerable<CategoryReadDto>>(categories)).Returns(expectedReadDtos);

            var result = await _categoryService.GetAllCategoriesAsync();

            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().ContainItemsAssignableTo<CategoryReadDto>();
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ExistingId_ReturnsCategoryReadDto()
        {
            var categoryId = Guid.NewGuid();
            var category = new Category { Id = categoryId, Name = "Test Category" };
            var expectedReadDto = new CategoryDetailedReadDto { Id = category.Id, Name = category.Name };

            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId, cancellationToken)).ReturnsAsync(category);
            _mockMapper.Setup(m => m.Map<CategoryDetailedReadDto>(category)).Returns(expectedReadDto);

            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            result.Should().NotBeNull();
            result.Id.Should().Be(categoryId);
            result.Name.Should().Be("Test Category");
        }

        [Fact]
        public async Task UpdateCategoryAsync_ValidDto_ReturnsUpdatedCategoryReadDto()
        {
            var categoryId = Guid.NewGuid();
            var existingCategory = new Category { Id = categoryId, Name = "Existing Category" };
            var updatedDto = new CategoryUpdateDto { Id = categoryId, Name = "Updated Category" };
            var updatedCategory = new Category { Id = categoryId, Name = updatedDto.Name };
            var expectedReadDto = new CategoryReadDto { Id = updatedCategory.Id, Name = updatedCategory.Name };

            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId, cancellationToken)).ReturnsAsync(existingCategory);
            _mockMapper.Setup(m => m.Map(updatedDto, existingCategory)).Returns(updatedCategory);
            _mockRepository.Setup(repo => repo.SaveChangesAsync(cancellationToken)).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<CategoryReadDto>(updatedCategory)).Returns(expectedReadDto);

            var result = await _categoryService.UpdateCategoryAsync(updatedDto);

            result.Should().NotBeNull();
            result.Id.Should().Be(categoryId);
            result.Name.Should().Be("Updated Category");
        }

        [Fact]
        public async Task CreateCategoryAsync_NullDto_ThrowsArgumentNullException()
        {
            Func<Task> action = async () => await _categoryService.CreateCategoryAsync(null);

            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeleteCategoryByIdAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            var categoryId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId, cancellationToken)).ReturnsAsync((Category)null);

            Func<Task> action = async () => await _categoryService.DeleteCategoryByIdAsync(categoryId);

            await action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task GetCategoryByIdAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            var categoryId = Guid.NewGuid();
            _mockRepository.Setup(repo => repo.GetByIdAsync(categoryId, cancellationToken)).ReturnsAsync((Category)null);

            Func<Task> action = async () => await _categoryService.GetCategoryByIdAsync(categoryId);

            await action.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateCategoryAsync_NullDto_ThrowsArgumentNullException()
        {
            Func<Task> action = async () => await _categoryService.UpdateCategoryAsync(null);

            await action.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
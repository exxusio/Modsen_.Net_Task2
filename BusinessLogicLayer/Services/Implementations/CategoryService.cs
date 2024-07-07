using AutoMapper;
using BusinessLogicLayer.Dtos.Categories;
using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services.Interfaces;
using DataAccessLayer.Data.Interfaces;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryReadDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default)
        {
            if (categoryCreateDto == null)
            {
                throw new ArgumentNullException(nameof(categoryCreateDto));
            }

            var category = _mapper.Map<Category>(categoryCreateDto);

            await _categoryRepository.AddAsync(category, cancellationToken);
            await _categoryRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryReadDto>(category);
        }

        public async Task<CategoryDetailedReadDto> DeleteCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);

            if (category == null)
            {
                throw new NotFoundException($"Category not found with id: {id}");
            }

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CategoryDetailedReadDto>(category);
        }

        public async Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _categoryRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<CategoryReadDto>>(categories);
        }

        public async Task<CategoryDetailedReadDto> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
            if (category == null)
            {
                throw new NotFoundException($"Category not found with id: {id}");
            }

            return _mapper.Map<CategoryDetailedReadDto>(category);
        }

        public async Task<CategoryReadDto> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default)
        {
            if (categoryUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(categoryUpdateDto));
            }

            var existingCategory = await _categoryRepository.GetByIdAsync(categoryUpdateDto.Id, cancellationToken);
            if (existingCategory == null)
            {
                throw new NotFoundException($"Category not found with id: {categoryUpdateDto.Id}");
            }

            var newCategory = _mapper.Map(categoryUpdateDto, existingCategory);
            await _categoryRepository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<CategoryReadDto>(newCategory);
        }
    }
}
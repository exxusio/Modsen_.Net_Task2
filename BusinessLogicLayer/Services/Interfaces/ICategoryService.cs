using BusinessLogicLayer.Dtos.Categories;

namespace BusinessLogicLayer.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryReadDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
        Task<CategoryDetailedReadDto> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<CategoryReadDto> CreateCategoryAsync(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default);
        Task<CategoryReadDto> UpdateCategoryAsync(CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default);
        Task<CategoryDetailedReadDto> DeleteCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

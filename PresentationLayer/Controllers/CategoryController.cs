using BusinessLogicLayer;
using BusinessLogicLayer.Dtos.Categories;
using BusinessLogicLayer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories(CancellationToken cancellationToken = default)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(cancellationToken);
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id, CancellationToken cancellationToken = default)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id, cancellationToken);
            return Ok(category);
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreateDto categoryCreateDto, CancellationToken cancellationToken = default)
        {
            var category = await _categoryService.CreateCategoryAsync(categoryCreateDto, cancellationToken);
            return Ok(category);
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken = default)
        {
            categoryUpdateDto.Id = id;
            var category = await _categoryService.UpdateCategoryAsync(categoryUpdateDto, cancellationToken);
            return Ok(category);
        }

        [Authorize(Roles = $"{RoleConstants.Admin}")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken = default)
        {
            var deletedCategory = await _categoryService.DeleteCategoryByIdAsync(id, cancellationToken);
            return Ok(deletedCategory);
        }
    }
}

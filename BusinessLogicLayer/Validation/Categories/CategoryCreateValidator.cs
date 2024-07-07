using BusinessLogicLayer.Dtos.Categories;
using FluentValidation;

namespace BusinessLogicLayer.Validation.Categories;

public class CategoryCreateValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateValidator()
    {
        RuleFor(category => category.Name)
            .CategoryOrProductName();
    }
}
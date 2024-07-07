using BusinessLogicLayer.Dtos.Categories;
using FluentValidation;

namespace BusinessLogicLayer.Validation.Categories;

public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateValidator()
    {
        RuleFor(category => category.Name)
            .CategoryOrProductName();
    }
}

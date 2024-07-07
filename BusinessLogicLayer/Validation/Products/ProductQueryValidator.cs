using BusinessLogicLayer.Dtos.Products;
using FluentValidation;

namespace BusinessLogicLayer.Validation.Products;

public class ProductQueryValidator : AbstractValidator<ProductQuery>
{
    public ProductQueryValidator()
    {
        When(query => query.Name is not null, () =>
        {
            RuleFor(query => query.Name)
            .CategoryOrProductName();
        });

        When(query => query.Description is not null, () =>
        {
            RuleFor(query => query.Description)
            .ProductDescription();
        });

        RuleFor(query => query.MinPrice)
            .Price()
            .Must((query, minPrice) => minPrice < query.MaxPrice).WithMessage("Minimum price must be less than maximum price");

        RuleFor(query => query.MaxPrice)
            .Price()
            .Must((query, maxPrice) => maxPrice > query.MinPrice).WithMessage("Maximum price must be higher than minimum price");
    }
}

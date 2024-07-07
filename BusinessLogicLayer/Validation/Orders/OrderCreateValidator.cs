using BusinessLogicLayer.Dtos.Orders;
using FluentValidation;

namespace BusinessLogicLayer.Validation.Orders;

public class OrderCreateValidator : AbstractValidator<OrderCreateDto>
{
    public OrderCreateValidator()
    {
        RuleFor(order => order.OrderItems)
            .NotNull().WithMessage("Order items should not be null")
            .NotEmpty().WithMessage("Order items should not be empty");
        //.ForEach(item => item.SetValidator(new OrderItemCreateValidator()));
    }
}

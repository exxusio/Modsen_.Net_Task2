using BusinessLogicLayer.Dtos.OrderItems;
using FluentValidation;

namespace BusinessLogicLayer.Validation.OrderItems;

public class OrderItemUpdateValidator : AbstractValidator<OrderItemUpdateDto>
{
    public OrderItemUpdateValidator()
    {
        RuleFor(orderItem => orderItem.UserName)
            .UserName();

        RuleFor(orderItem => orderItem.Amount)
            .ItemAmount();
    }
}

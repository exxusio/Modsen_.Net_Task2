using BusinessLogicLayer.Dtos.OrderItems;
using FluentValidation;


namespace BusinessLogicLayer.Validation.OrderItems;

public class OrderItemCreateNewOrderValidator : AbstractValidator<OrderItemCreateNewOrderDto>
{
    public OrderItemCreateNewOrderValidator()
    {
        RuleFor(order => order.Amount)
            .ItemAmount();
    }
}

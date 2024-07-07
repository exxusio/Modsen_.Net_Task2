using BusinessLogicLayer.Dtos.Users;
using FluentValidation;

namespace BusinessLogicLayer.Validation.Users;

public class UserLoginValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginValidator()
    {
        RuleFor(user => user.UserName)
            .UserName();

        RuleFor(user => user.Password)
            .Password();
    }
}

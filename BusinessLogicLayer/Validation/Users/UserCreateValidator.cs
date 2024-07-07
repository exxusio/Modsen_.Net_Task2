using BusinessLogicLayer.Dtos.Users;
using FluentValidation;

namespace BusinessLogicLayer.Validation.Users;

public class UserCreateValidator : AbstractValidator<UserCreateDto>
{
    public UserCreateValidator()
    {
        RuleFor(user => user.UserName)
            .UserName();

        RuleFor(user => user.Password)
            .PasswordCreating();
    }
}
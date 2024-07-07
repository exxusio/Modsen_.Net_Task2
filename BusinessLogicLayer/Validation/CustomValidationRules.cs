using FluentValidation;

namespace BusinessLogicLayer.Validation;

public static class CustomValidationRules
{
    public static IRuleBuilder<T, string> CategoryOrProductName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull().WithMessage("{PropertyName} should not be null")
            .NotEmpty().WithMessage("{PropertyName} should not be empty")
            .Length(3, 50).WithMessage("{PropertyName} should have length between 3 and 50");
    }
    public static IRuleBuilder<T, string> ProductDescription<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull().WithMessage("Product description should not be null")
            .NotEmpty().WithMessage("Product description should not be empty")
            .MaximumLength(500).WithMessage("Product description should not exceed 500 characters");
    }
    public static IRuleBuilder<T, float> Price<T>(this IRuleBuilder<T, float> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage("Price should be greater than 0");
    }
    public static IRuleBuilder<T, Guid> IsGuid<T>(this IRuleBuilder<T, Guid> ruleBuilder)
    {
        return ruleBuilder
            .NotEqual(Guid.Empty).WithMessage("{PropertyName} cannot by empty guid!");
    }
    public static IRuleBuilder<T, string?> UserName<T>(this IRuleBuilder<T, string?> ruleBuilder, bool allowNullCheck = true)
    {
        if (allowNullCheck)
        {
            ruleBuilder = ruleBuilder
                .NotNull().WithMessage("Username should not be null")
                .NotEmpty().WithMessage("Username should not be empty");
        }

        return ruleBuilder
            .Must(username => username == null || (username.Length >= 3 && username.Length <= 20))
            .WithMessage("Username should have length between 3 and 20");
    }
    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotNull().WithMessage("Password should not be null")
            .NotEmpty().WithMessage("Password should not be empty");
    }
    public static IRuleBuilder<T, string> PasswordCreating<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Password()
            .Length(8, 20).WithMessage("Password should have length between 8 and 20")
            .Matches("[A-Z]").WithMessage("Password should contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password should contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password should contain at least one digit")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password should contain at least one special character.");
    }
    public static IRuleBuilder<T, int> ItemAmount<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage("Amount should be greater than 0");
    }
}

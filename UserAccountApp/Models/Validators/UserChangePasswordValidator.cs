using FluentValidation;

namespace UserAccountApp.Models.Validators
{
    /// <summary>
    /// Устанавливает правила валидации для формы смены пароля
    /// </summary>
    public class UserChangePasswordValidator : AbstractValidator<UserChangePassword>
    {
        public UserChangePasswordValidator()
        {
            RuleFor(c => c.Email).EmailRule();

            RuleFor(c => c.OldPassword).PasswordRule();

            RuleFor(c => c.NewPassword).PasswordRule();
        }
    }
}

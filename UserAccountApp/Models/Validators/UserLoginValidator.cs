using FluentValidation;

namespace UserAccountApp.Models.Validators
{
    /// <summary>
    /// Устанавливает правила валидации для формы авторизации
    /// </summary>
    public class UserLoginValidator : AbstractValidator<UserLogin>
    {
        public UserLoginValidator()
        {

            RuleFor(c => c.Email).EmailRule();

            RuleFor(c => c.Password).NotEmpty().WithMessage(ValidationRules.notEmpty);

        }
    }
}

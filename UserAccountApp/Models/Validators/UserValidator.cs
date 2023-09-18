using FluentValidation;
using UserAccountApp.Services;

namespace UserAccountApp.Models.Validators
{
    /// <summary>
    /// Устанавливает правила валидации для полей модели User
    /// </summary>
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(c => c.FirstName).FirstNameRule();

            RuleFor(c => c.SecondName).SecondNameRule();

            RuleFor(c => c.FatherName).FatherNameRule();

            RuleFor(c => c.Email).EmailRule();

            RuleFor(c => c.Password).PasswordRule();

            RuleFor(c => c.Address).AddressRule();
        }
    }
}

using FluentValidation;

namespace UserAccountApp.Models.Validators
{
    /// <summary>
    /// Устанавливает правила валидации для формы смены пользовательских данных
    /// </summary>
    public class UserChangeDetailsValidator : AbstractValidator<UserChangeDetails>
    {
        public UserChangeDetailsValidator()
        {
            RuleFor(c => c.Email).EmailRule();

            RuleFor(c => c.FirstName).FirstNameRule();

            RuleFor(c => c.SecondName).SecondNameRule();

            RuleFor(c => c.FatherName).FatherNameRule();

            RuleFor(c => c.Address).AddressRule();
        }
    }
}

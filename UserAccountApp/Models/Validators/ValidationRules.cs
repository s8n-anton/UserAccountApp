using FluentValidation;

namespace UserAccountApp.Models.Validators
{
    /// <summary>
    /// Класс, реализующий комплексные правила валидации 
    /// </summary>
    //Правила валидации для FluentValidation
    public static class ValidationRules
    {
        //Перечень дефолтных сообщений с плейсхолдерами
        public const string tooLongStr = "The length of {PropertyName} must be {MaxLength} characters or fewer.";
        public const string tooShortStr = "The length of {PropertyName} must be at least {MinLength} characters.";
        public const string containsLettersOnly = "{PropertyName} must contain only letters.";
        public const string notEmpty = "{PropertyName} cannot be empty.";
        public const string atLeastOneUpCase = "{PropertyName} must contain at least one uppercase letter.";
        public const string atLeastOneLowCase = "{PropertyName} must contain at least one uppercase letter.";
        public const string atLeastOneNum = "{PropertyName} must contain at least one number.";
        public const string atLeastOneSpecChar = "{PropertyName} must contain at least one (!?*.).";
        public const string emailIncorrect = "Email is incorrect.";
        public const int maxNamesLength = 50;
        public const int minNamesLength = 2;
        public const int minPasswordLength = 8;
        public const int maxPasswordLength = 16;
        public const int maxAddressLength = 100;

        //Методы расширения, которые реализуют общие правила валидации для разных форм

        /// <summary>
        /// Правило валидации почты 
        /// </summary>
        public static IRuleBuilderOptions<T, string> EmailRule<T>(this IRuleBuilder< T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage(notEmpty)
                .EmailAddress().WithMessage(emailIncorrect);
        }

        /// <summary>
        /// Правило валидации имени 
        /// </summary>
        public static IRuleBuilderOptions<T, string> FirstNameRule<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage(notEmpty)
                .Must(c => c.All(char.IsLetter)).WithMessage(containsLettersOnly)
                .MaximumLength(maxNamesLength).WithMessage(tooLongStr);
        }

        /// <summary>
        /// Правило валидации фамилии 
        /// </summary>
        public static IRuleBuilderOptions<T, string> SecondNameRule<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage(notEmpty)
                .Must(c => c.All(char.IsLetter)).WithMessage(containsLettersOnly)
                .MinimumLength(minNamesLength).WithMessage(tooShortStr)
                .MaximumLength(maxNamesLength).WithMessage(tooLongStr);
        }

        /// <summary>
        /// Правило валидации отчества 
        /// </summary>
        public static IRuleBuilderOptions<T, string> FatherNameRule<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .Must(c => c.All(char.IsLetter)).WithMessage(containsLettersOnly)
                .MaximumLength(maxNamesLength).WithMessage(tooLongStr);
        }

        /// <summary>
        /// Правило валидации пароля 
        /// </summary>
        public static IRuleBuilderOptions<T, string> PasswordRule<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage(notEmpty)
                .MinimumLength(minPasswordLength).WithMessage(tooShortStr)
                .MaximumLength(maxPasswordLength).WithMessage(tooLongStr)
                .Matches(@"[A-Z]+").WithMessage(atLeastOneUpCase)
                .Matches(@"[a-z]+").WithMessage(atLeastOneLowCase)
                .Matches(@"[0-9]+").WithMessage(atLeastOneNum)
                .Matches(@"[\!\?\*\.]+").WithMessage(atLeastOneSpecChar);
        }

        /// <summary>
        /// Правило валидации адреса 
        /// </summary>
        public static IRuleBuilderOptions<T, string> AddressRule<T>(this IRuleBuilder<T, string> rule)
        {
            return rule
                .NotEmpty().WithMessage(notEmpty)
                .MaximumLength(maxAddressLength).WithMessage(tooLongStr);
        }


    }
}

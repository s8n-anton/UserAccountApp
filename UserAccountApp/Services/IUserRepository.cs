using FluentValidation.Results;
using UserAccountApp.Models;

namespace UserAccountApp.Services
{
    //Интерфейс для реализации взаимодействия с моделью пользователя
    //Подразумевается, что это будет осуществляться через БД, но для задания сделана другая реализация (UserData)
    public interface IUserRepository
    {
        /// <summary>
        /// Возвращает всех пользователей
        /// </summary>
        public IEnumerable<User> GetAll();

        /// <summary>
        /// Получает пользователя по адресу электронной почты
        /// </summary>
        /// <param name="email">Адрес электронной почты</param>
        public User? GetByEmail(string email);

        /// <summary>
        /// Регистрирует пользователя в системе
        /// </summary>
        /// <param name="user">Модель пользователя</param>
        public User Register(User user);

        /// <summary>
        /// Меняет пароль пользователя
        /// </summary>
        /// <param name="userChangePassword">Форма смены пароля</param>
        public void ChangePassword(UserChangePassword userChangePassword);

        /// <summary>
        /// Меняет пользовательские данные
        /// </summary>
        /// <param name="userChangeDetails">Форма смены пользовательских данных</param>
        public void ChangeDetails(UserChangeDetails userChangeDetails);

        /// <summary>
        /// Авторизует пользователя
        /// </summary>
        /// <param name="userLogin">Форма авторизации</param>
        public User Login(UserLogin userLogin);

        /// <summary>
        /// Осуществляет валидацию данных в модели пользователя до регистрации
        /// </summary>
        /// <param name="user">Модель пользователя</param>
        
        public ValidationResult ValidateUser(User user);
        /// <summary>
        /// Осуществляет валидацию данных в модели пользователя
        /// </summary>
        /// <param name="user">Модель пользователя</param>

        /// <summary>
        /// Осуществляет валидацию данных с формы смены пароля
        /// </summary>
        /// <param name="userChangePassword">Форма смены пароля</param>
        public ValidationResult ValidateUserChangePassword(UserChangePassword userChangePassword);

        /// <summary>
        /// Осуществляет валидацию данных с формы смены пользовательских данных
        /// </summary>
        /// <param name="userChangeDetails">Форма смены пароля</param>
        public ValidationResult ValidateUserChangeDetails(UserChangeDetails userChangeDetails);

        /// <summary>
        /// Осуществляет валидацию данных с формы авторизации
        /// </summary>
        /// <param name="userLogin">Форма авторизации</param>
        public ValidationResult ValidateUserLogin(UserLogin userLogin);
    }
}

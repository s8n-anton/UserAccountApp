namespace UserAccountApp.Models
{
    /// <summary>
    /// Форма авторизации
    /// </summary>
    public class UserLogin
    {
        /// <summary>
        /// Электронная почта пользователя
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }

        public UserLogin(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}

namespace UserAccountApp.Models
{
    /// <summary>
    /// Форма смены пароля
    /// </summary>
    public class UserChangePassword
    {
        /// <summary>
        /// Электронная почта пользователя 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Старый пароль 
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// Новый пароль
        /// </summary>
        public string NewPassword { get; set; }

        public UserChangePassword(string email, string oldPassword, string newPassword)
        {
            Email = email;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}

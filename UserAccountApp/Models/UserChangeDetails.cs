namespace UserAccountApp.Models
{
    /// <summary>
    /// Форма смены пользовательских данных
    /// </summary>
    public class UserChangeDetails
    {
        /// <summary>
        /// Электронная почта пользователя 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Имя пользователя 
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя 
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// Отчество пользователя 
        /// </summary>
        public string FatherName { get; set; }

        /// <summary>
        /// Адрес пользователя 
        /// </summary>
        public string Address { get; set; }

        public UserChangeDetails(string email, string firstName, string secondName, 
            string fatherName, string address) 
        { 
            Email = email;
            FirstName = firstName;
            SecondName = secondName;
            FatherName = fatherName;
            Address = address;
        }
    }
}

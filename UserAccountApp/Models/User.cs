using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UserAccountApp.Utilities;

namespace UserAccountApp.Models
{
    /// <summary>
    /// Модель пользователя
    /// </summary>
    public class User
    {
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
        /// Электронная почта пользователя 
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль пользователя 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Адрес пользователя 
        /// </summary>
        public string Address { get; set; }

        public User(string firstName, string secondName, string fatherName, string email, string password, string address) { 
            FirstName = firstName;
            SecondName = secondName;
            FatherName = fatherName;
            Email = email;
            Password = password;
            Address = address;
        }

        /// <summary>
        /// Заменяет пароль на хэш пароля 
        /// </summary>
        public User Register()
        {
            Password = HashPassword(Password);
            return this;
        }

        /// <summary>
        /// Формирует хэш пароля 
        /// </summary>
        /// <param name="password">Строка с паролем</param>
        public string HashPassword(string password)
        {
            //массив под "соль" (генерируется на основе пароля)
            byte[] salt;
            //массив под хэш пароля
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("Empty Password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 16, 1000))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(32);
            }
            byte[] dst = new byte[49];
            //итоговый хэш это соль + хэш пароля
            Buffer.BlockCopy(salt, 0, dst, 1, 16);
            Buffer.BlockCopy(buffer2, 0, dst, 17, 32);
            return Convert.ToBase64String(dst);
        }

        /// <summary>
        /// Сверяет хэшированный пароль и не хэшированный пароль
        /// </summary>
        /// <param name="hashedPassword">Хэш пароля</param>
        /// <param name="password">Пароль</param>
        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 49) || (src[0] != 0))
            {
                return false;
            }
            byte[] salt = new byte[16];
            //забираем байты со 2 по 17 в качестве соли
            Buffer.BlockCopy(src, 1, salt, 0, 16);
            //забираем остальные байты (хэш пароля)
            byte[] buffer3 = new byte[32];
            Buffer.BlockCopy(src, 17, buffer3, 0, 32);
            //получаем хэш пароля на основе той же соли
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, salt, 1000))
            {
                buffer4 = bytes.GetBytes(32);
            }
            //совпал ли исходный хэш пароля с хэшированным на основе той же соли введенным паролем
            return BytesOperations.ByteArraysEqual(buffer3, buffer4);
        }

        /// <summary>
        /// Меняет текущий пароль на хэш нового пароля
        /// </summary>
        /// <param name="oldPassword">Хэш пароля</param>
        public void ChangePassword(string newPassword)
        {
            Password = HashPassword(newPassword); 
        }

        /// <summary>
        /// Меняет пользовательские данные
        /// </summary>
        /// <param name="firstName">Новое имя</param>
        /// <param name="secondName">Новая фамилия</param>
        /// <param name="fatherName">Новое отчество</param>
        /// <param name="address">Новый адрес</param>
        public void ChangeDetails(string firstName, string secondName, string fatherName, string address)
        {
            FirstName = firstName;
            SecondName = secondName;
            FatherName = fatherName;
            Address = address;
        }
    }
}
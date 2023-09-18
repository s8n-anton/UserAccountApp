using FluentValidation;
using FluentValidation.Results;
using UserAccountApp.Models;
using UserAccountApp.Models.Validators;

namespace UserAccountApp.Services
{
    /// <summary>
    /// Реализует функционал по работе с хранилищем моделей пользователей
    /// </summary>
    public class UserData : IUserRepository
    {
        //Стандартные сообщения для исключений
        public const string userAlreadyExists = "User with given Email already exists.";
        public const string userDoesntExist = "User with given Email does not exist.";
        public const string oldPassDoesntMatch = "Old password does not match";
        public const string wrongPass = "Password does not match for account with given Email.";

        private readonly IValidator<User> _userValidator = new UserValidator();
        private readonly IValidator<UserChangePassword> _userChangePasswordValidator = new UserChangePasswordValidator();
        private readonly IValidator<UserChangeDetails> _userChangeDetailsValidator = new UserChangeDetailsValidator();
        private readonly IValidator<UserLogin> _userLoginValidator = new UserLoginValidator();
        private readonly IUserDataGenerator _userSource;

        //хранение моделей (БД в данной реализации не подразумевается)
        private readonly List<User> _users;

        // В конструкторе реализована инъекция зависимости
        // Чтобы она работала, в Program.cs зарегистрирован сервис
        public UserData(IUserDataGenerator dataGen)
        {
            _userSource = dataGen;
            _users = _userSource.GenerateUsers();

        }

        public IEnumerable<User> GetAll()
        {
            if (_users == null)
                return new List<User>();
            else
                return _users;
        }

        public User? GetByEmail(string email)
        {
            return _users.FirstOrDefault(x => x.Email == email);
        }

        public User Register(User user)
        {
            //Проверяем что пользователя с такой почтой еще нет
            if (_users.FirstOrDefault(x => x.Email == user.Email) == null)
            {
                User newUser = user.Register();
                _users.Add(newUser);
                return newUser;
            }
            else
                throw new Exception(userAlreadyExists);
        }

        public void ChangePassword(UserChangePassword userChangePassword)
        {
            User? user = GetByEmail(userChangePassword.Email);
            if (user != null )
            {
                if (user.VerifyHashedPassword(user.Password, userChangePassword.OldPassword))
                {
                    user.ChangePassword(userChangePassword.NewPassword);
                }
                else
                    throw new Exception(oldPassDoesntMatch);
            }
            else 
                throw new Exception(userDoesntExist);
        }

        public void ChangeDetails(UserChangeDetails userChangeDetails)
        {
            User? user = GetByEmail(userChangeDetails.Email);
            if (user != null)
            {
                user.ChangeDetails(userChangeDetails.FirstName, userChangeDetails.SecondName, userChangeDetails.FatherName, userChangeDetails.Address);
            }
            else
                throw new Exception(userDoesntExist);
        }

        public User Login(UserLogin userLogin)
        {
            User? user = GetByEmail(userLogin.Email);
            if (user != null)
                if (user.VerifyHashedPassword(user.Password, userLogin.Password))
                {
                    return user;
                }
                else 
                    throw new Exception(wrongPass);
            else
                throw new Exception(userDoesntExist);
        }

        public ValidationResult ValidateUser(User user)
        {
            return _userValidator.Validate(user);
        }

        public ValidationResult ValidateUserChangePassword(UserChangePassword userChangePassword)
        {
            return _userChangePasswordValidator.Validate(userChangePassword);
        }

        public ValidationResult ValidateUserChangeDetails(UserChangeDetails userChangeDetails)
        {
            return _userChangeDetailsValidator.Validate(userChangeDetails);
        }

        public ValidationResult ValidateUserLogin(UserLogin userLogin)
        {
            return _userLoginValidator.Validate(userLogin);
        }
    }
}

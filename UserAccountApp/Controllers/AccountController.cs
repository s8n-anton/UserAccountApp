using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using UserAccountApp.Models;
using UserAccountApp.Models.Validators;
using UserAccountApp.Services;

namespace UserAccountApp.Controllers
{
    /// <summary>
    /// Класс контроллера, реализующего методы по управлению учетными записями
    /// </summary>
    /// <param name="repeat">Сколько раз передать привет</param>
    /// <returns>Сама строка с приветами</returns>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="userRepo">Реализация интерфейса IUserRepository</param>
        // В конструкторе реализована инъекция зависимости.
        // Чтобы она работала, в Program.cs зарегистрирован сервис.
        public AccountController(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        /// <summary>
        /// Возвращает список пользователей
        /// </summary>
        [HttpGet]
        [Route("/api/[controller]/get-all-users")]
        public IActionResult Get()
        {
            return new JsonResult(_userRepository.GetAll());
        }

        /// <summary>
        /// Регистрирует пользователя
        /// </summary>
        ///  /// <param name="user">Модель пользователя</param>
        [HttpPost]
        [Route("/api/[controller]/register")]
        public IActionResult RegisterUser(User user)
        {
            ValidationResult validationResult = _userRepository.ValidateUser(user);
            if (validationResult.IsValid)
            {
                User? newUser;
                try
                {
                    newUser = _userRepository.Register(user);
                    return Ok(newUser);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
                return BadRequest(validationResult.ToString(" "));
        }

        /// <summary>
        /// Изменяет пароль пользователя
        /// </summary>
        /// <param name="userChangePassword">Форма для смены пароля</param>
        [HttpPost]
        [Route("/api/[controller]/change-password")]
        public IActionResult ChangePassword(UserChangePassword userChangePassword)
        {
            ValidationResult validationResult = _userRepository.ValidateUserChangePassword(userChangePassword);
            if (validationResult.IsValid)
            {
                try
                {
                    _userRepository.ChangePassword(userChangePassword);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return Ok("Password changed successfully.");
            }
            else
                return BadRequest(validationResult.ToString(" "));
        }

        /// <summary>
        /// Изменяет данные пользователя
        /// </summary>
        /// <param name="userChangeDetails">Форма для смены пользовательских данных</param>
        [HttpPost]
        [Route("/api/[controller]/change-details")]
        public IActionResult ChangeDetails(UserChangeDetails userChangeDetails)
        {
            ValidationResult validationResult = _userRepository.ValidateUserChangeDetails(userChangeDetails);
            if (validationResult.IsValid)
            {
                try
                {
                    _userRepository.ChangeDetails(userChangeDetails);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                return Ok("User details changed successfully.");
            }
            else
                return BadRequest(validationResult.ToString(" "));
        }

        /// <summary>
        /// Авторизация
        /// </summary>
        /// <param name="userLogin">Форма входа в систему</param>
        [HttpPost]
        [Route("/api/[controller]/login")]
        public IActionResult Login(UserLogin userLogin)
        {
            ValidationResult validationResult = _userRepository.ValidateUserLogin(userLogin);
            if (validationResult.IsValid)
            {
                User? user;
                try
                {
                    user = _userRepository.Login(userLogin);
                    return Ok(user);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
                return BadRequest(validationResult.ToString(" "));
        }
    }
}

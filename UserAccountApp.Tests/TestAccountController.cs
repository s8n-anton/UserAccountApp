using Microsoft.AspNetCore.Mvc;
using Moq;
using UserAccountApp.Controllers;
using UserAccountApp.Models;
using UserAccountApp.Models.Validators;
using UserAccountApp.Services;
using FormatWith;
using FluentValidation.Results;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace UserAccountApp.Tests
{
    public class TestAccountController : TestBase
    {

        #region RegisterUser Method Tests

        //-----------------------------
        //Все исходные данные корректны
        //-----------------------------
        [Fact]
        public void RegisterUser_ShouldReturnUserObject()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();
            mock.Setup(a => a.Register(It.IsAny<User>())).Returns(GetUserWithHashedPass);
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(new ValidationResult());
            var controller = new AccountController(mock.Object);
            var newUser = GetUserCorrectModel();

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<OkObjectResult>(result);
        }

        #region Incorrect First Name cases

        //---------------------------
        //Кейсы с неправильным именем
        //---------------------------
        [Fact]
        public void RegisterUser_BadRequestIncorrectFirstNameWithNotOnlyLetters ()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.containsLettersOnly.FormatWith(new { PropertyName = "First Name" });
            validationResult.Errors.Add(new ValidationFailure("First Name",errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_strWithNums, _correctSecondName, _correctFatherName,
                _correctEmail, _correctPassword, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.containsLettersOnly.FormatWith(new {PropertyName = "First Name"}), 
                returnedResult.Value.ToString());
        }

        [Fact]
        public void RegisterUser_BadRequestIncorrectFirstNameEmpty()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.notEmpty.FormatWith(new { PropertyName = "First Name" });
            validationResult.Errors.Add(new ValidationFailure("First Name", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User("", _correctSecondName, _correctFatherName,
                _correctEmail, _correctPassword, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.notEmpty.FormatWith(new { PropertyName = "First Name" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void RegisterUser_BadRequestIncorrectFirstNameTooLong()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "First Name", MaxLength = ValidationRules.maxNamesLength });
            validationResult.Errors.Add(new ValidationFailure("First Name", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_tooLongString, _correctSecondName, _correctFatherName,
                _correctEmail, _correctPassword, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "First Name", MaxLength = ValidationRules.maxNamesLength }),
                returnedResult.Value.ToString());
        }
        #endregion

        #region Incorrect Second Name cases
        //-----------------------------
        //Кейсы с неправильной фамилией
        //-----------------------------

        [Fact]
        public void RegisterUser_BadRequestIncorrectSecondNameWithNotOnlyLetters()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.containsLettersOnly.FormatWith(new { PropertyName = "Second Name" });
            validationResult.Errors.Add(new ValidationFailure("Second Name", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _strWithNums, _correctFatherName,
                _correctEmail, _correctPassword, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.containsLettersOnly.FormatWith(new { PropertyName = "Second Name" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void RegisterUser_BadRequestIncorrectSecondNameEmpty()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.notEmpty.FormatWith(new { PropertyName = "Second Name" });
            validationResult.Errors.Add(new ValidationFailure("Second Name", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, "", _correctFatherName,
                _correctEmail, _correctPassword, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.notEmpty.FormatWith(new { PropertyName = "Second Name" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void RegisterUser_BadRequestIncorrectSecondNameTooLong()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Second Name", MaxLength = ValidationRules.maxNamesLength });
            validationResult.Errors.Add(new ValidationFailure("Second Name", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _tooLongString, _correctFatherName,
                _correctEmail, _correctPassword, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Second Name", MaxLength = ValidationRules.maxNamesLength }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void RegisterUser_BadRequestIncorrectSecondNameTooShort()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooShortStr.FormatWith(
                new { PropertyName = "Second Name", MinLength = ValidationRules.minNamesLength });
            validationResult.Errors.Add(new ValidationFailure("Second Name", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _singleChar, _correctFatherName,
                _correctEmail, _correctPassword, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooShortStr.FormatWith(
                new { PropertyName = "Second Name", MinLength = ValidationRules.minNamesLength }),
                returnedResult.Value.ToString());
        }

        #endregion

        #region Incorrect Father Name cases
        //-----------------------------
        //Кейсы с неправильным отчеством
        //-----------------------------

        [Fact]
        public void RegisterUser_BadRequestIncorrectFatherNameWithNotOnlyLetters()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.containsLettersOnly.FormatWith(new { PropertyName = "Father Name" });
            validationResult.Errors.Add(new ValidationFailure("Father Name", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _correctSecondName, _strWithNums,
                _correctEmail, _correctPassword, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.containsLettersOnly.FormatWith(new { PropertyName = "Father Name" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void RegisterUser_BadRequestIncorrectFatherNameTooLong()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Father Name", MaxLength = ValidationRules.maxNamesLength });
            validationResult.Errors.Add(new ValidationFailure("Father Name", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _correctSecondName, _tooLongString,
                _correctEmail, _correctPassword, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Father Name", MaxLength = ValidationRules.maxNamesLength }),
                returnedResult.Value.ToString());
        }

        #endregion

        #region Incorrect Email cases

        //-----------------------------
        //Почта некорректна
        //-----------------------------

        [Fact]
        public void RegisterUser_BadRequestIncorrectEmail()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.emailIncorrect.FormatWith(new { PropertyName = "Email" });
            validationResult.Errors.Add(new ValidationFailure("Email", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _correctSecondName, _correctFatherName,
                _incorrectEmail, _correctPassword, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.emailIncorrect.FormatWith(new { PropertyName = "Email" }),
                returnedResult.Value.ToString());
        }

        #endregion

        #region Incorrect Password Cases

        //-----------------------------
        //Кейсы с неправильным паролем
        //-----------------------------

        [Fact]
        public void RegisterUser_BadRequestPasswordTooShort()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooShortStr.FormatWith(
                new { PropertyName = "Password", MinLength = ValidationRules.minPasswordLength });
            validationResult.Errors.Add(new ValidationFailure("Password", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _correctSecondName, _correctFatherName,
                _correctEmail, _tooShortPassword, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooShortStr.FormatWith(
                new { PropertyName = "Password", MinLength = ValidationRules.minPasswordLength }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void RegisterUser_BadRequestPasswordTooLong()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Password", MaxLength = ValidationRules.maxPasswordLength });
            validationResult.Errors.Add(new ValidationFailure("Password", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _correctSecondName, _correctFatherName,
                _correctEmail, _tooLongPassword, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Password", MaxLength = ValidationRules.maxPasswordLength }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void RegisterUser_BadRequestPasswordNoUpper()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.atLeastOneUpCase.FormatWith(new { PropertyName = "Password" });
            validationResult.Errors.Add(new ValidationFailure("Password", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _correctSecondName, _correctFatherName,
                _correctEmail, _passwordAllLower, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.atLeastOneUpCase.FormatWith(new { PropertyName = "Password" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void RegisterUser_BadRequestPasswordNoLower()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.atLeastOneLowCase.FormatWith(new { PropertyName = "Password" });
            validationResult.Errors.Add(new ValidationFailure("Password", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _correctSecondName, _correctFatherName,
                _correctEmail, _passwordAllUpper, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.atLeastOneLowCase.FormatWith(new { PropertyName = "Password" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void RegisterUser_BadRequestPasswordNoNum()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.atLeastOneNum.FormatWith(new { PropertyName = "Password" });
            validationResult.Errors.Add(new ValidationFailure("Password", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _correctSecondName, _correctFatherName,
                _correctEmail, _passwordNoNums, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.atLeastOneNum.FormatWith(new { PropertyName = "Password" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void RegisterUser_BadRequestPasswordNoSpecChars()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.atLeastOneSpecChar.FormatWith(new { PropertyName = "Password" });
            validationResult.Errors.Add(new ValidationFailure("Password", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _correctSecondName, _correctFatherName,
                _correctEmail, _passwordNoSpecChars, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.atLeastOneSpecChar.FormatWith(new { PropertyName = "Password" }),
                returnedResult.Value.ToString());
        }

        #endregion

        #region Incorrect Address cases

        //-----------------------------
        //Кейсы с неправильным адресом
        //-----------------------------

        [Fact]
        public void RegisterUser_BadRequestAddressEmpty()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.notEmpty.FormatWith(new { PropertyName = "Address" });
            validationResult.Errors.Add(new ValidationFailure("Address", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).
                Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _correctSecondName, _correctFatherName,
                _correctEmail, _correctPassword, _tooLongAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.notEmpty.FormatWith(new { PropertyName = "Address" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void RegisterUser_BadRequestAddressTooLong()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Address", MaxLength = ValidationRules.maxAddressLength });
            validationResult.Errors.Add(new ValidationFailure("Address", errorMsg));
            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).
                Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUser = new User(_correctFirstName, _correctSecondName, _correctFatherName,
                _correctEmail, _passwordNoSpecChars, _correctAddress);

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Address", MaxLength = ValidationRules.maxAddressLength }),
                returnedResult.Value.ToString());
        }

        #endregion

        #region User Already Exists cases

        //---------------------------------------
        //Пользователь с введенным email уже есть
        //---------------------------------------

        [Fact]
        public void RegisterUser_BadRequestUserAlreadyExists()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            mock.Setup(a => a.Register(It.IsAny<User>()))
                .Throws(new Exception(UserData.userAlreadyExists));

            mock.Setup(a => a.ValidateUser(It.IsAny<User>())).Returns(new ValidationResult());

            var controller = new AccountController(mock.Object);
            var newUser = GetUserCorrectModel();

            //Act
            var result = controller.RegisterUser(newUser);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(UserData.userAlreadyExists, returnedResult.Value);
        }

        #endregion

        #endregion

        #region ChangePassword Method Tests

        #region All Data Correct
        //-----------------------
        //---Все данные корректны
        //-----------------------

        [Fact]
        public void ChangePassword_ShouldReturnOk()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            mock.Setup(a => a.ChangePassword(It.IsAny<UserChangePassword>()));

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _correctPassword, _correctNewPassword);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<OkObjectResult>(result);
        }

        #endregion

        #region User Doesnt Exist
        //-----------------------------
        //---Пользователь не существует 
        //-----------------------------

        [Fact]
        public void ChangePassword_BadRequestUserDoesntExist()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            mock.Setup(a => a.ChangePassword(It.IsAny<UserChangePassword>()))
                .Throws(new Exception(UserData.userDoesntExist));

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_unregisteredEmail, _correctPassword, _correctNewPassword);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(UserData.userDoesntExist, returnedResult.Value.ToString());
        }

        #endregion

        #region Old Password Doesnt Match

        [Fact]
        public void ChangePassword_BadRequestUserOldPassDoesntMatch()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            mock.Setup(a => a.ChangePassword(It.IsAny<UserChangePassword>()))
                .Throws(new Exception(UserData.oldPassDoesntMatch));

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _correctNewPassword, _correctPassword);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(UserData.oldPassDoesntMatch, returnedResult.Value.ToString());
        }

        #endregion

        #region Incorrect Email

        [Fact]
        public void ChangePassword_BadRequestIncorrectEmail()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.emailIncorrect.FormatWith(new { PropertyName = "Email" });
            validationResult.Errors.Add(new ValidationFailure("Email", errorMsg));
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_incorrectEmail, _correctPassword, _correctNewPassword);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.emailIncorrect.FormatWith(new { PropertyName = "Email" }), 
                returnedResult.Value.ToString());
        }

        #endregion

        #region Incorrect Old Password

        [Fact]
        public void ChangePassword_BadRequestOldPasswordTooShort()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooShortStr.FormatWith(
                new { PropertyName = "Old Password", MinLength = ValidationRules.minPasswordLength });
            validationResult.Errors.Add(new ValidationFailure("Old Password", errorMsg));
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _tooShortPassword, _correctNewPassword);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooShortStr.FormatWith(
                new { PropertyName = "Old Password", MinLength = ValidationRules.minPasswordLength }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangePassword_BadRequestOldPasswordTooLong()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Old Password", MaxLength = ValidationRules.maxPasswordLength });
            validationResult.Errors.Add(new ValidationFailure("Old Password", errorMsg));
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _tooLongPassword, _correctNewPassword);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Old Password", MaxLength = ValidationRules.maxPasswordLength }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangePassword_BadRequestOldPasswordNoUpper()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.atLeastOneUpCase.FormatWith(new { PropertyName = "Old Password" });
            validationResult.Errors.Add(new ValidationFailure("Old Password", errorMsg));
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult); ;

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _passwordAllLower, _correctNewPassword);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.atLeastOneUpCase.FormatWith(new { PropertyName = "Old Password" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangePassword_BadRequestOldPasswordNoLower()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.atLeastOneLowCase.FormatWith(new { PropertyName = "Old Password" });
            validationResult.Errors.Add(new ValidationFailure("Old Password", errorMsg));
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _passwordAllUpper, _correctNewPassword);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.atLeastOneLowCase.FormatWith(new { PropertyName = "Old Password" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangePassword_BadRequestOldPasswordNoNum()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.atLeastOneNum.FormatWith(new { PropertyName = "Old Password" });
            validationResult.Errors.Add(new ValidationFailure("Old Password", errorMsg));
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _passwordNoNums, _correctNewPassword);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.atLeastOneNum.FormatWith(new { PropertyName = "Old Password" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangePassword_BadRequestOldPasswordNoSpecChars()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.atLeastOneSpecChar.FormatWith(new { PropertyName = "Old Password" });
            validationResult.Errors.Add(new ValidationFailure("Old Password", errorMsg));
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _passwordNoSpecChars, _correctNewPassword);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.atLeastOneSpecChar.FormatWith(new { PropertyName = "Old Password" }),
                returnedResult.Value.ToString());
        }

        #endregion

        #region Incorrect New Password

        [Fact]
        public void ChangePassword_BadRequestNewPasswordTooShort()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooShortStr.FormatWith(
                new { PropertyName = "New Password", MinLength = ValidationRules.minPasswordLength });
            validationResult.Errors.Add(new ValidationFailure("New Password", errorMsg));
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _correctPassword, _tooShortPassword);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooShortStr.FormatWith(
                new { PropertyName = "New Password", MinLength = ValidationRules.minPasswordLength }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangePassword_BadRequestNewPasswordTooLong()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "New Password", MaxLength = ValidationRules.maxPasswordLength });
            validationResult.Errors.Add(new ValidationFailure("New Password", errorMsg));
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _correctPassword, _tooLongPassword);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "New Password", MaxLength = ValidationRules.maxPasswordLength }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangePassword_BadRequestNewPasswordNoUpper()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.atLeastOneUpCase.FormatWith(new { PropertyName = "New Password" });
            validationResult.Errors.Add(new ValidationFailure("New Password", errorMsg));
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult); ;

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _correctPassword, _passwordAllLower);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.atLeastOneUpCase.FormatWith(new { PropertyName = "New Password" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangePassword_BadRequestNewPasswordNoLower()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.atLeastOneLowCase.FormatWith(new { PropertyName = "New Password" });
            validationResult.Errors.Add(new ValidationFailure("New Password", errorMsg));
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _correctPassword, _passwordAllUpper);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.atLeastOneLowCase.FormatWith(new { PropertyName = "New Password" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangePassword_BadRequestNewPasswordNoNum()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.atLeastOneNum.FormatWith(new { PropertyName = "New Password" });
            validationResult.Errors.Add(new ValidationFailure("New Password", errorMsg));
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _correctPassword, _passwordNoNums);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.atLeastOneNum.FormatWith(new { PropertyName = "New Password" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangePassword_BadRequestNewPasswordNoSpecChars()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.atLeastOneSpecChar.FormatWith(new { PropertyName = "New Password" });
            validationResult.Errors.Add(new ValidationFailure("New Password", errorMsg));
            mock.Setup(a => a.ValidateUserChangePassword(It.IsAny<UserChangePassword>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangePassword = new UserChangePassword(_correctEmail, _correctPassword, _passwordNoSpecChars);

            //Act
            var result = controller.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.atLeastOneSpecChar.FormatWith(new { PropertyName = "New Password" }),
                returnedResult.Value.ToString());
        }

        #endregion

        #endregion

        #region ChangeDetails Method Tests

        #region All Data Correct

        //-----------------------------
        //Все исходные данные корректны
        //-----------------------------
        [Fact]
        public void ChangeDetails_ShouldReturnOk()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();
            mock.Setup(a => a.ChangeDetails(It.IsAny<UserChangeDetails>()));
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).Returns(new ValidationResult());
            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_correctEmail, _correctFirstName,
                _correctSecondName, _correctFatherName, _correctAddress);

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<OkObjectResult>(result);
        }

        #endregion

        #region Incorrect First Name cases

        //---------------------------
        //Кейсы с неправильным именем
        //---------------------------
        [Fact]
        public void ChangeDetails_BadRequestIncorrectFirstNameWithNotOnlyLetters()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.containsLettersOnly.FormatWith(new { PropertyName = "First Name" });
            validationResult.Errors.Add(new ValidationFailure("First Name", errorMsg));
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_correctEmail, _strWithNums,
                _correctSecondName, _correctFatherName, _correctAddress);

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.containsLettersOnly.FormatWith(new { PropertyName = "First Name" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangeDetails_BadRequestIncorrectFirstNameEmpty()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.notEmpty.FormatWith(new { PropertyName = "First Name" });
            validationResult.Errors.Add(new ValidationFailure("First Name", errorMsg));
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_correctEmail, "",
                _correctSecondName, _correctFatherName, _correctAddress);

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.notEmpty.FormatWith(new { PropertyName = "First Name" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangeDetails_BadRequestIncorrectFirstNameTooLong()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "First Name", MaxLength = ValidationRules.maxNamesLength });
            validationResult.Errors.Add(new ValidationFailure("First Name", errorMsg));
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_correctEmail, _tooLongString,
                _correctSecondName, _correctFatherName, _correctAddress);

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "First Name", MaxLength = ValidationRules.maxNamesLength }),
                returnedResult.Value.ToString());
        }
        #endregion

        #region Incorrect Second Name cases
        //-----------------------------
        //Кейсы с неправильной фамилией
        //-----------------------------

        [Fact]
        public void ChangeDetails_BadRequestIncorrectSecondNameWithNotOnlyLetters()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.containsLettersOnly.FormatWith(new { PropertyName = "Second Name" });
            validationResult.Errors.Add(new ValidationFailure("Second Name", errorMsg));
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_correctEmail, _correctFirstName,
                _strWithNums, _correctFatherName, _correctAddress);

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.containsLettersOnly.FormatWith(new { PropertyName = "Second Name" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangeDetails_BadRequestIncorrectSecondNameEmpty()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.notEmpty.FormatWith(new { PropertyName = "Second Name" });
            validationResult.Errors.Add(new ValidationFailure("Second Name", errorMsg));
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_correctEmail, _correctFirstName,
                "", _correctFatherName, _correctAddress);

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.notEmpty.FormatWith(new { PropertyName = "Second Name" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangeDetails_BadRequestIncorrectSecondNameTooLong()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Second Name", MaxLength = ValidationRules.maxNamesLength });
            validationResult.Errors.Add(new ValidationFailure("Second Name", errorMsg));
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_correctEmail, _correctFirstName,
                _tooLongString, _correctFatherName, _correctAddress);

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Second Name", MaxLength = ValidationRules.maxNamesLength }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangeDetails_BadRequestIncorrectSecondNameTooShort()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooShortStr.FormatWith(
                new { PropertyName = "Second Name", MinLength = ValidationRules.minNamesLength });
            validationResult.Errors.Add(new ValidationFailure("Second Name", errorMsg));
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_correctEmail, _correctFirstName,
                _singleChar, _correctFatherName, _correctAddress);

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooShortStr.FormatWith(
                new { PropertyName = "Second Name", MinLength = ValidationRules.minNamesLength }),
                returnedResult.Value.ToString());
        }

        #endregion

        #region Incorrect Father Name cases
        //-----------------------------
        //Кейсы с неправильным отчеством
        //-----------------------------

        [Fact]
        public void ChangeDetails_BadRequestIncorrectFatherNameWithNotOnlyLetters()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.containsLettersOnly.FormatWith(new { PropertyName = "Father Name" });
            validationResult.Errors.Add(new ValidationFailure("Father Name", errorMsg));
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_correctEmail, _correctFirstName,
                _correctSecondName, _strWithNums, _correctAddress);

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.containsLettersOnly.FormatWith(new { PropertyName = "Father Name" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangeDetails_BadRequestIncorrectFatherNameTooLong()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Father Name", MaxLength = ValidationRules.maxNamesLength });
            validationResult.Errors.Add(new ValidationFailure("Father Name", errorMsg));
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_correctEmail, _correctFirstName,
                _correctSecondName, _tooLongString, _correctAddress);

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Father Name", MaxLength = ValidationRules.maxNamesLength }),
                returnedResult.Value.ToString());
        }

        #endregion

        #region Incorrect Email cases

        //-----------------------------
        //Почта некорректна
        //-----------------------------

        [Fact]
        public void ChangeDetails_BadRequestIncorrectEmail()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.emailIncorrect.FormatWith(new { PropertyName = "Email" });
            validationResult.Errors.Add(new ValidationFailure("Email", errorMsg));
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_incorrectEmail, _correctFirstName,
                _correctSecondName, _correctFatherName, _correctAddress);

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.emailIncorrect.FormatWith(new { PropertyName = "Email" }),
                returnedResult.Value.ToString());
        }

        #endregion

        #region Incorrect Address cases

        //-----------------------------
        //Кейсы с неправильным адресом
        //-----------------------------

        [Fact]
        public void ChangeDetails_BadRequestAddressEmpty()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.notEmpty.FormatWith(new { PropertyName = "Address" });
            validationResult.Errors.Add(new ValidationFailure("Address", errorMsg));
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).
                Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_correctEmail, _correctFirstName,
                _correctSecondName, _correctFatherName, "");

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.notEmpty.FormatWith(new { PropertyName = "Address" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void ChangeDetails_BadRequestAddressTooLong()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Address", MaxLength = ValidationRules.maxAddressLength });
            validationResult.Errors.Add(new ValidationFailure("Address", errorMsg));
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).
                Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_correctEmail, _correctFirstName,
                _correctSecondName, _correctFatherName, _tooLongAddress);

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.tooLongStr.FormatWith(
                new { PropertyName = "Address", MaxLength = ValidationRules.maxAddressLength }),
                returnedResult.Value.ToString());
        }

        #endregion

        #region User Doesnt Exist
        //-----------------------------
        //---Пользователь не существует 
        //-----------------------------

        [Fact]
        public void ChangeDetails_BadRequestUserDoesntExist()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            mock.Setup(a => a.ValidateUserChangeDetails(It.IsAny<UserChangeDetails>())).Returns(validationResult);

            mock.Setup(a => a.ChangeDetails(It.IsAny<UserChangeDetails>()))
                .Throws(new Exception(UserData.userDoesntExist));

            var controller = new AccountController(mock.Object);
            var newUserChangeDetails = new UserChangeDetails(_correctEmail, _correctFirstName,
                _correctSecondName, _correctFatherName, _correctAddress);

            //Act
            var result = controller.ChangeDetails(newUserChangeDetails);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(UserData.userDoesntExist, returnedResult.Value.ToString());
        }

        #endregion

        #endregion

        #region Login Method Tests

        #region All Data Correct
        //-----------------------------
        //Все исходные данные корректны
        //-----------------------------
        [Fact]
        public void Login_ShouldReturnUserObject()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();
            mock.Setup(a => a.Login(It.IsAny<UserLogin>())).Returns(GetUserWithHashedPass);
            mock.Setup(a => a.ValidateUserLogin(It.IsAny<UserLogin>())).Returns(new ValidationResult());
            var controller = new AccountController(mock.Object);
            var userLogin = new UserLogin(_correctEmail, _correctPassword);

            //Act
            var result = controller.Login(userLogin);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<OkObjectResult>(result);
        }

        #endregion

        #region Incorrect Email Cases

        //-----------------------------
        //Почта некорректна
        //-----------------------------

        [Fact]
        public void Login_BadRequestIncorrectEmail()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.emailIncorrect.FormatWith(new { PropertyName = "Email" });
            validationResult.Errors.Add(new ValidationFailure("Email", errorMsg));
            mock.Setup(a => a.ValidateUserLogin(It.IsAny<UserLogin>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var userLogin = new UserLogin(_correctEmail, _correctPassword);

            //Act
            var result = controller.Login(userLogin);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.emailIncorrect.FormatWith(new { PropertyName = "Email" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void Login_BadRequestUserDoesntExist()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            mock.Setup(a => a.ValidateUserLogin(It.IsAny<UserLogin>())).Returns(validationResult);

            mock.Setup(a => a.Login(It.IsAny<UserLogin>()))
                .Throws(new Exception(UserData.userDoesntExist));

            var controller = new AccountController(mock.Object);
            var userLogin = new UserLogin(_correctEmail, _correctPassword);

            //Act
            var result = controller.Login(userLogin);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(UserData.userDoesntExist,returnedResult.Value.ToString());
        }

        #endregion

        #region Incorrect Password Cases

        //-----------------------------
        //Кейсы с неправильным паролем
        //-----------------------------

        [Fact]
        public void Login_BadRequestPasswordEmpty()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            var errorMsg = ValidationRules.notEmpty.FormatWith( new { PropertyName = "Password" });
            validationResult.Errors.Add(new ValidationFailure("Password", errorMsg));
            mock.Setup(a => a.ValidateUserLogin(It.IsAny<UserLogin>())).Returns(validationResult);

            var controller = new AccountController(mock.Object);
            var userLogin = new UserLogin(_correctEmail, _correctPassword);

            //Act
            var result = controller.Login(userLogin);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(ValidationRules.notEmpty.FormatWith(new { PropertyName = "Password" }),
                returnedResult.Value.ToString());
        }

        [Fact]
        public void Login_BadRequestWrongPassword()
        {
            //Arrange
            var mock = new Mock<IUserRepository>();

            var validationResult = new ValidationResult();
            mock.Setup(a => a.ValidateUserLogin(It.IsAny<UserLogin>())).Returns(validationResult);

            mock.Setup(a => a.Login(It.IsAny<UserLogin>()))
                .Throws(new Exception(UserData.wrongPass));

            var controller = new AccountController(mock.Object);
            var userLogin = new UserLogin(_correctEmail, _correctPassword);

            //Act
            var result = controller.Login(userLogin);

            //Assert
            mock.VerifyAll();
            var returnedResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(UserData.wrongPass, returnedResult.Value.ToString());
        }

        #endregion

        #endregion

    }
}

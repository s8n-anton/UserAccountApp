using FluentValidation.Results;
using Moq;
using UserAccountApp.Models;
using UserAccountApp.Services;

namespace UserAccountApp.Tests
{
    public class TestUserData : TestBase
    {
        #region GetAll Method Tests
        [Fact]
        public void GetAll_ShouldReturnListOfUsers()
        {
            //Arrange
            var userData = new UserData();

            //Act
            var result = userData.GetAll();

            //Assert
            Assert.IsType<List<User>>(result);
        }

        [Fact]
        public void GetByEmail_ShouldReturnUser()
        {
            //Arrange
            var userData = new UserData();
            var firstUser = userData.GetAll().First();

            //Act
            var result = userData.GetByEmail(firstUser.Email);

            //Assert
            Assert.IsType<User>(result);
            Assert.Equal(firstUser, result);
        }

        [Fact]
        public void GetByEmail_ShouldReturnNull()
        {
            //Arrange
            var userData = new UserData();

            //Act
            var result = userData.GetByEmail(_correctEmail);

            //Assert
            Assert.Null(result);
        }

        #endregion

        #region Register Method Tests

        [Fact]
        public void Register_ShouldReturnUser()
        {
            //Arrange
            var userData = new UserData();
            var newUser = GetUserCorrectModel();

            //Act
            var result = userData.Register(newUser);

            //Assert
            Assert.IsType<User>(result);
            Assert.Equal(newUser.FirstName, result.FirstName);
            Assert.Equal(newUser.SecondName, result.SecondName);
            Assert.Equal(newUser.FatherName, result.FatherName);
            Assert.Equal(newUser.Email, result.Email);
            Assert.Equal(newUser.Password, result.Password);
            Assert.Equal(newUser.Address, result.Address);
        }

        [Fact]
        public void Register_ExceptionUserAlreadyExists()
        {
            //Arrange
            var userData = new UserData();
            var existingUser = GetUsers().First();

            //Act
            Action act = () => userData.Register(existingUser);

            //Assert
            var exception = Assert.Throws<Exception>(act);
            Assert.Equal(UserData.userAlreadyExists, exception.Message);
        }

        #endregion

        #region ChangePassword Method Tests

        [Fact]
        public void ChangePassword_ChangesPass()
        {
            //Arrange
            var mock = new Mock<UserData>();
            var user = GetUserWithHashedPass();
            mock.Setup(a => a.GetByEmail(It.IsAny<string>())).Returns(user);
            var newUserChangePassword = new UserChangePassword(user.Email,
                _correctPassword, _correctNewPassword);

            //Act
            mock.Object.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            Assert.NotEqual(_correctHashedPassword, user.Password);
        }

        [Fact]
        public void ChangePassword_ExceptionOldPassDoesntMatch()
        {
            //Arrange
            var mock = new Mock<UserData>();
            mock.Setup(a => a.GetByEmail(It.IsAny<string>())).Returns(GetUserWithHashedPass());
            var user = mock.Object.GetByEmail(GetUserWithHashedPass().Email);
            var newUserChangePassword = new UserChangePassword(user.Email, 
                _correctPassword+"!",_correctNewPassword);

            //Act
            Action act = () => mock.Object.ChangePassword(newUserChangePassword);

            //Assert
            var exception = Assert.Throws<Exception>(act);
            Assert.Equal(UserData.oldPassDoesntMatch, exception.Message);
        }

        [Fact]
        public void ChangePassword_ExceptionUserDoesntExist()
        {
            //Arrange
            var user = GetUserWithHashedPass();
            var userData = new UserData();
            var newUserChangePassword = new UserChangePassword("new"+user.Email,
                _correctPassword, _correctNewPassword);

            //Act
            Action act = () => userData.ChangePassword(newUserChangePassword);

            //Assert
            var exception = Assert.Throws<Exception>(act);
            Assert.Equal(UserData.userDoesntExist, exception.Message);
        }

        #endregion

        #region ChangeDetails Method Tests

        [Fact]
        public void ChangeDetails_ChangesDetails()
        {
            //Arrange
            var user = GetUserWithHashedPass();
            var mock = new Mock<UserData>();
            mock.Setup(a => a.GetByEmail(It.IsAny<string>())).Returns(user);

            var newFirstName = user.FirstName + "a";
            var newAddress = "New Address";
            var userChangeDetails = new UserChangeDetails(user.Email,
                newFirstName, user.SecondName, user.FatherName, newAddress);

            //Act
            mock.Object.ChangeDetails(userChangeDetails);

            //Assert
            mock.VerifyAll();
            Assert.Equal(newFirstName, user.FirstName);
            Assert.Equal(newAddress, user.Address);
        }

        [Fact]
        public void ChangeDetails_ExceptionUserDoesntExist()
        {
            //Arrange
            var user = GetUserWithHashedPass();
            var userData = new UserData();
            var userChangeDetails = new UserChangeDetails("new" + user.Email,
                _correctFirstName+"a", _correctSecondName, _correctFatherName, _correctAddress);

            //Act
            Action act = () => userData.ChangeDetails(userChangeDetails);

            //Assert
            var exception = Assert.Throws<Exception>(act);
            Assert.Equal(UserData.userDoesntExist, exception.Message);
        }

        #endregion

        #region Login Method Tests

        [Fact]
        public void Login_ReturnsUser()
        {
            //Arrange
            var user = GetUserWithHashedPass();
            var userData = new UserData();
            var userLogin = new UserLogin(user.Email, _correctPassword);

            //Act
            var result = userData.Login(userLogin);

            //Assert
            Assert.IsType<User>(result);
        }

        [Fact]
        public void Login_ExceptionWrongPass()
        {
            //Arrange
            var user = GetUserWithHashedPass();
            var mock = new Mock<UserData>();
            mock.Setup(a => a.GetByEmail(It.IsAny<string>())).Returns(user);
            var userLogin = new UserLogin(user.Email, _correctPassword + "Qwe1*");

            //Act
            Action act = () => mock.Object.Login(userLogin);

            //Assert
            var exception = Assert.Throws<Exception>(act);
            mock.VerifyAll();
            Assert.Equal(UserData.wrongPass, exception.Message);
        }

        [Fact]
        public void Login_ExceptionUserDoesntExist()
        {
            //Arrange
            var user = GetUserWithHashedPass();
            var userData = new UserData();
            var userLogin = new UserLogin("new"+user.Email, _correctPassword);

            //Act
            Action act = () => userData.Login(userLogin);

            //Assert
            var exception = Assert.Throws<Exception>(act);
            Assert.Equal(UserData.userDoesntExist, exception.Message);
        }

        #endregion

        #region ValidateUser Method Tests

        [Fact]
        public void ValidateUser_Valid()
        {
            //Arrange
            var user = GetUserCorrectModel();
            var userData = new UserData();

            //Act
            var result = userData.ValidateUser(user);

            //Assert
            Assert.IsType<ValidationResult>(result);
            Assert.True(result.IsValid);
        }

        [Fact]
        public void ValidateUser_FirstNameIncorrect()
        {
            //Arrange
            var user = GetUserCorrectModel();
            user.FirstName = _correctFirstName + "1!";
            var userData = new UserData();

            //Act
            var result = userData.ValidateUser(user);

            //Assert
            Assert.IsType<ValidationResult>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, error => error.PropertyName.Equals("FirstName"));
        }

        [Fact]
        public void ValidateUser_SecondNameIncorrect()
        {
            //Arrange
            var user = GetUserCorrectModel();
            user.SecondName = _correctSecondName + "*94";
            var userData = new UserData();

            //Act
            var result = userData.ValidateUser(user);

            //Assert
            Assert.IsType<ValidationResult>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, error => error.PropertyName.Equals("SecondName"));
        }

        [Fact]
        public void ValidateUser_FatherNameIncorrect()
        {
            //Arrange
            var user = GetUserCorrectModel();
            user.FatherName = _correctFatherName + " ";
            var userData = new UserData();

            //Act
            var result = userData.ValidateUser(user);

            //Assert
            Assert.IsType<ValidationResult>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, error => error.PropertyName.Equals("FatherName"));
        }

        [Fact]
        public void ValidateUser_EmailIncorrect()
        {
            //Arrange
            var user = GetUserCorrectModel();
            user.Email = _incorrectEmail;
            var userData = new UserData();

            //Act
            var result = userData.ValidateUser(user);

            //Assert
            Assert.IsType<ValidationResult>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, error => error.PropertyName.Equals("Email"));
        }

        [Fact]
        public void ValidateUser_PasswordNoUpperNoNumsNoSpecCharsTooShort()
        {
            //Arrange
            var user = GetUserCorrectModel();
            user.Password = "ezpass";
            var userData = new UserData();

            //Act
            var result = userData.ValidateUser(user);

            //Assert
            Assert.IsType<ValidationResult>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(4, result.Errors.Count);
            Assert.Contains(result.Errors, error => error.PropertyName.Equals("Password"));
        }

        [Fact]
        public void ValidateUser_PasswordNoUpperNoNumsNoSpecChars()
        {
            //Arrange
            var user = GetUserCorrectModel();
            user.Password = "easypass";
            var userData = new UserData();

            //Act
            var result = userData.ValidateUser(user);

            //Assert
            Assert.IsType<ValidationResult>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(3, result.Errors.Count);
            Assert.Contains(result.Errors, error => error.PropertyName.Equals("Password"));
        }

        [Fact]
        public void ValidateUser_PasswordNoLowerNoNums()
        {
            //Arrange
            var user = GetUserCorrectModel();
            user.Password = "EASYPASSWORD!";
            var userData = new UserData();

            //Act
            var result = userData.ValidateUser(user);

            //Assert
            Assert.IsType<ValidationResult>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Equal(2, result.Errors.Count);
            Assert.Contains(result.Errors, error => error.PropertyName.Equals("Password"));
        }

        [Fact]
        public void ValidateUser_IncorrectAddress()
        {
            //Arrange
            var user = GetUserCorrectModel();
            user.Address = _tooLongAddress;
            var userData = new UserData();

            //Act
            var result = userData.ValidateUser(user);

            //Assert
            Assert.IsType<ValidationResult>(result);
            Assert.NotEmpty(result.Errors);
            Assert.Contains(result.Errors, error => error.PropertyName.Equals("Address"));
        }

        #endregion
    }
}

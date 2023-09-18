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
            var mock = new Mock<IUserDataGenerator>();
            mock.Setup(a => a.GenerateUsers()).Returns(GetUsers());
            var userData = new UserData(mock.Object);

            //Act
            var result = userData.GetAll();

            //Assert
            mock.VerifyAll();
            Assert.IsType<List<User>>(result);
        }

        [Fact]
        public void GetByEmail_ShouldReturnUser()
        {
            //Arrange
            var mock = new Mock<IUserDataGenerator>();
            mock.Setup(a => a.GenerateUsers()).Returns(GetUsers());
            var userData = new UserData(mock.Object);
            var firstUser = userData.GetAll().First();

            //Act
            var result = userData.GetByEmail(firstUser.Email);

            //Assert
            mock.VerifyAll();
            Assert.IsType<User>(result);
            Assert.Equal(firstUser, result);
        }

        [Fact]
        public void GetByEmail_ShouldReturnNull()
        {
            //Arrange
            var mock = new Mock<IUserDataGenerator>();
            mock.Setup(a => a.GenerateUsers()).Returns(GetUsers());
            var userData = new UserData(mock.Object);

            //Act
            var result = userData.GetByEmail(_correctEmail);

            //Assert
            mock.VerifyAll();
            Assert.Null(result);
        }

        #endregion

        #region Register Method Tests

        [Fact]
        public void Register_ShouldReturnUser()
        {
            //Arrange
            var mock = new Mock<IUserDataGenerator>();
            mock.Setup(a => a.GenerateUsers()).Returns(GetUsers());
            var userData = new UserData(mock.Object);
            var newUser = GetUserCorrectModel();

            //Act
            var result = userData.Register(newUser);

            //Assert
            mock.VerifyAll();
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
            var mock = new Mock<IUserDataGenerator>();
            mock.Setup(a => a.GenerateUsers()).Returns(GetUsers());
            var userData = new UserData(mock.Object);
            var existingUser = GetUsers().First();

            //Act
            Action act = () => userData.Register(existingUser);

            //Assert
            mock.VerifyAll();
            var exception = Assert.Throws<Exception>(act);
            Assert.Equal(UserData.userAlreadyExists, exception.Message);
        }

        #endregion

        #region ChangePassword Method Tests

        [Fact]
        public void ChangePassword_ChangesPass()
        {
            //Arrange
            var user = GetUserWithHashedPass();
            var mock = new Mock<IUserDataGenerator>();
            mock.Setup(a => a.GenerateUsers()).Returns(new List<User> { user });
            var userData = new UserData(mock.Object);
            var newUserChangePassword = new UserChangePassword(user.Email,
                _correctPassword, _correctNewPassword);

            //Act
            userData.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            Assert.NotEqual(_correctHashedPassword, user.Password);
        }

        [Fact]
        public void ChangePassword_ExceptionOldPassDoesntMatch()
        {
            //Arrange
            var user = GetUserWithHashedPass();
            var mock = new Mock<IUserDataGenerator>();
            mock.Setup(a => a.GenerateUsers()).Returns(new List<User> {user});
            var userData = new UserData(mock.Object);
            var newUserChangePassword = new UserChangePassword(user.Email, 
                _correctPassword+"!",_correctNewPassword);

            //Act
            Action act = () => userData.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
            var exception = Assert.Throws<Exception>(act);
            Assert.Equal(UserData.oldPassDoesntMatch, exception.Message);
        }

        [Fact]
        public void ChangePassword_ExceptionUserDoesntExist()
        {
            //Arrange
            var user = GetUserWithHashedPass();
            var mock = new Mock<IUserDataGenerator>();
            mock.Setup(a => a.GenerateUsers()).Returns(new List<User> { user });
            var userData = new UserData(mock.Object);
            var newUserChangePassword = new UserChangePassword("new"+user.Email,
                _correctPassword, _correctNewPassword);

            //Act
            Action act = () => userData.ChangePassword(newUserChangePassword);

            //Assert
            mock.VerifyAll();
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
            var mock = new Mock<IUserDataGenerator>();
            mock.Setup(a => a.GenerateUsers()).Returns(new List<User> { user });
            var userData = new UserData(mock.Object);
            var newFirstName = user.FirstName + "a";
            var newAddress = "New Address";
            var userChangeDetails = new UserChangeDetails(user.Email,
                newFirstName, user.SecondName, user.FatherName, newAddress);

            //Act
            userData.ChangeDetails(userChangeDetails);

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
            var mock = new Mock<IUserDataGenerator>();
            mock.Setup(a => a.GenerateUsers()).Returns(new List<User> { user });
            var userData = new UserData(mock.Object);
            var userChangeDetails = new UserChangeDetails("new" + user.Email,
                _correctFirstName+"a", _correctSecondName, _correctFatherName, _correctAddress);

            //Act
            Action act = () => userData.ChangeDetails(userChangeDetails);

            //Assert
            mock.VerifyAll();
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
            var mock = new Mock<IUserDataGenerator>();
            mock.Setup(a => a.GenerateUsers()).Returns(new List<User> { user });
            var userData = new UserData(mock.Object);
            var userLogin = new UserLogin(user.Email, _correctPassword);

            //Act
            var result = userData.Login(userLogin);

            //Assert
            mock.VerifyAll();
            Assert.IsType<User>(result);
        }

        [Fact]
        public void Login_ExceptionWrongPass()
        {
            //Arrange
            var user = GetUserWithHashedPass();
            var mock = new Mock<IUserDataGenerator>();
            mock.Setup(a => a.GenerateUsers()).Returns(new List<User> { user });
            var userData = new UserData(mock.Object);
            var userLogin = new UserLogin(user.Email, _correctPassword+"Qwe1*");

            //Act
            Action act = () => userData.Login(userLogin);

            //Assert
            mock.VerifyAll();
            var exception = Assert.Throws<Exception>(act);
            Assert.Equal(UserData.wrongPass, exception.Message);
        }

        [Fact]
        public void Login_ExceptionUserDoesntExist()
        {
            //Arrange
            var user = GetUserWithHashedPass();
            var mock = new Mock<IUserDataGenerator>();
            mock.Setup(a => a.GenerateUsers()).Returns(new List<User> { user });
            var userData = new UserData(mock.Object);
            var userLogin = new UserLogin("new"+user.Email, _correctPassword);

            //Act
            Action act = () => userData.Login(userLogin);

            //Assert
            mock.VerifyAll();
            var exception = Assert.Throws<Exception>(act);
            Assert.Equal(UserData.userDoesntExist, exception.Message);
        }

        #endregion


    }
}

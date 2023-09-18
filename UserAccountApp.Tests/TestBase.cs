using UserAccountApp.Models;

namespace UserAccountApp.Tests
{
    public class TestBase
    {
        #region Standard string fields for data imitation
        protected const string _correctFirstName = "Нина";
        protected const string _correctSecondName = "Мамонова";
        protected const string _correctFatherName = "Данииловна";
        protected const string _correctEmail = "nina83@yandex.ru";
        protected const string _unregisteredEmail = "ivan.ivanov@mail.ru";
        protected const string _correctPassword = "B4KdcLZk*";

        //Equals to _correctPassword hashed with PBKDF2 algorithm
        protected const string _correctHashedPassword = "ALOa/YhjmGMZh1qN1PsTFPXM+PVDcbibMwP67yL+DtXyjt3L2a7d9Gcrn2VtCw/Thg==";

        protected const string _correctNewPassword = "B4KdcLZk!";
        protected const string _correctAddress = "Россия, г. Орёл, Колхозный пер., д. 23 кв.38";
        protected const string _strWithNums = "Иван123";
        protected const string _tooLongString = "Антоооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооооон";
        protected const string _singleChar = "М";
        protected const string _tooShortPassword = "gRsj5rN";
        protected const string _tooLongPassword = "0lEEraygK9aX7FTQuL";
        protected const string _passwordAllLower = "0leerayg!k9a";
        protected const string _passwordAllUpper = "0LEEERAYG!K9A";
        protected const string _passwordNoNums = "LEEERAYG!KA";
        protected const string _passwordNoSpecChars = "0lEEraygK9a";
        protected const string _incorrectEmail = "nina83SOBAKAyandex.ru";
        protected const string _tooLongAddress = "Российская Федерация, Новосибирская область, г. Бердск, Территория, изъятая из земель подсобного хозяйства Всесоюзного центрального совета профессиональных союзов, для организации крестьянского хозяйства, дом 17";
        #endregion

        #region Helper Methods
        protected User GetUserWithHashedPass()
        {
            return new User(_correctFirstName, _correctSecondName, _correctFatherName,
                _correctEmail, _correctHashedPassword, _correctAddress);
        }

        protected User GetUserCorrectModel()
        {
            return new User(_correctFirstName, _correctSecondName, _correctFatherName,
                _correctEmail, _correctPassword, _correctAddress);
        }

        protected List<User> GetUsers()
        {
            return new List<User>() {
                new User("Валентин", "Козин", "Адамович", "valentin.kozin@hotmail.com", "yUxUh5gA", "Россия, г. Казань, Южная ул., д. 18 кв.29").Register(),
                new User("Светлана", "Кокоткина", "Ивановна", "svetlana05081967@mail.ru", "JN7TFiM!", "Россия, г. Новосибирск, Новая ул., д. 13 кв.166").Register(),
                new User("Даниил", "Каипов", "Петрович", "daniil04031970@ya.ru", "?Vd9BXKi", "Россия, г. Екатеринбург, Хуторская ул., д. 10 кв.29").Register(),
                new User("Марк", "Янышев", "Федотович", "mark.yanyshev@outlook.com", "tDKls9r*", "Россия, г. Саратов, Лесная ул., д. 3 кв.127").Register(),
                new User("Ульяна", "Чайка", "Прохоровна", "ulyana.chayka@gmail.com", "QgVeS5Qu", "Россия, г. Рязань, Парковая ул., д. 14 кв.65").Register(),
                new User("Георгий", "Суворин", "Никитьевич", "georgiy14061984@mail.ru", "NJiGPKyY", "Россия, г. Владивосток, Молодежная ул., д. 18 кв.122").Register()
            };
        }

        #endregion


    }
}

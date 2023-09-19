using Microsoft.AspNetCore.Components;
using UserAccountApp.Models;

namespace UserAccountApp.Services
{
    /// <summary>
    /// Реализует функционал по генерации моделей пользователей
    /// </summary>
    public static class NoDataBaseUsers
    {
        /// <summary>
        /// Генерирует список пользователей
        /// </summary>
        public static List<User> GenerateUsers()
        {
            List<User> users = new List<User>() {
                new User("Валентин", "Козин", "Адамович", "valentin.kozin@hotmail.com", "yUxUh5gA", "Россия, г. Казань, Южная ул., д. 18 кв.29").Register(),
                new User("Светлана", "Кокоткина", "Ивановна", "svetlana05081967@mail.ru", "JN7TFiM!", "Россия, г. Новосибирск, Новая ул., д. 13 кв.166").Register(),
                new User("Даниил", "Каипов", "Петрович", "daniil04031970@ya.ru", "?Vd9BXKi", "Россия, г. Екатеринбург, Хуторская ул., д. 10 кв.29").Register(),
                new User("Марк", "Янышев", "Федотович", "mark.yanyshev@outlook.com", "tDKls9r*", "Россия, г. Саратов, Лесная ул., д. 3 кв.127").Register(),
                new User("Ульяна", "Чайка", "Прохоровна", "ulyana.chayka@gmail.com", "QgVeS5Qu", "Россия, г. Рязань, Парковая ул., д. 14 кв.65").Register(),
                new User("Георгий", "Суворин", "Никитьевич", "georgiy14061984@mail.ru", "NJiGPKyY", "Россия, г. Владивосток, Молодежная ул., д. 18 кв.122").Register()
            };
            return users;
        }
    }
}

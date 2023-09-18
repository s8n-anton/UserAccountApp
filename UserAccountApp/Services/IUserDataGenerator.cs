using UserAccountApp.Models;

namespace UserAccountApp.Services
{
    /// <summary>
    /// Интерфейс для функционала по генерации моделей пользователей
    /// </summary>
    public interface IUserDataGenerator
    {
        public List<User> GenerateUsers();
    }
}

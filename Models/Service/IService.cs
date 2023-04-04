using Microsoft.EntityFrameworkCore;
using WebQuiz.Data;

namespace WebQuiz.Models.Service
{
    public interface IService
    {
        public Task<List<int>> GetRolesOfUsersAsync(List<Offer> offers);    //Выдает все роли пользователей, которые оставили 
                                                                            //комментарий в разделе "предлложения"
        public Task PostOfferAsync(string offerField);  //Добавляет комментарий в разделе "предложения" в разделе БД 
        public Task<List<Result>> LoadUserResultsAsync();   //Выдает все результаты текущего пользователя в странице аккаунта
        public Task ChangePasswordAsync(string newPassword, string newPasswordConfirm);  //Смена пароля текущего пользователя
        public Task<List<User>> LoadAdminPageAsync();   //Загрузка страницы главного админа со всеми пользователями
        public Task<List<User>> FindUserAsync(string searchString); //Поиск пользователей, в которых содержится searchString
        public Task<bool> NewRoleToUserAsync(int userId, int newRole); //Выдача пользователю новой роли
        public Task<List<Result>> LoadStatisticsAsync();    //Все результаты по всем пользователям
        public Task<List<Result>> LoadStatisticsAsync(
            string searchString, string firstSortParameter, string secondSortParameter);    //Все результаты учитывая поиск/сортировку
        public Task<List<UserRating>> FormUserRatingAsync(
            string searchString = null, string sortParameter = null);   //Страница с рейтингом польз-ей
        public Task<List<Country>> CountriesStatsAsync(                 //Статистика по странам
            string searchString = null, string sortParameter = null);
        public Task<List<Ruler>> RulersStatsAsync(
            string searchString = null, string sortParameter = null);   // Статистика по правителем
        public Task<string> RegisterUserAsync(string confirmPassword, User user);   //Регистрация пользователя
        public Task<string> LogInAsync(User user);  //Вход 
    }
}

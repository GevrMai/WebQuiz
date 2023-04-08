using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebQuiz.Data;

namespace WebQuiz.Models.Service
{
    public class Service : IService
    {
        private readonly UnitOfWork uow;
        public Service(DbContextOptions<AppDbContext> options)
        {
            uow = new UnitOfWork(options);
        }
        public async Task<List<int>> GetRolesOfUsersAsync(List<Offer> offers)
        {
            List<int> userRoles = new List<int>();   // получение роли для каждого предложения
            int id;
            foreach (var offer in offers)
            {
                try        // предложение оставлено пол-ем, который есть в БД
                {
                    var user = await uow.User.GetByNameAsync(offer.UserName);
                    id = user.RoleId;
                }
                catch       // предложение оставлено пользователем, который удален 
                {
                    id = 0;
                }

                userRoles.Add(id);
            }

            return userRoles;
        }
        public async Task PostOfferAsync(string offerField)
        {
            if (CurrentUser._CurrentUser != null && !string.IsNullOrEmpty(offerField))
            {
                try
                {
                    int id = 0;

                    try { id = id = await uow.Offer.MaxAsync(); }
                    catch { }
                    Offer offer = new Offer()
                    {
                        Id = id + 1,
                        UserName = CurrentUser._CurrentUser.Name,
                        OfferText = offerField.Trim(),
                        CreatedDate = DateTime.Now.ToString("G", CultureInfo.GetCultureInfo("ru")),
                        UserRole = CurrentUser._CurrentUser.RoleId,
                    };

                    await uow.Offer.CreateAsync(offer);
                    await uow.SaveAsync();
                }
                catch
                { }
            }
        }

        public async Task<List<Result>> LoadUserResultsAsync()
        {
            try
            {
                var userResults = uow.Result.GetCurrentUserResults().ToListAsync().Result;
                await AverageMetrics.CountMetricsAsync(1, userResults);
                return userResults;
            }
            catch { return new List<Result>(); }

            
        }
        public async Task ChangePasswordAsync(string newPassword, string newPasswordConfirm)
        {
            
            if (newPassword.Length >= 3 && string.Equals(newPassword, newPasswordConfirm))
            {
                newPassword = newPassword.Trim();
                newPasswordConfirm = newPasswordConfirm.Trim();
                var user = await uow.User.GetByNameAsync(CurrentUser._CurrentUser.Name);
                if (user != null)
                {
                    user.Password = PasswordHasher.HashPassword(newPassword);
                    CurrentUser._CurrentUser.Password = PasswordHasher.HashPassword(newPassword);
                }
                await uow.SaveAsync();
            }
            else
            {
                throw new Exception("Данные введены неверно");
            }
        }
        public async Task<List<User>> LoadAdminPageAsync()
        {
            if (CurrentUser._CurrentUser != null && CurrentUser._CurrentUser.RoleId == 2)       // вход выполнен и админ
            {
                try
                {
                    List<User> users = await uow.User.GetAll().ToListAsync();
                    return users;
                }
                catch
                {
                    return new List<User>();
                }
            }
            return new List<User>();
        }
        public async Task<List<User>> FindUserAsync(string searchString)
        {
            if (CurrentUser._CurrentUser != null && CurrentUser._CurrentUser.RoleId == 2)       // выполнен вход и админ
            {
                try
                {
                    if (searchString != null)
                    {
                        var users = await uow.User.SearchedUsers(searchString).ToListAsync();
                        return users;
                    }
                    else
                    {
                        var users = await uow.User.GetAll().ToListAsync();
                        return users;
                    }

                }
                catch { }
            }
            return new List<User>();    // не админ / произошла ошибка
        }
        public async Task<bool> NewRoleToUserAsync(int userId, int newRole)
        //true - успех; false - неуспех; 1- назначить модератора; 0 - снять
        {
            if (CurrentUser._CurrentUser != null && CurrentUser._CurrentUser.RoleId == 2)
            {
                try
                {
                    var user = await uow.User.GetAsync(userId);
                    user.RoleId = newRole;
                    await uow.SaveAsync();
                    return true;
                }
                catch { }
            }
            return false;
        }
        public async Task<List<Result>> LoadStatisticsAsync()
        {
            try
            {
                var statistics = uow.Result.GetAll().ToListAsync().Result;

                await AverageMetrics.CountMetricsAsync(0, statistics);

                return statistics;
            }
            catch { return new List<Result>(); }
        }

        public async Task<List<Result>> LoadStatisticsAsync(
            string searchString, string firstSortParameter, string secondSortParameter)
        {
            try
            {
                IQueryable<Result>? results;
                if (!string.IsNullOrEmpty(searchString))
                    results = uow.Result.GetSearched(searchString);
                else
                    results = uow.Result.GetAll();

                if (firstSortParameter == "user" && secondSortParameter == "time")
                    results = results.OrderBy(x => x.UserName).ThenBy(x => x.Time);

                else if (firstSortParameter == "user" && secondSortParameter == "correctAnswers")
                    results = results.OrderBy(x => x.UserName).ThenByDescending(x => x.CorrectAnswers);

                else if (firstSortParameter == "user" && secondSortParameter == "points")
                    results = results.OrderBy(x => x.UserName).ThenByDescending(x => x.Points);

                else if (firstSortParameter == "correctAnswers" && secondSortParameter == "user")
                    results = results.OrderByDescending(x => x.CorrectAnswers).ThenBy(x => x.UserName);

                else if (firstSortParameter == "correctAnswers" && secondSortParameter == "points")
                    results = results.OrderByDescending(x => x.CorrectAnswers).ThenByDescending(x => x.Points);

                else if (firstSortParameter == "correctAnswers" && secondSortParameter == "time")
                    results = results.OrderByDescending(x => x.CorrectAnswers).ThenBy(x => x.Time);

                else if (firstSortParameter == "time" && secondSortParameter == "correctAnswers")
                    results = results.OrderBy(x => x.Time).ThenByDescending(x => x.CorrectAnswers);

                else if (firstSortParameter == "time" && secondSortParameter == "points")
                    results = results.OrderBy(x => x.Time).ThenByDescending(x => x.Points);

                else if (firstSortParameter == "time" && secondSortParameter == "user")
                    results = results.OrderBy(x => x.Time).ThenBy(x => x.UserName);

                else if (firstSortParameter == "points" && secondSortParameter == "time")
                    results = results.OrderByDescending(x => x.Points).ThenBy(x => x.Time);

                else if (firstSortParameter == "points" && secondSortParameter == "correctAnswers")
                    results = results.OrderByDescending(x => x.Points).ThenByDescending(x => x.CorrectAnswers);

                else if (firstSortParameter == "points" && secondSortParameter == "user")
                    results = results.OrderByDescending(x => x.Points).ThenBy(x => x.UserName);

                else if (firstSortParameter == "time")
                    results = results.OrderBy(x => x.Time);

                else if (firstSortParameter == "user")
                    results = results.OrderBy(x => x.UserName);

                else if (firstSortParameter == "correctAnswers")
                    results = results.OrderByDescending(x => x.CorrectAnswers);

                else if (firstSortParameter == "points")
                    results = results.OrderByDescending(x => x.Points);

                var listResults = await results.ToListAsync();

                await AverageMetrics.CountMetricsAsync(0, listResults);

                return listResults;
            }
            catch { return new List<Result>(); }
        }
        public List<UserRating> FormUserRating(string searchString = null, string sortParameter = null)
        {
            try
            {
                List<UserRating> usersRating = new();
                List<string> addedUsers = new();

                var results = uow.Result.GetAll();
                foreach (var result in results)
                {
                    var splittedTime = result.Time.Split(":");  // разбитая строка со временем. Было "1:6", стало ["1", "6"]. Далее переведем это в секунды
                    if (!addedUsers.Contains(result.UserName))      // список добавленных поль-ей не содержит автора результата
                    {
                        addedUsers.Add(result.UserName);            // добавляем. Далее когда встретим этого поль-ля будем обновлять список usersRating

                        usersRating.Add(                            // добавляем польз-ля в рейтинг
                            new UserRating()
                            {
                                UserName = result.UserName,
                                TotalCorrectAnswers = result.CorrectAnswers,
                                TotalPoints = result.Points,
                                TotalSeconds = Convert.ToInt32(splittedTime[0]) * 60 + Convert.ToInt32(splittedTime[1]),
                                TotalTries = 1,

                            });
                    }
                    else
                    {
                        usersRating.Find(x => x.UserName == result.UserName).TotalCorrectAnswers += result.CorrectAnswers;
                        usersRating.Find(x => x.UserName == result.UserName).TotalTries += 1;
                        usersRating.Find(x => x.UserName == result.UserName).TotalPoints += result.Points;
                        usersRating.Find(x => x.UserName == result.UserName).TotalSeconds += Convert.ToInt32(splittedTime[0]) * 60
                                                                                                    + Convert.ToInt32(splittedTime[1]);
                    }
                }

                usersRating = usersRating.OrderByDescending(x => x.TotalPoints).ToList();

                for (int i = 0; i < usersRating.Count; i++)     // выставление места в рейтинге
                {
                    usersRating[i].PositionInRating = i + 1;
                    usersRating[i].TotalPoints = Math.Round(usersRating[i].TotalPoints, 2);
                }

                if (searchString != null)
                    usersRating = usersRating.Where(x => x.UserName.ToLower().Contains(searchString.ToLower().Trim())).ToList();

                if (sortParameter == "position")
                    usersRating = usersRating.OrderBy(x => x.PositionInRating).ToList();
                else if (sortParameter == "user")
                    usersRating = usersRating.OrderBy(x => x.UserName).ToList();
                else if (sortParameter == "points")
                    usersRating = usersRating.OrderByDescending(x => x.TotalPoints).ToList();
                else if (sortParameter == "correctAnswers")
                    usersRating = usersRating.OrderByDescending(x => x.TotalCorrectAnswers).ToList();
                else if (sortParameter == "time")
                    usersRating = usersRating.OrderByDescending(x => x.TotalSeconds).ToList();

                return usersRating;

            }
            catch
            {
                return new List<UserRating>();
            }
        }
        public async Task<List<Country>> CountriesStatsAsync(string searchString = null, string sortParameter = null)
        {
            try
            {
                IQueryable<Country> countries;
                if (sortParameter == "name")
                    countries = uow.Country.GetAllOrdered();
                else if (sortParameter == "percent")
                {
                    Dictionary<string, double> countryPercent = new Dictionary<string, double>(); // страна - процент верных ответов
                    foreach (var country in await uow.Country.GetAll().ToListAsync())
                    {
                        if (country.TimesDisplayed == 0)            // избегаем деления на 0, вручную ставим 0
                            countryPercent.Add(country.RightAnswers, 0.0);
                        else                                        // деления на 0 не будет, считаем отношение
                            countryPercent.Add(country.RightAnswers, country.TimesGuessed / (double)country.TimesDisplayed);
                    }
                    var sortedPercents = countryPercent.OrderByDescending(x => x.Value);
                    countryPercent = null;

                    List<Country> sortedCountries = new List<Country>();    // этот список вернем в контроллер
                    foreach (var country in sortedPercents)                 // прохождение и добавление отсортированных данных
                    {
                        sortedCountries.Add(await uow.Country.GetAsync(country.Key));
                    }

                    if (searchString != null)
                        sortedCountries = sortedCountries.Select(x => x)
                            .Where(x => x.RightAnswers.Contains(searchString.Trim().ToLower())).ToList();

                    return sortedCountries;

                }
                else
                    countries = uow.Country.GetAllOrdered(sortParameter);

                
                if (searchString != null)
                    countries = countries.Where(x => x.RightAnswers.Contains(searchString.Trim().ToLower()));
                
                var countriesList = await countries.ToListAsync();
                return countriesList;
            }
            catch
            {
                return new List<Country>();
            }
        }
        public async Task<List<Ruler>> RulersStatsAsync(string searchString = null, string sortParameter = null)
        {
            try
            {
                IQueryable<Ruler> rulers;
                if (sortParameter == "name")
                    rulers = uow.Ruler.GetAllOrdered();

                else if (sortParameter == "percent")
                {
                    Dictionary<string, double> rulersPercent = new Dictionary<string, double>(); // правитель - процент верных ответов
                    foreach (var ruler in uow.Ruler.GetAll())
                    {
                        if (ruler.TimesDisplayed == 0)            // избегаем деления на 0, вручную ставим 0
                            rulersPercent.Add(ruler.RightAnswers, 0.0);
                        else                                        // деления на 0 не будет, считаем отношение
                            rulersPercent.Add(ruler.RightAnswers, (double)ruler.TimesGuessed / (double)ruler.TimesDisplayed);
                    }
                    var sortedPercents = rulersPercent.OrderByDescending(x => x.Value);
                    rulersPercent = null;

                    List<Ruler> sortedRulers = new List<Ruler>();    // этот список вернем в контроллер
                    foreach (var ruler in sortedPercents)                 // прохождение и добавление отсортированных данных
                    {
                        sortedRulers.Add(await uow.Ruler.GetAsync(ruler.Key));
                    }

                    if (searchString != null)
                        sortedRulers = sortedRulers.Select(x => x).Where(x =>
                                                x.RightAnswers.Contains(searchString.Trim().ToLower())).ToList();

                    return sortedRulers;

                }
                else
                    rulers = uow.Ruler.GetAllOrdered(searchString);

                if (searchString != null)
                    rulers = rulers.Select(x => x).Where(x => x.RightAnswers.Contains(searchString.Trim().ToLower()));

                var rulersList = await rulers.ToListAsync();
                return rulersList;
            }
            catch
            {
                return new List<Ruler>();
            }
        }
        public async Task<string> RegisterUserAsync(string confirmPassword, User user)
        {
            try
            {
                IQueryable<User>? users = uow.User.GetAllByName(user.Name);
                if (users.ToListAsync().Result.Count == 0)
                {
                    User UserToAdd = new User()
                    {
                        Name = user.Name,
                        Password = PasswordHasher.HashPassword(user.Password),
                        RoleId = 0,                                          // по умолчанию роль 0 - default user
                    };

                    await uow.User.CreateAsync(UserToAdd);
                    await uow.SaveAsync();
                    return "Пользователь успешно добавлен";
                }
                else
                {
                    return "Пользователь с таким именем уже существует";
                }
            }
            catch
            {
                return "Произошла ошибка!";
            }
        }
        public async Task<string> LogInAsync(User user)
        {
            try
            {
                if (await uow.User.AnyByNameAsync(user.Name))
                {
                    
                    if (uow.User.GetByNameAsync(user.Name).Result.Password == PasswordHasher.HashPassword(user.Password))
                    {
                        CurrentUser._CurrentUser = await uow.User.GetByNameAsync(user.Name);
                        return null;
                    }
                    else
                    {
                        return "Пароль неверный";
                    }
                }
                else
                {
                    return "Пользователь с таким именем не найден";
                }
            }
            catch
            {
                return "Произошла ошибка!";
            }
        }
        public async Task CheckAnswersAsync()
        {
            try
            {
                Country country = null; Ruler ruler = null;

                for (int i = 0; i < 10; i++)
                {
                    if (QuizState.Type == QuizState.QuizType.Rulers)
                    { 
                        ruler = await uow.Ruler.GetAsync(QuizState._UserAnswers[i].RightAnswer);    // берем правителя из контекста для обновления данных
                        ruler.TimesDisplayed += 1;
                    }
                    else
                    {
                        country = await uow.Country.GetAsync(QuizState._UserAnswers[i].RightAnswer); // берем страну из контекста для обновления данных
                        country.TimesDisplayed += 1;
                    }

                    var rightAnswers = QuizState._UserAnswers[i].RightAnswer.Split("/");      // разделяем ответы
                    if (rightAnswers.Contains(QuizState._UserAnswers[i].UserAnswer))
                    {
                        QuizState.CorrectAnswers += 1;
                        QuizState._UserAnswers[i].IsCorrect = true;

                        if (QuizState.Type == QuizState.QuizType.Rulers)
                            ruler.TimesGuessed += 1;
                        else
                            country.TimesGuessed += 1;
                    }

                }

                await uow.SaveAsync();
                await SaveResultAsync();
            }
            catch { }

        }
        public async Task SaveResultAsync()
        {
            try
            {
                int id = 0;
                try
                {
                    id = uow.Result.MaxIdAsync().Result + 1;
                }
                catch { }

                double koefficient;
                int seconds = QuizState.TotalTime.Minutes * 60 + QuizState.TotalTime.Seconds;
                if (seconds <= 20) koefficient = 1.25;
                else if (seconds <= 35) koefficient = 1.15;
                else if (seconds <= 50) koefficient = 1.1;
                else if (seconds <= 65) koefficient = 1.05;
                else if (seconds <= 80) koefficient = 1;
                else if (seconds <= 95) koefficient = 0.95;
                else if (seconds <= 110) koefficient = 0.9;
                else if (seconds <= 130) koefficient = 0.85;
                else koefficient = 0.8;

                string time;
                if (QuizState.TotalTime.Seconds < 10)
                    time = QuizState.TotalTime.Minutes.ToString() + ":0" + QuizState.TotalTime.Seconds.ToString();
                else
                    time = QuizState.TotalTime.Minutes.ToString() + ":" + QuizState.TotalTime.Seconds.ToString();

                Result Result = new Result()
                {
                    Id = id,
                    UserName = CurrentUser._CurrentUser.Name,
                    CorrectAnswers = QuizState.CorrectAnswers,
                    Time = time,
                    Points = Math.Round(QuizState.CorrectAnswers * koefficient, 2),
                };

                QuizState.Points = Result.Points;

                await uow.Result.CreateAsync(Result);
                await uow.SaveAsync();

                QuizState.ResultIsShown = true;
            }
            catch { }
        }
    }

}

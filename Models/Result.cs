using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebQuiz.Data;

namespace WebQuiz.Models
{
    public class Result     // таблица в БД, которая хранит отдельно каждый из ответов пользователей
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Range(0, 10)]
        public int CorrectAnswers { get; set; }

        [Required]
        public string Time { get; set; }

        public double Points { get; set; }

        public async static Task CheckAnswers(DbContextOptions<AppDbContext> options)
        {
            try
            {
                AppDbContext _context = new AppDbContext(options);

                Country country = null; Ruler ruler = null;

                for (int i = 0; i < 10; i++)
                {
                    if (QuizState.Type == QuizState.QuizType.Rulers)
                    {
                        ruler = await _context.rulers.FirstAsync(x => x.RightAnswers == QuizState._UserAnswers[i].RightAnswer);// берем правителя из контекста для обновления данных
                        ruler.TimesDisplayed += 1;
                    }
                    else
                    {
                        country = await _context.countries.FirstAsync(x => x.RightAnswers == QuizState._UserAnswers[i].RightAnswer);// берем страну из контекста для обновления данных
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

                _context.SaveChanges();
                SaveResult(options);
            }
            catch { }
            
        }

        public static void SaveResult(DbContextOptions<AppDbContext> options)
        {
            using (var _context = new AppDbContext(options))
            {
                try
                {
                    int id = 0;
                    try
                    {
                        id = _context.results.Max(x => x.Id) + 1;
                    }
                    catch { }

                    double koefficient;
                    int seconds = QuizState.TotalTime.Minutes * 60 + QuizState.TotalTime.Seconds;
                    if(seconds <= 20) koefficient = 1.25;
                    else if(seconds <= 35) koefficient = 1.15;
                    else if(seconds <= 50) koefficient = 1.1;
                    else if(seconds <= 65) koefficient = 1.05;
                    else if(seconds <= 80) koefficient = 1;
                    else if(seconds <= 95) koefficient = 0.95;
                    else if(seconds <= 110) koefficient = 0.9;
                    else if(seconds <= 130) koefficient = 0.85;
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

                    _context.results.Add(Result);
                    _context.SaveChanges();

                    QuizState.ResultIsShown = true;
                }
                catch { }
            }
        }

        public async static Task<List<Result>> GetTop10ResultsAsync(DbContextOptions<AppDbContext> options)
        {
            AppDbContext _context = new AppDbContext(options);

            var topResults = await (from result in _context.results
                                join user in _context.users
                                on result.UserName equals user.Name
                                select result).OrderByDescending(x => x.CorrectAnswers).ThenBy(x => x.Time).Take(10).ToListAsync();

            _context.Dispose();
            return topResults;
        }
    }
}

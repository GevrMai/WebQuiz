using Microsoft.EntityFrameworkCore;
using WebQuiz.Data;

namespace WebQuiz.Models
{
    public class QuizState
    {
        public static QuizType Type { get; set; }
        public static HashSet<IAbstraction> Questions { get; set; }
        public static HashSet<string> PhotoUrls { get; set; }
        public static int CorrectAnswers { get; set; }
        public static double Points { get; set; }
        public static int CurrentQuestionIndex { get; set; }

        public static DateTime StartTime{ get; set; }
        public static TimeSpan TotalTime{ get; set; }
        public static List<Answer> _UserAnswers;
        public static bool ResultIsShown { get; set; }

        public static void StartQuiz(DbContextOptions<AppDbContext> options, int type)
        {
            using (var _context = new AppDbContext(options))
            {
                try
                {
                    Random rnd = new Random();
                    Questions = new HashSet<IAbstraction>();
                    PhotoUrls = new HashSet<string>();
                    _UserAnswers = new List<Answer>();
                    for (int i = 0; i < 10; i++) { _UserAnswers.Add(new Answer()); }

                    switch (type)
                    {
                        case 1:                                     // выбраны страны
                            while (Questions.Count < 10)
                            {
                                int id = rnd.Next(1, _context.countries.Count());
                                var question = _context.countries.Where(x => x.Id == id).ToList().First();
                                Questions.Add(question);
                                if (Type == QuizType.Flags)
                                    PhotoUrls.Add(question.FlagUrl);
                                else
                                    PhotoUrls.Add(question.BorderUrl);
                            }
                            break;

                        case 2:                                     // выбраны правители
                            while (Questions.Count < 10)
                            {
                                int id = rnd.Next(1, _context.rulers.Count());
                                var question = _context.rulers.Select(x => x).Where(x => x.Id == id).ToList().First();
                                Questions.Add(question);
                                PhotoUrls.Add(question.PhotoUrl);
                            }
                            break;

                        default:
                            break;

                    }
                    ResultIsShown = false;
                    StartTime = DateTime.Now;
                    CurrentQuestionIndex = 0;
                    CorrectAnswers = 0;
                }
                catch { }
            }
        }

        public static bool QuizGoesOn(string userAnswer)
        {
            if (Type == QuizType.Flags || Type == QuizType.Borders)
            {
                Country country;
                country = (Country)Questions.ToList()[CurrentQuestionIndex];
                _UserAnswers[CurrentQuestionIndex].RightAnswer = country.RightAnswers;
            }
            else if (Type == QuizType.Rulers)
            {
                Ruler ruler = new Ruler();
                ruler = (Ruler)Questions.ToList()[CurrentQuestionIndex];
                _UserAnswers[CurrentQuestionIndex].RightAnswer = ruler.RightAnswers;
            }

            userAnswer = userAnswer.Trim().ToLower();
            _UserAnswers[CurrentQuestionIndex].UserAnswer = userAnswer;

            if (CurrentQuestionIndex != 9)      // не достигнут 10ый вопрос
            {
                CurrentQuestionIndex++;
                return true;                    // продолжение
            }
            else
            {
                TotalTime = (DateTime.Now - StartTime);     // "таймер" сколько времени прошло с начала викторины
                CurrentQuestionIndex = 0;                   // обнуляем индекс текущего вопроса
                return false;                               // завершение, обрабатывается в контроллере Quiz
            }
        }
        public enum QuizType      // типы викторин
        {
            Flags = 1,
            Borders = 2,
            Rulers = 3,
        }

        public class Answer      // отслеживание ответов и их правильности. Показывается после пройденной викторины
        {
            public string UserAnswer { get; set; }
            public string RightAnswer { get; set; }
            public bool IsCorrect { get; set; }
        }
    }
}






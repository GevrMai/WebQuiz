using System.ComponentModel.DataAnnotations;

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
    }
}

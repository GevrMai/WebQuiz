using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebQuiz.Data;

namespace WebQuiz.Models
{
    public class Ruler : IAbstraction
    {
        public int Id { get; set; }

        [Required]
        public string RightAnswers { get; set; }

        [Required]
        public string PhotoUrl { get; set; }
        public int TimesDisplayed { get; set; }
        public int TimesGuessed { get; set; }
    }
}

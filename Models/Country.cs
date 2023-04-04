using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebQuiz.Data;

namespace WebQuiz.Models
{
    public class Country : IAbstraction
    {
        public int Id { get; set; }

        [Required]
        public string RightAnswers { get; set; }

        [Required]
        public string FlagUrl { get; set; }

        [Required]
        public string BorderUrl { get; set; }
        public int TimesDisplayed { get; set; }
        public int TimesGuessed { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using WebQuiz.Data;

namespace WebQuiz.Models
{
    public class UserRating
    {
        public int PositionInRating { get;set; }
        [Required]
        public string UserName { get; set; }
        public int TotalCorrectAnswers { get; set; }
        public double TotalPoints { get; set; }
        public int TotalSeconds { get; set; }
        public int TotalTries { get; set; }

        
    }
    
}

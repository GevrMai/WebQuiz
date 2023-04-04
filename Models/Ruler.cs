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

        
        public async static Task<List<Ruler>> RulersStatsAsync(DbContextOptions<AppDbContext> options)
        {
            using (AppDbContext _context = new AppDbContext(options))
            {
                try
                {
                    var rulers = await _context.rulers.Select(x => x)
                        .OrderByDescending(x => x.TimesDisplayed)
                        .ThenByDescending(x => x.TimesGuessed).ToListAsync();

                    return rulers;
                }
                catch
                {
                    return new List<Ruler>();
                }
            }
        }
    }
}

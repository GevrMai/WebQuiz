using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using WebQuiz.Data;

namespace WebQuiz.Models
{
    public class Offer
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(1000)]
        public string OfferText { get; set; }

        [Required]
        public string CreatedDate { get; set; }
        public int UserRole { get; set; }

        public async static Task<List<Offer>> LoadOffersAsync(DbContextOptions<AppDbContext> options)
        {
            using (AppDbContext _context = new AppDbContext(options))
            {
                var offers = await _context.offers.OrderByDescending(x => x.CreatedDate).ToListAsync();
                return offers;
            }
        }
        
        public async static Task PostOfferAsync(DbContextOptions<AppDbContext> options, string offerField)
        {
            //await Task.Run(() => PostOffer(options, offerField));
        }
        
    }
}

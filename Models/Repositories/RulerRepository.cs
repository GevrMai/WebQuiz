using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Runtime.InteropServices;
using WebQuiz.Data;
using WebQuiz.Models.RepositoryIntefraces;

namespace WebQuiz.Models.Repositories
{
    public class RulerRepository : IRulerRepository
    {
        private readonly AppDbContext _context;
        public RulerRepository(DbContextOptions<AppDbContext> options)
        {
            _context = new AppDbContext(options);
        }

        public Task<bool> CreateAsync(Ruler entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Ruler>> GetAllAsync()
        {
            return await _context.rulers.OrderBy(x => x.RightAnswers).ToListAsync();
        }

        public async Task<List<Ruler>> GetAllOrdered()
        {
            return await _context.rulers.OrderBy(x => x.RightAnswers).ToListAsync();
        }

        public async Task<List<Ruler>> GetAllOrdered(string typeOfSort)
        {
            if (typeOfSort == "displayed")
                return await _context.rulers.OrderByDescending(x => x.TimesDisplayed).ToListAsync();
            else if (typeOfSort == "guessed")
                return await _context.rulers.OrderByDescending(x => x.TimesGuessed).ToListAsync();
            else
                return await _context.rulers.ToListAsync();

            throw new NotImplementedException();
        }

        public async Task<Ruler> GetAsync(int id)
        {
            return await _context.rulers.FirstAsync(x => x.Id == id);
        }

        public async Task<Ruler> GetAsync(string key)
        {
            return await _context.rulers.FirstAsync(x => x.RightAnswers == key);
        }
        public async Task<int> Count()
        {
            return await _context.rulers.CountAsync();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}

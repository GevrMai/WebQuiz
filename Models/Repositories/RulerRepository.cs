using Microsoft.EntityFrameworkCore;
using System.Data;
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

        public IQueryable<Ruler> GetAll()
        {
            return _context.rulers.OrderBy(x => x.RightAnswers);
        }

        public IQueryable<Ruler> GetAllOrdered()
        {
            return  _context.rulers.OrderBy(x => x.RightAnswers);
        }

        public IQueryable<Ruler> GetAllOrdered(string typeOfSort)
        {
            if (typeOfSort == "displayed")
                return  _context.rulers.OrderByDescending(x => x.TimesDisplayed);
            else if (typeOfSort == "guessed")
                return  _context.rulers.OrderByDescending(x => x.TimesGuessed);
            else
                return  _context.rulers;
        }

        public async Task<Ruler> GetAsync(int id)
        {
            return await _context.rulers.FirstAsync(x => x.Id == id);
        }

        public async Task<Ruler> GetAsync(string key)
        {
            return await _context.rulers.FirstAsync(x => x.RightAnswers == key);
        }
        public async Task<int> CountAsync()
        {
            return await _context.rulers.CountAsync();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}

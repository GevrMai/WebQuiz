using Microsoft.EntityFrameworkCore;
using WebQuiz.Data;
using WebQuiz.Models.RepositoryIntefraces;

namespace WebQuiz.Models.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;
        public CountryRepository(DbContextOptions<AppDbContext> options)
        {
            _context = new AppDbContext(options);
        }
        public Task<bool> CreateAsync(Country entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Country> GetAll()
        {
            return _context.countries.OrderBy(x => x.RightAnswers);
        }

        public IQueryable<Country> GetAllOrdered()
        {
            return _context.countries.OrderBy(x => x.RightAnswers); 
        }

        public IQueryable<Country> GetAllOrdered(string typeOfSort)
        {
            if (typeOfSort == "displayed")
                return  _context.countries.OrderByDescending(x => x.TimesDisplayed);
            else if (typeOfSort == "guessed")
                return  _context.countries.OrderByDescending(x => x.TimesGuessed);
            else
                return _context.countries.OrderBy(x => x.RightAnswers);
        }

        public async Task<Country> GetAsync(string key)
        {
            return await _context.countries.FirstAsync(x => x.RightAnswers == key);
        }
        public async Task<int> Count()
        {
            return await _context.countries.CountAsync();
        }

        public async Task<Country> GetAsync(int id)
        {
            return await _context.countries.FirstAsync(x => x.Id == id);
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}

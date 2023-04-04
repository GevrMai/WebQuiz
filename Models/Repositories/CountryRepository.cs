using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System.Diagnostics.Metrics;
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

        public async Task<List<Country>> GetAllAsync()
        {
            return await _context.countries.OrderBy(x => x.RightAnswers).ToListAsync();
        }

        public async Task<List<Country>> GetAllOrdered()
        {
            return await _context.countries.OrderBy(x => x.RightAnswers).ToListAsync(); 
        }

        public async Task<List<Country>> GetAllOrdered(string typeOfSort)
        {
            if (typeOfSort == "displayed")
                return await _context.countries.OrderByDescending(x => x.TimesDisplayed).ToListAsync();
            else if (typeOfSort == "guessed")
                return await _context.countries.OrderByDescending(x => x.TimesGuessed).ToListAsync();
            else
                return await _context.countries.ToListAsync();
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

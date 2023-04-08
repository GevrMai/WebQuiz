using Microsoft.EntityFrameworkCore;
using WebQuiz.Data;
using WebQuiz.Models.RepositoryIntefraces;

namespace WebQuiz.Models.Repositories
{
    public class ResultRepository : IResultRepository
    {
        private readonly AppDbContext _context;
        public ResultRepository(DbContextOptions<AppDbContext> options)
        {
            _context = new AppDbContext(options);
        }


        public async Task<bool> CreateAsync(Result entity)
        {
            if (entity != null)
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Result> GetAll()
        {
            return _context.results.OrderByDescending(x => x.Points)
                    .ThenByDescending(x => x.CorrectAnswers);
        }

        public Task<Result> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Result> GetCurrentUserResults()
        {
            return _context.results.Where(x => x.UserName == CurrentUser._CurrentUser.Name)
                                       .OrderByDescending(x => x.Points).ThenBy(x => x.Time);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<List<Result>> GetTop10ResultsAsync()
        {
            var topResults = await (from result in _context.results
                                    join user in _context.users
                                    on result.UserName equals user.Name
                                    select result).OrderByDescending(x => x.CorrectAnswers).ThenBy(x => x.Time).Take(10).ToListAsync();

            return topResults;
        }

        public IQueryable<Result> GetSearched(string searchString)
        {
            return _context.results.Where(x => x.UserName.ToLower().Contains(searchString.ToLower()));
        }

        public async Task<int> MaxIdAsync()
        {
            return await _context.results.MaxAsync(x => x.Id);
        }
    }
}

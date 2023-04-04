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


        public Task<bool> CreateAsync(Result entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Result>> GetAllAsync()
        {
            return await _context.results.OrderByDescending(x => x.Points)
                    .ThenByDescending(x => x.CorrectAnswers).ToListAsync();
        }

        public Task<Result> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Result>> GetCurrentUserResultsAsync()
        {
            return await _context.results.Where(x => x.UserName == CurrentUser._CurrentUser.Name)
                                       .OrderByDescending(x => x.Points).ThenBy(x => x.Time).ToListAsync();
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

        public async Task<List<Result>> GetSearched(string searchString)
        {
            return await _context.results.Where(x => x.UserName.ToLower().Contains(searchString.ToLower())).ToListAsync();
        }
    }
}

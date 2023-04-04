using Microsoft.EntityFrameworkCore;
using WebQuiz.Data;

namespace WebQuiz.Models.RepositoryIntefraces
{
    public interface IResultRepository : IBaseRepository<Result>
    {
        Task<List<Result>> GetCurrentUserResultsAsync();
        Task<List<Result>> GetTop10ResultsAsync();
        Task<List<Result>> GetSearched(string searchString);
    }
}

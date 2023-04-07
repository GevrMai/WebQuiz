namespace WebQuiz.Models.RepositoryIntefraces
{
    public interface IResultRepository : IBaseRepository<Result>
    {
        IQueryable<Result> GetCurrentUserResults();
        Task<List<Result>> GetTop10ResultsAsync();
        IQueryable<Result> GetSearched(string searchString);
    }
}

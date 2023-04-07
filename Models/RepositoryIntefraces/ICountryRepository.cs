namespace WebQuiz.Models.RepositoryIntefraces
{
    public interface ICountryRepository : IBaseRepository<Country>
    {
        IQueryable<Country> GetAllOrdered();
        IQueryable<Country> GetAllOrdered(string typeOfSort);
        Task<Country> GetAsync(string key);
        Task<int> Count();
    }
}

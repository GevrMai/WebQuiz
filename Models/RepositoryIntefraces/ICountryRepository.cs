namespace WebQuiz.Models.RepositoryIntefraces
{
    public interface ICountryRepository : IBaseRepository<Country>
    {
        Task<List<Country>> GetAllOrdered();
        Task<List<Country>> GetAllOrdered(string typeOfSort);
        Task<Country> GetAsync(string key);
        Task<int> Count();
    }
}

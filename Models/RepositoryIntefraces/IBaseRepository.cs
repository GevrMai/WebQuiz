namespace WebQuiz.Models.RepositoryIntefraces
{
    public interface IBaseRepository<T>
    {
        Task<bool> CreateAsync( T entity);

        Task<T> GetAsync(int id);

        Task<List<T>> GetAllAsync();

        Task<bool> DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}

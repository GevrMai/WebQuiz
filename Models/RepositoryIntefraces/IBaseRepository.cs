namespace WebQuiz.Models.RepositoryIntefraces
{
    public interface IBaseRepository<T>
    {
        Task<bool> CreateAsync( T entity);

        Task<T> GetAsync(int id);

        IQueryable<T> GetAll();

        Task<bool> DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}

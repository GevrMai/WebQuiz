namespace WebQuiz.Models.RepositoryIntefraces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByNameAsync(string name);
        IQueryable<User> SearchedUsers(string searchString);
        IQueryable<User> GetAllByName(string name);
        Task<bool> AnyByNameAsync(string name);
    }
}

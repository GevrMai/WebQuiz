namespace WebQuiz.Models.RepositoryIntefraces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetByNameAsync(string name);
        Task<List<User>> SearchUsers(string searchString);
        Task<List<User>> GetAllByNameAsync(string name);
        Task<bool> AnyByName(string name);
    }
}

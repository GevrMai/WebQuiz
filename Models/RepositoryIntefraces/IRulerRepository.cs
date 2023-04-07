namespace WebQuiz.Models.RepositoryIntefraces
{
    public interface IRulerRepository : IBaseRepository<Ruler>
    {
        //Task<List<Ruler>> GetAllOrdered();
        //Task<List<Ruler>> GetAllOrdered(string typeOfSort);
        //Task<Ruler> GetAsync(string key);
        //Task<int> Count();

        IQueryable<Ruler> GetAllOrdered();
        IQueryable<Ruler> GetAllOrdered(string typeOfSort);
        Task<Ruler> GetAsync(string key);
        Task<int> CountAsync();
    }
}

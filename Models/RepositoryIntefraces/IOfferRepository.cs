namespace WebQuiz.Models.RepositoryIntefraces
{
    public interface IOfferRepository : IBaseRepository<Offer>
    {
        Task<Offer> GetByNameAsync(string name);
        Task<int> MaxAsync();
    }
}

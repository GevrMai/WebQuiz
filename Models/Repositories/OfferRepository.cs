using Microsoft.EntityFrameworkCore;
using WebQuiz.Data;
using WebQuiz.Models.RepositoryIntefraces;

namespace WebQuiz.Models.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly AppDbContext _context;
        public OfferRepository(DbContextOptions<AppDbContext> options)
        {
            _context = new AppDbContext(options);
        }

        public async Task<bool> CreateAsync(Offer entity)
        {
            if(entity != null)
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var offerToDelete = await _context.offers.FirstAsync(x => x.Id == id);

                _context.offers.Attach(offerToDelete);
                _context.offers.Remove(offerToDelete);

                _ = _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Offer> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Offer>> GetAllAsync()
        {
            return await _context.offers.ToListAsync();
        }

        public async Task<Offer> GetByNameAsync(string name)
        {
            return await _context.offers.FirstAsync(x => x.UserName == name);
        }

        public async Task<int> Max()
        {
            return await _context.offers.MaxAsync(x => x.Id);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

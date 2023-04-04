using Microsoft.EntityFrameworkCore;
using WebQuiz.Data;
using WebQuiz.Models.RepositoryIntefraces;

namespace WebQuiz.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(DbContextOptions<AppDbContext> options)
        {
            _context = new AppDbContext(options);
        }


        public async Task<bool> CreateAsync(User entity)
        {
            if (entity != null)
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.users.OrderByDescending(x => x.RoleId).ToListAsync();
        }

        public async Task<List<User>> GetAllByNameAsync(string name)
        {
            return await _context.users.Where(x => x.Name == name).ToListAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            return await _context.users.FirstAsync(x => x.Id == id);
        }

        public async Task<User> GetByNameAsync(string name)
        {
            return await _context.users.FirstAsync(x => x.Name == name);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> SearchUsers(string searchString)
        {
            return await _context.users.Where(x => x.Name.ToLower().Contains(searchString.Trim().ToLower()))
                                                    .OrderByDescending(x => x.RoleId).ToListAsync();
        }
        public async Task<bool> AnyByName(string toFind)
        {
            return await _context.users.AnyAsync(x => x.Name == toFind);
        }
    }
}

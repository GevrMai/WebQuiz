using Microsoft.EntityFrameworkCore;
using WebQuiz.Data;
using WebQuiz.Models.Repositories;

namespace WebQuiz.Models.Service
{
    public class UnitOfWork : IDisposable
    {
        public UnitOfWork(DbContextOptions<AppDbContext> options)
        {
            _context = new AppDbContext(options);
            this.options = options;
        }
        DbContextOptions<AppDbContext> options;
        private readonly AppDbContext _context;
        private CountryRepository? countryRepository;
        private OfferRepository? offerRepository;
        private ResultRepository? resultRepository;
        private RulerRepository? rulerRepository;
        private UserRepository? userRepository;

        private static readonly object lockThread = new object();
        public CountryRepository Country
        {
            get
            {
                lock(lockThread)
                {
                    if (countryRepository == null)
                        countryRepository = new CountryRepository(options);
                    return countryRepository;
                }
            }
        }

        public OfferRepository Offer
        {
            get
            {
                lock (lockThread)
                {
                    if (offerRepository == null)
                        offerRepository = new OfferRepository(options);
                    return offerRepository;
                }
            }
        }
        public ResultRepository Result
        {
            get
            {
                lock (lockThread)
                {
                    if (resultRepository == null)
                        resultRepository = new ResultRepository(options);
                    return resultRepository;
                }
            }
        }
        public RulerRepository Ruler
        {
            get
            {
                lock (lockThread)
                {
                    if (rulerRepository == null)
                        rulerRepository = new RulerRepository(options);
                    return rulerRepository;
                }
            }
        }
        public UserRepository User
        {
            get
            {
                lock (lockThread)
                {
                    if (userRepository == null)
                        userRepository = new UserRepository(options);
                    return userRepository;
                }
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

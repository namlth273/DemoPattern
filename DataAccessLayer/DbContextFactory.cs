using EntityFramework.DbContextScope.Interfaces;

namespace DataAccessLayer
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly ApplicationDbContextOptions _options;

        public DbContextFactory(ApplicationDbContextOptions options)
        {
            _options = options;
        }

        public TDbContext CreateDbContext<TDbContext>() where TDbContext : class, IDbContext
        {
            return new AppDbContext(_options) as TDbContext;
        }
    }
}

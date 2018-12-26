using Autofac;
using EntityFramework.DbContextScope.Interfaces;

namespace DataAccessLayer
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly ILifetimeScope _scope;

        public DbContextFactory(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public TDbContext CreateDbContext<TDbContext>() where TDbContext : class, IDbContext
        {
            return _scope.Resolve<AppDbContext>() as TDbContext;
        }
    }
}

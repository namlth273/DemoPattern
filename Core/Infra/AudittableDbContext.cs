using Core.Common;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Core.Infra
{
    public class AuditDbContext : DbContext, IDbContext
    {
        protected IUserService UserService;
        private const string UnknownUser = "unknown";

        public AuditDbContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified));

            var identityName = UserService?.CurrentUser;

            foreach (var entry in modifiedEntries)
            {
                var entity = entry.Entity as EfBaseModel;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedBy = identityName ?? UnknownUser;
                }

                entity.ModifiedBy = identityName ?? UnknownUser;
            }

            return base.SaveChanges();
        }
    }
}

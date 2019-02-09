using Core.Common;
using Core.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;

namespace DataAccessLayer
{
    public class ApplicationDbContextOptions
    {
        public readonly DbContextOptions<AppDbContext> Options;
        public readonly IUserService UserService;

        public ApplicationDbContextOptions(DbContextOptions<AppDbContext> options, IUserService userService)
        {
            Options = options;
            UserService = userService;
        }
    }

    public partial class AppDbContext : AuditDbContext
    {
        public AppDbContext(ApplicationDbContextOptions options) : base(options.Options, options.UserService)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new BarConfiguration());
            var assemblyWithConfigurations = GetType().Assembly; //get whatever assembly you want
            modelBuilder.ApplyConfigurationsFromAssembly(assemblyWithConfigurations);

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string)))
            {
                property.AsProperty().Builder.HasMaxLength(256, ConfigurationSource.Convention);
                property.SqlServer().ColumnType = $"varchar({property.GetMaxLength()})";
            }
        }
    }
}

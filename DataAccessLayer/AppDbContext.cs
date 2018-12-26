using Core.Common;
using Core.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;

namespace DataAccessLayer
{
    public partial class AppDbContext : AuditableDbContext
    {
        public AppDbContext()
        {

        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        //public AppDbContext(DbContextOptions options, IUserService userService) : base(options, userService)
        //{
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BarConfiguration());

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

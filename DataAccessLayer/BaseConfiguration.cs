using Core.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer
{
    public partial class AppDbContext
    {
        public class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
            where TEntity : EfBaseModel
        {
            public virtual void Configure(EntityTypeBuilder<TEntity> builder)
            {
                builder.HasKey(p => p.Id);
                builder.Property(p => p.Id).ValueGeneratedNever();
                builder.Property(p => p.CreatedDate).HasDefaultValueSql("getutcdate()");
                builder.Property(p => p.ModifiedDate).HasDefaultValueSql("getutcdate()");
                builder.Property(p => p.IsDeleted).HasDefaultValueSql("0");
            }
        }
    }
}

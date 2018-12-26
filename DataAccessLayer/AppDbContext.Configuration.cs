using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer
{
    public partial class AppDbContext
    {
        public class BarConfiguration : BaseConfiguration<Models.Bar>
        {
            public override void Configure(EntityTypeBuilder<Models.Bar> builder)
            {
                base.Configure(builder);

                builder.Property(p => p.Name).HasMaxLength(20);
                builder.Property(p => p.Description).HasMaxLength(500);
            }
        }
    }
}

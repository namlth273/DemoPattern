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

        public class ProductConfiguration : BaseConfiguration<Models.Product>
        {
            public override void Configure(EntityTypeBuilder<Models.Product> builder)
            {
                base.Configure(builder);

                builder.Property(p => p.Name).HasMaxLength(20);
                builder.Property(p => p.Description).HasMaxLength(500);
            }
        }

        public class ProductInventoryConfiguration : BaseConfiguration<Models.ProductInventory>
        {
            public override void Configure(EntityTypeBuilder<Models.ProductInventory> builder)
            {
                base.Configure(builder);

                builder.Property(p => p.Quantity).IsRequired();
            }
        }

        public class OrderItemConfiguration : BaseConfiguration<Models.OrderItem>
        {
            public override void Configure(EntityTypeBuilder<Models.OrderItem> builder)
            {
                base.Configure(builder);

                builder.Property(p => p.OrderItemType).IsRequired();
                builder.Property(p => p.Quantity).IsRequired();
                //builder.HasOne<Models.Product>().WithMany(m => m.OrderItems).HasForeignKey(k => k.ProductId);
            }
        }

        public class ProductColorConfiguration : BaseConfiguration<Models.ProductColor>
        {
            public override void Configure(EntityTypeBuilder<Models.ProductColor> builder)
            {
                base.Configure(builder);

                builder.Property(p => p.ColorType).IsRequired();
            }
        }

        public class ProductSizeConfiguration : BaseConfiguration<Models.ProductSize>
        {
            public override void Configure(EntityTypeBuilder<Models.ProductSize> builder)
            {
                base.Configure(builder);

                builder.Property(p => p.SizeType).IsRequired();
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.DomainModel.Entities
{
    public class ProductType : BaseEntity
    {

        public byte ProductTypeID { get; set; }

        public string Name { get; set; }

        public float Width { get; set; }

        public byte StackedCount { get; set; }

    }

    internal class ProductTypeEntityTypeConfiguration : IEntityTypeConfiguration<ProductType>
    {
        public void Configure(EntityTypeBuilder<ProductType> builder)
        {
            builder.HasKey(q => q.ProductTypeID);

            builder.Property(q => q.ProductTypeID)
                .ValueGeneratedNever(); // don't make the key identity

            builder.Property(q => q.Name)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(q => q.Width)
                .IsRequired();

            builder.Property(q => q.StackedCount)
                .IsRequired()
                .HasDefaultValue(1);
        }
    }

}

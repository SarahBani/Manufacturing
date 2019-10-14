using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Core.DomainModel.Entities
{

    public class OrderDetail : BaseEntity
    {

        public long OrderDetailID { get; set; }

        public long OrderID { get; set; }

        public byte ProductTypeID { get; set; }

        public short Quantity { get; set; }


        public virtual Order Order { get; set; }

        public virtual ProductType ProductType { get; set; }

    }

    internal class OrderDetailEntityTypeConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(q => q.OrderDetailID);

            builder.Property(q => q.OrderDetailID)
                .UseSqlServerIdentityColumn()
                .Metadata.BeforeSaveBehavior = PropertySaveBehavior.Ignore; // make the column Identity

            builder.Property(q => q.OrderID)
                .IsRequired();

            builder.Property(q => q.ProductTypeID)
                .IsRequired();

            builder.HasIndex(q => new { q.OrderID, q.ProductTypeID }) // unique index
                .IsUnique();

            builder.Property(q => q.Quantity)
                .IsRequired()
                .HasDefaultValue(1);

            builder.HasOne(q => q.Order)
                .WithMany(q => q.OrderDetails)
                .HasForeignKey(q => q.OrderID)
                .HasConstraintName("FK_OrderDetail_Order");

            builder.HasOne(q => q.ProductType)
                .WithMany()
                .HasForeignKey(q => q.ProductTypeID)
                .HasConstraintName("FK_OrderDetail_ProductType");
        }
    }

}

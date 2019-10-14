using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Core.DomainModel.Entities
{
    public class Order : BaseEntity
    {

        public long OrderID { get; set; }

        public float RequiredBinWidth { get; set; }


        public ICollection<OrderDetail> OrderDetails { get; set; }

    }

    internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(q => q.OrderID);

            builder.Property(q => q.OrderID)
                .ValueGeneratedNever(); // don't make the key identity

            builder.Property(q => q.RequiredBinWidth)
                .IsRequired();
        }

    }
}

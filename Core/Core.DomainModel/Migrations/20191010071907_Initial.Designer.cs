﻿// <auto-generated />
using Core.DomainModel.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Core.DomainModel.Migrations
{
    [DbContext(typeof(ManufacturingDbContext))]
    [Migration("20191010071907_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.DomainModel.Entities.Order", b =>
                {
                    b.Property<long>("OrderID");

                    b.Property<float>("RequiredBinWidth");

                    b.HasKey("OrderID");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("Core.DomainModel.Entities.OrderDetail", b =>
                {
                    b.Property<long>("OrderDetailID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("OrderID");

                    b.Property<byte>("ProductTypeID");

                    b.Property<short>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue((short)1);

                    b.HasKey("OrderDetailID");

                    b.HasIndex("ProductTypeID");

                    b.HasIndex("OrderID", "ProductTypeID")
                        .IsUnique();

                    b.ToTable("OrderDetail");
                });

            modelBuilder.Entity("Core.DomainModel.Entities.ProductType", b =>
                {
                    b.Property<byte>("ProductTypeID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<byte>("StackedCount")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue((byte)1);

                    b.Property<float>("Width");

                    b.HasKey("ProductTypeID");

                    b.ToTable("ProductType");
                });

            modelBuilder.Entity("Core.DomainModel.Entities.OrderDetail", b =>
                {
                    b.HasOne("Core.DomainModel.Entities.Order", "Order")
                        .WithMany("OrderDetails")
                        .HasForeignKey("OrderID")
                        .HasConstraintName("FK_OrderDetail_Order")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Core.DomainModel.Entities.ProductType", "ProductType")
                        .WithMany()
                        .HasForeignKey("ProductTypeID")
                        .HasConstraintName("FK_OrderDetail_ProductType")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

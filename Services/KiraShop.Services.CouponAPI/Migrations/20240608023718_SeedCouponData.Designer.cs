﻿// <auto-generated />
using System;
using KiraShop.Services.CouponAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KiraShop.Services.CouponAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240608023718_SeedCouponData")]
    partial class SeedCouponData
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KiraShop.Services.CouponAPI.Models.Coupon", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CouponCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("DiscountAmount")
                        .HasColumnType("float");

                    b.Property<int>("MinAmount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Coupons");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0313fe2d-3a0c-4d3b-b87c-6cbff5f5b6de"),
                            CouponCode = "10OFF",
                            DiscountAmount = 10.0,
                            MinAmount = 20
                        },
                        new
                        {
                            Id = new Guid("8aac1d4c-9efc-4452-b5ce-13c295bec92d"),
                            CouponCode = "20OFF",
                            DiscountAmount = 20.0,
                            MinAmount = 40
                        });
                });
#pragma warning restore 612, 618
        }
    }
}

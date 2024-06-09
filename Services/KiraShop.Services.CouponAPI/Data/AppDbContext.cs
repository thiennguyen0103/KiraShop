using KiraShop.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace KiraShop.Services.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>()
                .HasData(new Coupon()
                {
                    Id = Guid.Parse("0313fe2d-3a0c-4d3b-b87c-6cbff5f5b6de"),
                    CouponCode = "10OFF",
                    DiscountAmount = 10,
                    MinAmount = 20,
                });

            modelBuilder.Entity<Coupon>()
                .HasData(new Coupon()
                {
                    Id = Guid.Parse("8aac1d4c-9efc-4452-b5ce-13c295bec92d"),
                    CouponCode = "20OFF",
                    DiscountAmount = 20,
                    MinAmount = 40,
                });
        }
    }
}

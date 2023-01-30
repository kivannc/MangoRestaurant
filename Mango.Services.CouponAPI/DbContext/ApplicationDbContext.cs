using Mango.Services.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.DbContext;

public class ApplicationDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Coupon> Coupons { get; set; }
    //public DbSet<CartHeader> CartHeaders { get; set; }
    //public DbSet<CartDetails> CartDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Coupon>().HasData(
            new Coupon
            {
                CouponId = 1,
                CouponCode = "MANGO10",
                DiscountAmount = 10
            },
            new Coupon
            {
                CouponId = 2,
                CouponCode = "MANGO20",
                DiscountAmount = 20
            },
            new Coupon
            {
                CouponId = 3,
                CouponCode = "MANGO30",
                DiscountAmount = 30
            }
        );
    }
}
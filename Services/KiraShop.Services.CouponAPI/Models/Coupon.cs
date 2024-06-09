using System.ComponentModel.DataAnnotations;

namespace KiraShop.Services.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string CouponCode { get; set; } = string.Empty;

        [Required]
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}

namespace KiraShop.Services.CouponAPI.Dtos
{
    public class CouponRequestDto
    {
        public string CouponCode { get; set; } = string.Empty;
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}

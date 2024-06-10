namespace KiraShop.Services.AuthAPI.Dtos
{
    public class RefreshTokenDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
    }
}


namespace KiraShop.Services.AuthAPI.Dtos
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; } = new();
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}

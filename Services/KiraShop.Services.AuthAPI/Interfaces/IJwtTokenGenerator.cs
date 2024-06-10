using KiraShop.Services.AuthAPI.Dtos;
using KiraShop.Services.AuthAPI.Models;

namespace KiraShop.Services.AuthAPI.Interfaces
{
    public interface IJwtTokenGenerator
    {
        Task<string> GenerateToken(ApplicationUser applicationUser);
        RefreshTokenDto GenerateRefreshToken();
    }
}

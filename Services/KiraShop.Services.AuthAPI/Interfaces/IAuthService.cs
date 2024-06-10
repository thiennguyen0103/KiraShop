using KiraShop.Services.AuthAPI.Dtos;

namespace KiraShop.Services.AuthAPI.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto<Guid>> Register(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto<LoginResponseDto>> Login(LoginRequestDto loginRequestDto);
    }
}

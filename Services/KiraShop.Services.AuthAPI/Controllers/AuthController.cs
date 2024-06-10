using KiraShop.Services.AuthAPI.Dtos;
using KiraShop.Services.AuthAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KiraShop.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequestDto)
        {
            return Ok(await _authService.Register(registrationRequestDto));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            return Ok(await _authService.Login(loginRequestDto));
        }
    }
}

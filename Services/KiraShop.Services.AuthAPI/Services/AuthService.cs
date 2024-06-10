using AutoMapper;
using KiraShop.Services.AuthAPI.Data;
using KiraShop.Services.AuthAPI.Dtos;
using KiraShop.Services.AuthAPI.Enums;
using KiraShop.Services.AuthAPI.Exceptions;
using KiraShop.Services.AuthAPI.Interfaces;
using KiraShop.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace KiraShop.Services.AuthAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMapper _mapper;

        public AuthService(AppDbContext appDbContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IJwtTokenGenerator jwtTokenGenerator,
            IMapper mapper)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _mapper = mapper;
        }

        public async Task<ResponseDto<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(loginRequestDto.Email);
            if (existingUser is null)
            {
                throw new ApiException($"No accounts registered with {loginRequestDto.Email}");
            }

            var result = await _signInManager.PasswordSignInAsync(loginRequestDto.Email, loginRequestDto.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                throw new ApiException($"Invalid credentials for '{loginRequestDto.Email}'.");
            }
            if (!existingUser.EmailConfirmed)
            {
                throw new ApiException($"Account not confirmed for '{loginRequestDto.Email}'.");
            }

            var rolesList = await _userManager.GetRolesAsync(existingUser).ConfigureAwait(false);
            UserDto userDto = new()
            {
                Id = existingUser.Id,
                Email = existingUser.Email!,
                FirstName = existingUser.FirstName,
                LastName = existingUser.LastName,
                PhoneNumber = existingUser.PhoneNumber!,
                Roles = rolesList
            };
            var token = await _jwtTokenGenerator.GenerateToken(existingUser);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            var loginResponseDto = new LoginResponseDto()
            {
                User = userDto,
                AccessToken = token,
                RefreshToken = refreshToken.Token
            };

            return new ResponseDto<LoginResponseDto>(loginResponseDto, $"Authenticated {existingUser.Email}");
        }

        public async Task<ResponseDto<Guid>> Register(RegistrationRequestDto registrationRequestDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registrationRequestDto.Email);

            if (existingUser is not null)
            {
                throw new ApiException($"Email {registrationRequestDto.Email} is already registered");
            }


            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email,
                FirstName = registrationRequestDto.FirstName,
                LastName = registrationRequestDto.LastName,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.User.ToString());
                return new ResponseDto<Guid>(user.Id);
            }
            else
            {
                throw new ApiException($"{result.Errors}");
            }
        }
    }
}

using AutoMapper;
using KiraShop.Services.AuthAPI.Data;
using KiraShop.Services.AuthAPI.Dtos;
using KiraShop.Services.AuthAPI.Interfaces;
using KiraShop.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace KiraShop.Services.AuthAPI.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions,
            UserManager<ApplicationUser> userManager,
            AppDbContext appDbContext,
            IMapper mapper)
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public RefreshTokenDto GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };

            // Save to the database
            _appDbContext.RefreshTokens.Add(refreshToken);
            _appDbContext.SaveChanges();

            return _mapper.Map<RefreshTokenDto>(refreshToken);
        }

        public async Task<string> GenerateToken(ApplicationUser applicationUser)
        {
            var userClaims = await _userManager.GetClaimsAsync(applicationUser);
            var roles = await _userManager.GetRolesAsync(applicationUser);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email!),
                new Claim("uid", applicationUser.Id.ToString()),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(_jwtOptions.DurationInDays),
                SigningCredentials = signingCredentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string RandomTokenString()
        {
            var randomNumber = new Byte[32];
            RandomNumberGenerator.Fill(randomNumber);
            string token = Convert.ToBase64String(randomNumber);
            return token;
        }
    }
}

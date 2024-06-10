using AutoMapper;
using KiraShop.Services.AuthAPI.Dtos;
using KiraShop.Services.AuthAPI.Models;

namespace KiraShop.Services.AuthAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ApplicationUser, UserDto>();
                config.CreateMap<RefreshToken, RefreshTokenDto>();
            });

            return mappingConfig;
        }
    }
}

using AutoMapper;
using KiraShop.Services.CouponAPI.Dtos;
using KiraShop.Services.CouponAPI.Models;

namespace KiraShop.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon, CouponDto>().ReverseMap();
                config.CreateMap<Coupon, CouponRequestDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}

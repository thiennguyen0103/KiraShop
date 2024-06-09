using AutoMapper;
using KiraShop.Services.CouponAPI.Data;
using KiraShop.Services.CouponAPI.Dtos;
using KiraShop.Services.CouponAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace KiraShop.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;

        public CouponController(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<ResponseDto<IEnumerable<Coupon>>> GetCouponList()
        {
            var response = new ResponseDto<IEnumerable<CouponDto>>();
            try
            {
                var couponList = _appDbContext.Coupons.ToList();
                response.Result = _mapper.Map<IEnumerable<CouponDto>>(couponList);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return Ok(response);
        }

        [HttpGet("{id}", Name = "GetCouponById")]
        public ActionResult<ResponseDto<CouponDto>> GetCouponById(Guid id)
        {
            var response = new ResponseDto<CouponDto>();
            try
            {
                var coupon = _appDbContext.Coupons.FirstOrDefault(c => c.Id == id);

                if (coupon is null)
                {
                    response.IsSuccess = false;
                    return NotFound();
                }

                response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpGet("GetByCode/{code}")]
        public ActionResult<ResponseDto<CouponDto>> GetCouponById(string code)
        {
            var response = new ResponseDto<CouponDto>();
            try
            {
                var coupon = _appDbContext.Coupons.FirstOrDefault(c => c.CouponCode == code);

                if (coupon is null)
                {
                    response.IsSuccess = false;
                    return NotFound(response);
                }

                response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        public ActionResult<ResponseDto<CouponDto>> CreateCoupon(CouponRequestDto couponDto)
        {
            var response = new ResponseDto<CouponDto>();
            var coupon = _mapper.Map<Coupon>(couponDto);

            try
            {
                _appDbContext.Coupons.Add(coupon);
                _appDbContext.SaveChanges();

                response.Result = _mapper.Map<CouponDto>(coupon);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return CreatedAtRoute(nameof(GetCouponById), new { Id = coupon.Id}, response);
        }

        [HttpPut("{id}")]
        public ActionResult<ResponseDto<CouponDto>> UpdateCoupon(Guid id, CouponRequestDto couponDto)
        {
            var response = new ResponseDto<CouponDto>();
            try
            {
                var existedCoupon = _appDbContext.Coupons.FirstOrDefault(c => c.Id == id);

                if (existedCoupon is null)
                {
                    response.IsSuccess = false;
                    return NotFound(response);
                }

                existedCoupon.CouponCode = couponDto.CouponCode;
                existedCoupon.DiscountAmount = couponDto.DiscountAmount;
                existedCoupon.MinAmount = couponDto.MinAmount;

                _appDbContext.Coupons.Update(existedCoupon);
                _appDbContext.SaveChanges();

                response.Result = _mapper.Map<CouponDto>(existedCoupon);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public ActionResult<ResponseDto<CouponDto>> DeleteCouponById(Guid id)
        {
            var response = new ResponseDto<CouponDto>();
            try
            {
                var coupon = _appDbContext.Coupons.FirstOrDefault(c => c.Id == id);

                if (coupon is null)
                {
                    response.IsSuccess = false;
                    return NotFound(response);
                }

                _appDbContext.Coupons.Remove(coupon);
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }
    }
}

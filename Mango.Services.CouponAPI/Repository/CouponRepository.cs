using AutoMapper;
using Mango.Services.CouponAPI.DbContext;
using Mango.Services.CouponAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Repository;

public class CouponRepository : ICouponRepository
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CouponRepository(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<CouponDto> GetCouponByCode(string couponCode)
    {
        var coupon = await _db.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);
        return _mapper.Map<CouponDto>(coupon);
    }

    
}
using AutoMapper;
using Mango.Services.ShoppingCartAPI.DbContext;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ShoppingCartAPI.Repository;

public class CartRepository : ICartRepository
{

    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CartRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    } 
    public async Task<CartDto> GetCartByUserId(string userId)
    {
        Cart cart = new()
        {
            CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId)
        };

        cart.CartDetails = await _context.CartDetails
            .Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId)
            .Include(u=> u.Product).ToListAsync();

        return _mapper.Map<CartDto>(cart);
    }

    public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
    {
        Cart cart = _mapper.Map<CartDto, Cart>(cartDto);
        var productInDb = _context.Products.FirstOrDefault(u => 
            u.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId);

        if (productInDb == null)
        {
            _context.Products.Add(cart.CartDetails.FirstOrDefault().Product);  
            await _context.SaveChangesAsync();
        }

        var cartHeaderFromDb = _context.CartHeaders.AsNoTracking().FirstOrDefault(u => 
            u.UserId == cart.CartHeader.UserId);

        if (cartHeaderFromDb == null)
        {
            _context.CartHeaders.Add(cart.CartHeader);
            await _context.SaveChangesAsync();
            cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
            cart.CartDetails.FirstOrDefault().Product = null;
            _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
            await _context.SaveChangesAsync();
        }

        else
        {
           var cartDetailsFromDb = await  _context.CartDetails.AsNoTracking().FirstOrDefaultAsync(
               u => u.CartHeaderId == cartHeaderFromDb.CartHeaderId &&
                    u.ProductId == cart.CartDetails.FirstOrDefault().ProductId);

            if (cartDetailsFromDb == null)
            {
                cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                _context.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
            else
            {
                cart.CartDetails.FirstOrDefault().Product = null;
                cart.CartDetails.FirstOrDefault().Count += cartDetailsFromDb.Count;
                _context.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                await _context.SaveChangesAsync();
            }
        }

        return  _mapper.Map<CartDto>(cart);
    }

    public async Task<bool> RemoveFromCart(int cartDetailsId)
    {
        CartDetails cartDetails = await _context.CartDetails
            .FirstOrDefaultAsync(u => u.CartDetailsId == cartDetailsId);

        if (cartDetails == null)
        {
            return false;
        }

        int totalCountOfCardItems = _context.CartDetails
            .Count(u => u.CartHeaderId == cartDetails.CartHeaderId);
        _context.Remove(cartDetails);
        if (totalCountOfCardItems == 1)
        {
            var cartHeaderToRemove = await _context.CartHeaders
                .FirstOrDefaultAsync(u => u.CartHeaderId == cartDetails.CartHeaderId);
            _context.CartHeaders.Remove(cartHeaderToRemove);
        }

        await _context.SaveChangesAsync();
        return true;
        
    }

    public async Task<bool> ApplyCoupon(string couponCode, string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> RemoveCoupon(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ClearCart(string userId)
    {
        var cartHeaderFromDb = _context.CartHeaders.AsNoTracking().FirstOrDefault(u =>
            u.UserId == userId);

        if (cartHeaderFromDb == null)
        {
            return false;
        }

        var cartDetailsFromDb = await _context.CartDetails.AsNoTracking().Where(
            u =>
            u.CartHeaderId == cartHeaderFromDb.CartHeaderId).ToListAsync();

        _context.CartDetails.RemoveRange(cartDetailsFromDb);
        _context.CartHeaders.Remove(cartHeaderFromDb);
        await _context.SaveChangesAsync();
        return true;
    }
}
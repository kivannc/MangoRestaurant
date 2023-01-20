using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Repository;

public interface ICartRepository
{
    Task<CartDto> GetCartByUserId(string userId);
    Task<CartDto> CreateUpdateCart(CartDto cartDto);
    Task<bool> RemoveFromCart(int cartDetailsId);
    Task<bool> ApplyCoupon(string couponCode, string userId);
    Task<bool> RemoveCoupon(string userId);
    Task<bool> ClearCart(string userId);
}
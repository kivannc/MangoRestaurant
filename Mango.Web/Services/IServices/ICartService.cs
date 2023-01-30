using Mango.Web.Models;

namespace Mango.Web.Services.IServices;

public interface ICartService : IBaseService
{
    Task<T> GetCartByUserIdAsync<T>(string userId, string token = null);
    Task<T> AddToCartAsync<T>(CartDto cartDto, string token);
    Task<T> UpdateCartAsync<T>(CartDto cartDto, string token);
    Task<T> RemoveFromCartAsync<T>(int cardId, string token);
    Task<T> ApplyCoupon<T>(CartDto cartDto, string token);
    Task<T> RemoveCoupon<T>(string userId,string token);
}
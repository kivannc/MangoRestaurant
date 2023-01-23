using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services;

public class CartService : BaseService, ICartService
{

    private readonly IHttpClientFactory _httpClientFactory;

    public CartService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    
    public async Task<T> GetCartByUserIdAsync<T>(string userId, string token = null)
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.ShoppingCartAPIBase + "/api/cart/GetCart/" + userId,
            AccessToken = token,
        });
    }

    public async Task<T> AddToCartAsync<T>(CartDto cartDto, string token)
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.POST,
            Url = SD.ShoppingCartAPIBase + "/api/cart/AddCart",
            AccessToken = token,
            Data = cartDto
        });
    }

    public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string token)
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.PUT,
            Url = SD.ShoppingCartAPIBase + "/api/cart/UpdateCart",
            AccessToken = token,
            Data = cartDto
        });
    }

    public  async Task<T> RemoveFromCartAsync<T>(int cardId, string token)
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Url = SD.ShoppingCartAPIBase + "/api/cart/RemoveCart/" ,
            AccessToken = token,
            Data = cardId
        });
    }
    
}
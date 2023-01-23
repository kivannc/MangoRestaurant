using System.Text.Json;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public CartController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }


        public async Task<IActionResult> CartIndex()
        {
            var cartDto = await LoadCartDtoBasedOnLoggedInUser();
            return View(cartDto);
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == "sub")?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var response = await _cartService.RemoveFromCartAsync<ResponseDto>(cartDetailsId, accessToken);

            if (response is { IsSuccess: true })
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return RedirectToAction(nameof(CartIndex));
        }

        private async Task<CartDto> LoadCartDtoBasedOnLoggedInUser()
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == "sub")?.Value;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var cartDto = new CartDto();
            var response = await _cartService.GetCartByUserIdAsync<ResponseDto>(userId, accessToken);
            if (response is { IsSuccess: true })
            {
                cartDto = JsonSerializer.Deserialize<CartDto>(Convert.ToString(response.Result)!, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            if (cartDto is { CartHeader: { } })
            {
                foreach (var detail in cartDto.CartDetails)
                {
                    cartDto.CartHeader.OrderTotal += (detail.Product.Price * detail.Count);
                }
              
            }
            return cartDto;
        }
    }
}
using System.Text.Json;
using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> productList = new();
            var response = await _productService.GetAllProductsAsync<ResponseDto>();
            if (response is { IsSuccess: true, Result: { } })
            {
                productList = JsonSerializer.Deserialize<List<ProductDto>>(Convert.ToString(response.Result)!, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            return View(productList);
        }
    }
}

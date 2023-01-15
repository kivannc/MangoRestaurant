using System.Reflection;
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

        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.CreateProductAsync<ResponseDto>(model);
                if (response is { IsSuccess: true, Result: { } })
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);

        }

        public async Task<IActionResult> ProductEdit(int productId)
        {
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productId);
            if (response is { IsSuccess: true, Result: { } })
            {
                var product = JsonSerializer.Deserialize<ProductDto>(Convert.ToString(response.Result)!, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                return View(product);

            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.UpdateProductAsync<ResponseDto>(model);
                if (response is { IsSuccess: true, Result: { } })
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);

        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            var response = await _productService.GetProductByIdAsync<ResponseDto>(productId);
            if (response is { IsSuccess: true, Result: { } })
            {
                var product = JsonSerializer.Deserialize<ProductDto>(Convert.ToString(response.Result)!, new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true
                });

                return View(product);
            }

            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(int productId,ProductDto model)
        {
            if (ModelState.IsValid)
            {
                var response = await _productService.DeleteProductAsync<ResponseDto>(productId);
                if (response is { IsSuccess: true, Result: { } })
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);

        }
    }
}

﻿using Mango.Web.Models;
using Mango.Web.Services.IServices;

namespace Mango.Web.Services;

public class ProductService : BaseService, IProductService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<T> GetAllProductsAsync<T>()
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.ProductAPIBase + "/api/products",
            AccessToken = "",
        });
    }

    public async Task<T> GetProductByIdAsync<T>(int id)
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = SD.ProductAPIBase + "/api/products/" + id,
            AccessToken = "",
        });
    }

    public async Task<T> CreateProductAsync<T>(ProductDto productDto)
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.POST,
            Url = SD.ProductAPIBase + "/api/products",
            AccessToken = "",
            Data = productDto
        });
    }

    public async Task<T> UpdateProductAsync<T>(ProductDto productDto)
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.PUT,
            Url = SD.ProductAPIBase + "/api/products",
            AccessToken = "",
            Data = productDto
        });
    }

    public async Task<T> DeleteProductAsync<T>(int id)
    {
        return await SendAsync<T>(new ApiRequest()
        {
            ApiType = SD.ApiType.DELETE,
            Url = SD.ProductAPIBase + "/api/products/" + id,
            AccessToken = "",
        });
    }
}
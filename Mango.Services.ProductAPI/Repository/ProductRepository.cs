using AutoMapper;
using Mango.Services.ProductAPI.DbContext;
using Mango.Services.ProductAPI.Model.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ProductRepository(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<ProductDto>> GetProducts()
    {
        var products = await _context.Products.ToListAsync();
        var productDtoList = _mapper.Map<List<ProductDto>>(products);
        return productDtoList;
    }

    public async Task<ProductDto> GetProductById(int productId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(u => u.ProductId == productId);
        var productDto = _mapper.Map<ProductDto>(product);
        return productDto;
    }

    public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
    {
        var product = _mapper.Map<ProductDto, Model.Product>(productDto);
        if (product.ProductId > 0)
        {
            _context.Products.Update(product);
        }
        else
        {
            _context.Products.Add(product);
        }

        await _context.SaveChangesAsync();
        
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<bool> DeleteProduct(int productId)
    {
        try
        {
            var product = _context.Products.FirstOrDefault(u => u.ProductId == productId);
            if (product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}
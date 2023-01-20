using AutoMapper;

namespace Mango.Services.ShoppingCartAPI;

public static class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            //config.CreateMap<Model.ProductDto, ProductDto>();
            //config.CreateMap<ProductDto, Model.ProductDto>();
        });

        return mappingConfig;
    }
}
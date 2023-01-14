using AutoMapper;
using Mango.Services.ProductAPI.Model.Dto;

namespace Mango.Services.ProductAPI;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<Model.Product, ProductDto>();
            config.CreateMap<ProductDto, Model.Product>();
        });

        return mappingConfig;
    }
}
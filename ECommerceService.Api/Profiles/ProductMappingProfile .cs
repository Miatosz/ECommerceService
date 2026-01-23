using AutoMapper;
using ECommerceService.Api.Dto;
using ECommerceService.Api.Models;

namespace ECommerceService.Api.Profiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<Product, GetProductDto>();
        }
    }
}

using AutoMapper;
using ECommerceService.Api.Dto;
using ECommerceService.Api.Models;

namespace ECommerceService.Api.Profiles
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<CreateOrderItemDto, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderItems, opt => opt.Ignore());

            CreateMap<OrderItem, GetOrderItemDto>();

            CreateMap<Order, GetOrderDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}

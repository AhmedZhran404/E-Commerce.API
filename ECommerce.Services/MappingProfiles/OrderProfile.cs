using AutoMapper;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared.OrdersDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddressDTO, OrderAddress>().ReverseMap();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(Dest => Dest.DeliveryMethod , op => op.MapFrom(src => src.DeliveryMethod.ShortName));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(Dest => Dest.ProductName , O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(Dest => Dest.PictureUrl , opt => opt.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<DeliveryMethod, DeliveryMethodDTO>();
        }
    }
}

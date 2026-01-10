using AutoMapper;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfiles
{
    internal class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<ProductType, TypeDTO>();

            CreateMap<Product, ProductDTO>()
                    .ForMember(Dest => Dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                    .ForMember(Dest => Dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
                    .ForMember(Dest => Dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>());

        }
    }
}

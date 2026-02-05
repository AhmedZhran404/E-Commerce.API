using AutoMapper;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Shared.OrdersDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<Address , AddressDTO>().ReverseMap();
        }
    }
}

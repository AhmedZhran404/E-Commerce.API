using AutoMapper;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared.ProductDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfiles
{
    internal class ProductPictureUrlResolver : IValueResolver<Product, ProductDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration) 
        {
            this._configuration = configuration;
        }
        public string Resolve(Product source, ProductDTO destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
            {
                return string.Empty;
            }

            if(source.PictureUrl.StartsWith("http") ||  source.PictureUrl.StartsWith("https"))
            {
                return source.PictureUrl;
            }
            var BaseUrl = _configuration.GetSection("URLs")["BaseUrl"];
            var PictureUrl = $"{BaseUrl}{source.PictureUrl}";

            return PictureUrl;
        }
    }
}

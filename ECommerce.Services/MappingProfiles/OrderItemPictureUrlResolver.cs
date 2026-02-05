using AutoMapper;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Shared.OrdersDTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.MappingProfiles
{
    public class OrderItemPictureUrlResolver : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            
            if(string.IsNullOrEmpty(source.Product.ProductUrl))
                return string.Empty;


            if(source.Product.ProductUrl.StartsWith("http") || source.Product.ProductUrl.StartsWith("https"))
                return source.Product.ProductUrl;

            var BaseUrl = _configuration.GetSection("URLs")["BaseUrl"];

            if(string.IsNullOrEmpty(BaseUrl))
                return string.Empty;

            return $"{BaseUrl}{source.Product.ProductUrl}";

        }
    }
}

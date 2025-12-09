using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Spacifications.ProductsSpecifications
{
    internal static class ProductSpecificationHelper
    {
        public static Expression<Func<Product , bool>> critariaFunc(ProductQueryParams queryParams)
        {
            return P =>
                     (!queryParams.brandId.HasValue || P.ProductBrandId == queryParams.brandId.Value)
                     &&
                     (!queryParams.typeId.HasValue || P.ProductTypeId == queryParams.typeId.Value)
                     &&
                     (string.IsNullOrEmpty(queryParams.search) || P.Name.ToLower().Contains(queryParams.search.ToLower()));

        }
    }
}

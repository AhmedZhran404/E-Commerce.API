using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Spacifications.ProductsSpecifications
{
    internal class ProductWithBrandsAndTypesSpecifications : BaseSpacification<Product , int>
    {
        public ProductWithBrandsAndTypesSpecifications(ProductQueryParams queryParams)
            :base(
                     P => 
                     (!queryParams.brandId.HasValue || P.ProductBrandId == queryParams.brandId.Value) 
                     &&
                     (!queryParams.typeId.HasValue || P.ProductTypeId == queryParams.typeId.Value)
                     &&
                     (string.IsNullOrEmpty(queryParams.search) || P.Name.ToLower().Contains(queryParams.search.ToLower()))
            )
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }

        public ProductWithBrandsAndTypesSpecifications(int id):base(P => P.Id == id)
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
    }
}

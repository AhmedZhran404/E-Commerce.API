using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Spacifications.ProductsSpecifications
{
    public class ProductWithCountspecifications : BaseSpacification<Product , int>
    {
        public ProductWithCountspecifications(ProductQueryParams queryParams) 
            : base(ProductSpecificationHelper.critariaFunc(queryParams))
        {}
    }
}

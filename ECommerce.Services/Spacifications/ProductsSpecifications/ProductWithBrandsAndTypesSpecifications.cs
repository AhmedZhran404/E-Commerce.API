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
            :base(ProductSpecificationHelper.critariaFunc(queryParams))
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);

            switch (queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderByAsc(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDesc(p => p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderByAsc(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDesc(p => p.Price);
                    break;
                default:
                    AddOrderByAsc(p => p.Id);
                    break;
            }




            ApplyPagenation(queryParams.PageIndex, queryParams.PageSize);
        }

        public ProductWithBrandsAndTypesSpecifications(int id):base(P => P.Id == id)
        {
            AddInclude(P => P.ProductBrand);
            AddInclude(P => P.ProductType);
        }
    }
}

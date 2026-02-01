using ECommerce.Shared;
using ECommerce.Shared.CommonResposes;
using ECommerce.Shared.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstraction
{
    public interface IProductService
    {

        Task<PaginatedResult<ProductDTO>> GetAllProductAsync(ProductQueryParams queryParams);

        Task<Result<ProductDTO>> GetProductByIdAsync(int id);

        Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();
        Task<IEnumerable<TypeDTO>> GetAllTypesAsync();

    }
}

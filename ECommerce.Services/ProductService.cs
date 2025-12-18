using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Services.Abstraction;
using ECommerce.Services.Exceptions;
using ECommerce.Services.Spacifications.ProductsSpecifications;
using ECommerce.Shared;
using ECommerce.Shared.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var Brands = await _unitOfWork.GetRepository<ProductBrand , int>().GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDTO>>(Brands);
            
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductAsync(ProductQueryParams queryParams)
        {
           
            var ProdectElement =  _unitOfWork.GetRepository<Product, int>();

            var spec = new ProductWithBrandsAndTypesSpecifications(queryParams);
            var Products = await ProdectElement.GetAllAsync(spec);
            var DataToResult =  _mapper.Map<IEnumerable<ProductDTO>>(Products);
            var CountOfResultData = DataToResult.Count();

            var CountSpec = new ProductWithCountspecifications(queryParams);
            var CountOverAll = await ProdectElement.CountAsync(CountSpec);


            return new PaginatedResult<ProductDTO>(queryParams.PageIndex, CountOfResultData, CountOverAll, DataToResult);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {

            var Types = await _unitOfWork.GetRepository<ProductType , int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDTO>>(Types);

        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandsAndTypesSpecifications(id);
            var Product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(spec);

            if(Product is null)
            {
                throw new ProductNotFound(id);
            }

            return _mapper.Map<ProductDTO>(Product);

        }
    }
}

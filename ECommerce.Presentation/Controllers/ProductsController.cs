using ECommerce.Presentation.Attributes;
using ECommerce.Services.Abstraction;
using ECommerce.Shared;
using ECommerce.Shared.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{

    public class ProductsController : ApiBaseControllers
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            this._productService = productService;
        }

        // Get All Products
        [HttpGet]
        // GET: baseUrl/api/Products
        [RedisCache(5)]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            
            var Products = await _productService.GetAllProductAsync(queryParams);
            return Ok(Products);
        }

        // Get Product By Id
        [HttpGet("{id}")]
        // GET: baseUrl/api/Products/2
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
           
            var result = await _productService.GetProductByIdAsync(id);

            return HandleResult<ProductDTO>(result);
        }

        // Get All Brands
        [HttpGet("brands")]
        // GET: baseUrl/api/Products/brands
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var Brands = await _productService.GetAllBrandsAsync();
            return Ok(Brands);
        }


        // Get All types
        [HttpGet("types")]
        // GET: baseUrl/api/Products/types
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var Types = await _productService.GetAllTypesAsync();
            return Ok(Types);
        }


    }
}

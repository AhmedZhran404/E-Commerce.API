using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // GET: baseUrl/api/Product
        [HttpGet("{id}")]
        public ActionResult<Product> GetById(int id)
        {
            return new Product
            {
                Id = id,
                Name = "Test Product"
            };
        }

        // GET: baseUrl/api/Product
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAll()
        {
            return new List<Product>();
        }

        [HttpPost]
        public ActionResult<Product> AddProduct(Product item)
        {
            return item;
        }

        [HttpPut]
        public ActionResult<Product> UpdateProduct(Product item)
        {
            return item;
        }
    }
}

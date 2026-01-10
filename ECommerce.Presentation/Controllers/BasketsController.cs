using ECommerce.Services.Abstraction;
using ECommerce.Shared.BasketDTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketsController(IBasketService basketService)
        {
            this._basketService = basketService;
        }

        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateAsync(BasketDTO basketDTO)
        {

            var basket = await _basketService.CreateOrUpdateBasket(basketDTO);

            return Ok(basket);
        }

        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasketAsync(string id)
        {
            var basket = await _basketService.GetBasketAsync(id);

            return Ok(basket);
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult<bool>> DeleteBasketAsync([FromRoute] string id)
        {
            var result = await _basketService.DeleteBasketAsync(id);

            return Ok(result);
        }

    }
}

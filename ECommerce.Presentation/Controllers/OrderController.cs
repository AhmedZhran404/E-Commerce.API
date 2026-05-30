using ECommerce.Services.Abstraction;
using ECommerce.Shared.CommonResposes;
using ECommerce.Shared.OrdersDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Presentation.Controllers
{
    public class OrdersController : ApiBaseControllers
    {
        private readonly IOrderSevice _orderSevice;

        public OrdersController(IOrderSevice orderSevice)
        {
            _orderSevice = orderSevice;
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder(OrderDTO orderDTO)
        {
            var result = await _orderSevice.CreateOrderAsync(orderDTO , GetEmailFromToken());

            return HandleResult(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderToReturnDTO>>> GetOrders()
        {

            var result = await _orderSevice.GetAllOrderAsync(GetEmailFromToken());

            return HandleResult(result);
        }


        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrder(Guid id)
        {
            var result = await _orderSevice.GetSpecificOrder(GetEmailFromToken() , id);

            return HandleResult(result);

        }

        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods()
        {
            var result = await _orderSevice.GetAllDeliveryMethodsAsync();
            return HandleResult(result);
        }



    }
}

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
    public class PaymentsController : ApiBaseControllers
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        // POST: baseUrl/api/payments/{basketId}
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdatePayentIntent(string basketId)
        {
            var result = await _paymentService.CreateOrUpdatePaymentAsync(basketId);
            return HandleResult(result);
        }

    }
}

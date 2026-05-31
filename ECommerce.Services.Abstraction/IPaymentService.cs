using ECommerce.Shared.BasketDTO;
using ECommerce.Shared.CommonResposes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstraction
{
    public interface IPaymentService
    {
        Task<Result<BasketDTO>> CreateOrUpdatePaymentAsync(string basketId);

        Task UpdateOrderPaymentStatus(string request , string stripeSignature);
    }
}

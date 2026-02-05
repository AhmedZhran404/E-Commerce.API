using ECommerce.Shared.CommonResposes;
using ECommerce.Shared.OrdersDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstraction
{
    public interface IOrderSevice
    {
        // Create Order (OrderDTO , Email) ==> OrderToReturnDTO

        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO , string email);

        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethodsAsync();

        Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrderAsync(string email);

        Task<Result<OrderToReturnDTO>> GetSpecificOrder(string email , Guid Id);

    }
}

using ECommerce.Shared.BasketDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Abstraction
{
    public interface IBasketService
    {
        Task<BasketDTO?> CreateOrUpdateBasket(BasketDTO basketDTO);

        Task<BasketDTO?> GetBasketAsync(string basketId);

        Task<bool> DeleteBasketAsync(string basketId);
    }
}

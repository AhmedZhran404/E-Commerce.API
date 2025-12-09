using ECommerce.Domain.Entities.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts
{
    public interface IBasketRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string basketId);

        Task<bool> DeleteBasketAsync(string basketId);

        Task<CustomerBasket?> CreateOrUpdateAsync(CustomerBasket basket , TimeSpan timeToLive = default); // 00:00:00 
    }
}

using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task<CustomerBasket?> CreateOrUpdateAsync(CustomerBasket basket, TimeSpan timeToLive = default)
        {

            var JsonBasket = JsonSerializer.Serialize(basket);

            var IsCreatedOrUpdated = await _database.StringSetAsync(basket.Id, JsonBasket, (timeToLive == default) ? TimeSpan.FromDays(7) : timeToLive);

            return  await GetBasketAsync(basket.Id);

        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var BasketReturned = await _database.StringGetAsync(basketId);

            if(BasketReturned.IsNullOrEmpty)
                return null;
            else 
                return JsonSerializer.Deserialize<CustomerBasket>(BasketReturned!);
        }
    }
}

using ECommerce.Domain.Contracts;
using ECommerce.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;

        public CacheService(ICacheRepository cacheRepository)
        {
            this._cacheRepository = cacheRepository;
        }
        public async Task<string?> GetAsync(string cacheKey)
        {
          return await _cacheRepository.GetAsync(cacheKey);       
        }

        public async Task SetAsync(string cacheKey, object cachValue, TimeSpan timeToLive)
        {
            var value = JsonSerializer.Serialize(cachValue , new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await _cacheRepository.SetAsync(cacheKey, value, timeToLive);
        }
    }
}

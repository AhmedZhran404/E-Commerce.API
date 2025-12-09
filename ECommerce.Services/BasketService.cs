using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Services.Abstraction;
using ECommerce.Shared.BasketDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository , IMapper mapper)
        {
            this._basketRepository = basketRepository;
            this._mapper = mapper;
        }
        public async Task<BasketDTO?> CreateOrUpdateBasket(BasketDTO CreatedOrUpdatedBasketDTO)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(CreatedOrUpdatedBasketDTO);

            var CreatedOrUpdatedBasket = await _basketRepository.CreateOrUpdateAsync(customerBasket);

            return _mapper.Map<BasketDTO>(CreatedOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _basketRepository.DeleteBasketAsync(basketId);
        }

        public async Task<BasketDTO?> GetBasketAsync(string basketId)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);

            return _mapper.Map<BasketDTO>(basket);

        }
    }
}

using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Services.Abstraction;
using ECommerce.Services.Spacifications.OrderSpecification;
using ECommerce.Shared.BasketDTO;
using ECommerce.Shared.CommonResposes;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = ECommerce.Domain.Entities.ProductModule.Product;

namespace ECommerce.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PaymentService(
                            IBasketRepository basketRepository ,
                            IUnitOfWork unitOfWork,
                            IConfiguration configuration ,
                            IMapper mapper
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<Result<BasketDTO>> CreateOrUpdatePaymentAsync(string basketId)
        {
            var skey = _configuration["Stripe:SKey"];
            if (skey is null)
                return Error.Faliure("Failed To Obtain Secret Key Value");
            StripeConfiguration.ApiKey = skey;

            // 1- retrive the basket by its Id
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if(basket is null)
                return Error.NotFound("Basket Not Found");

            // 2-Validate DeliveryMethod Inside Basket
            if (basket.DeliveryMethodId is null)
                return Error.Validation("Delivery Method Is Not Selected In The Basket");

            // 3- Retrive The Delivery Method details from the database
            var method = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value);
            if(method is null)
                return Error.NotFound("Delivery Method Is Not Found");

            basket.ShippingPrice = method.Price;

            // Total Amount
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if(product is null)
                {
                    return Error.NotFound("Product Is Not Found");
                }

                item.Price = product.Price;
                item.ProductName = product.Name;
                item.PictureUrl = product.PictureUrl;
            }

            long amount = (long)basket.Items.Sum(I => I.Quantity * I.Price) * 100;

            // 4- Create Or Update Payment Intent With Stripe API

            var stripeService = new PaymentIntentService();
            //basket PaymentInetentId Is  Null => Create
            if(basket.PaymentIntentID is null)
            {
                #region Integration With External Service
                // Integration With Any External Service
                // Dowenload package
                // Collection Of Classes
                // Classes As A DLL
                // Main Class To interact Wiht Stripe Api [Create From It Object]
                // User Service Inside The main Object 
                #endregion

                var options = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var paymentIntent = await stripeService.CreateAsync(options); // External Api Call

                basket.PaymentIntentID = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            // basket PaymentInetentId Is Not Null => Update
            else
            {

                var options = new PaymentIntentUpdateOptions
                {
                    Amount = amount,
                };
                await stripeService.UpdateAsync(basket.PaymentIntentID , options);
            }

            await _basketRepository.CreateOrUpdateAsync(basket);

            return _mapper.Map<BasketDTO>(basket);
        }

        public async Task UpdateOrderPaymentStatus(string request, string stripeSignature)
        {
            var endpointSecret = _configuration["Stripe:EndpointSecret"];
            var stripeEvent = EventUtility.ParseEvent(request, throwOnApiVersionMismatch: true);
             stripeEvent = EventUtility.ConstructEvent(request, stripeSignature, endpointSecret , throwOnApiVersionMismatch:true);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            Console.WriteLine(paymentIntent!.Id);
            var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdAsync(new OrderWithPaymentIntentSpec(paymentIntent!.Id));
            // Handle the event
            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                order.Status = OrderStatus.PaymentRecieved;
                _unitOfWork.GetRepository<Order, Guid>().Update(order);
                await _unitOfWork.SaveChangeAsync();
            }
            else if(stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                order.Status = OrderStatus.PaymentFaild;
                _unitOfWork.GetRepository<Order, Guid>().Update(order);
                await _unitOfWork.SaveChangeAsync();
            }
            // ... handle other event types
            else
            {
                Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
            }
        }
    }
}

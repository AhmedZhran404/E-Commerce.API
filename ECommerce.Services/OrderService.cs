using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Services.Abstraction;
using ECommerce.Services.Spacifications.OrderSpecification;
using ECommerce.Shared.CommonResposes;
using ECommerce.Shared.OrdersDTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace ECommerce.Services
{
    public class OrderService : IOrderSevice
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IMapper mapper , IBasketRepository basketRepository , IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string email)
        {


            //1- Maps the provided shipping address to the order address entity.
            var OrderAddress = _mapper.Map<OrderAddress>(orderDTO.Address);

            //2-Retrieves the basket and validates its existence.
            var basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if (basket is null)
               return Error.NotFound("Basket.NotFound", $"Basket With Id:{orderDTO.BasketId} Is Not Found");
            //3-Creates a list of order items by fetching product details from the database and validating each product.
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket!.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);

                if (product is null)
                    return Error.NotFound("Product.NotFound", $"Product With Id:{item.Id} Is Not Found");

                orderItems.Add(CreateOrderItem(item, product));

            }
            //4-Retrieves the selected delivery method and validates its existence.

            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);

            if (deliveryMethod is null)
                return Error.NotFound("deliveryMethod.NotFound", $"deliveryMethod With Id:{orderDTO.DeliveryMethodId} Is Not Found");


            //5-Calculates the subtotal of the order based on the items and their quantities.
            var subTotal = orderItems.Sum(OI => OI.Price * OI.Quantity);
            //6-Creates a new Order with all relevant details.
            var order = new Order()
            {
                UserEmail = email,
                Address = OrderAddress,
                DeliveryMethod = deliveryMethod,
                Items = orderItems,
                SubTotal = subTotal

            };

            await _unitOfWork.GetRepository<Order , Guid>().AddAsync(order); // Adding The Order And OrderItems Locally

            bool result = await _unitOfWork.SaveChangeAsync() > 0;

            if (!result)
                return Error.Faliure("Order.Failure", "Faild To Create Order");

            //7-Returns a DTO containing the full order details to the client,
            //including Id[OrderId], UserEmail, items[ProductName, PictureUrl,
            //Price, Quantity], address, delivery method[ShortName], order status,
            //OrderDate, subtotal, and total price

            return _mapper.Map<OrderToReturnDTO>(order);

        }

        public async Task<Result<IEnumerable<DeliveryMethodDTO>>> GetAllDeliveryMethodsAsync()
        {
           var DeliverMethods = await _unitOfWork.GetRepository<DeliveryMethod , int>().GetAllAsync();

            if (DeliverMethods is null)
                return Error.NotFound("DeliveryMethods.NotFound", "Delivery Method Not Found");

            var data = _mapper.Map<IEnumerable<DeliveryMethod> ,IEnumerable<DeliveryMethodDTO>>(DeliverMethods);
            if (data is null) 
                return Error.NotFound("DeliveryMethods.NotFound", "Delivery Method Not Found");

            return Result<IEnumerable<DeliveryMethodDTO>>.Ok(data);
        }

        public async Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrderAsync(string email)
        {
            var OrderSpec = new OrderSpecification(email);  

            var OrderData = await _unitOfWork.GetRepository<Order , Guid>().GetAllAsync(OrderSpec);

            if (!OrderData.Any())
                return Error.NotFound("Orders.NotFound" , $"Orders With Email:{email} Is Not Found");

            var dataAfterMapping = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderToReturnDTO>>(OrderData);

            return Result<IEnumerable<OrderToReturnDTO>>.Ok(dataAfterMapping);
        }

        public async Task<Result<OrderToReturnDTO>> GetSpecificOrder(string email, Guid Id)
        {
            var OrderSpec = new OrderSpecification(email , Id);
            var SpacificOrder = await _unitOfWork.GetRepository<Order , Guid>().GetByIdAsync(OrderSpec);

            if(SpacificOrder is null)
                return Error.NotFound("Order.NotFound", $"Orders With Email:{email} And Id:{Id} Is Not Found");

            var dataAfterMapping = _mapper.Map<Order, OrderToReturnDTO>(SpacificOrder);

            return Result<OrderToReturnDTO>.Ok(dataAfterMapping);

        }

        private  OrderItem CreateOrderItem(BasketItem item, Product product)
        {
           return   new OrderItem()
                    {
                        Price = product.Price,
                        Quantity = item.Quantity,
                    
                        Product = new ProductItemOrdered()
                        {
                            ProductId = product.Id,
                            ProductName = product.Name,
                            ProductUrl = product.PictureUrl
                        }
                     };
        }
    }
}

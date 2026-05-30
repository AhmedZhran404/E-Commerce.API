using ECommerce.Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Spacifications.OrderSpecification
{
    public class OrderWithPaymentIntentSpec:BaseSpacification<Order , Guid>
    {
        public OrderWithPaymentIntentSpec(string paymentIntentId):base(O => O.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}

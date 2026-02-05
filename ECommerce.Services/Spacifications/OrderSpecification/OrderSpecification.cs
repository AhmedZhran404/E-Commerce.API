using ECommerce.Domain.Entities.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Spacifications.OrderSpecification
{
    public class OrderSpecification : BaseSpacification<Order , Guid>
    {

        public OrderSpecification(string email) : base(O => O.UserEmail == email)
        {
            AddInclude(X => X.DeliveryMethod);
            AddInclude(X => X.Items);
            AddOrderByDesc(X => X.OrderDate);
        }

        public OrderSpecification(string email , Guid Id) : base(O => O.UserEmail == email && O.Id == Id)
        {
            AddInclude(X => X.DeliveryMethod);
            AddInclude(X => X.Items);
        }
    }
}

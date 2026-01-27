using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entities.BasketModule
{
    public class CustomerBasket
    {
        public string Id { get; set; } = default!; // Generated From Front End [GUID]

        public ICollection<BasketItem> Items { get; set; } = [];
    }
}

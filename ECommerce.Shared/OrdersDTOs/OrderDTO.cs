using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.OrdersDTOs
{
    public record OrderDTO
    {
        public string BasketId { get; init; } = default!;
        public int DeliveryMethodId { get; init; }

        public AddressDTO Address { get; init; } = default!;


    }
}

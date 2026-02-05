using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.OrdersDTOs
{
    public record DeliveryMethodDTO
    {
        public int Id { get; init; }

        public string shortName { get; init; } = default!;

        public string Description { get; init; } = default!;

        public string DeliveryTime { get; init; } = default!;

        public decimal Price { get; init; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.OrdersDTOs
{
    public record OrderItemDTO
    {
        public string ProductName { get; init; } = default!;

        public string PictureUrl { get; init; } = default!;

        public decimal Price { get; init; }

        public decimal Quantity { get; init; }
    }
}

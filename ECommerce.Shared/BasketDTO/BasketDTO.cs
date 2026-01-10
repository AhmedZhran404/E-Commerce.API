using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.BasketDTO
{
    public record BasketDTO
    (
         string Id,
         ICollection<BasketItemsDTO> Items
    );
   
}

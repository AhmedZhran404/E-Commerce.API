using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Services.Exceptions
{
    public abstract class NotFoundException(string message) : Exception(message)
    {

    }

    public sealed class ProductNotFound(int Id) : NotFoundException($"Product With {Id} Is Not Found")
    {}

    public sealed class BasketNotFound(string Id) : NotFoundException($"Basket With {Id} Is Not Found")
    {}
}

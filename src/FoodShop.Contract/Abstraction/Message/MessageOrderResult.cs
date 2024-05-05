using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Message
{
    public static class MessageOrderResult
    {
        public static string NotFound(Guid Id)
            => $"Order with Id is {Id} not found";
        public static string CreateSuccess(Guid Id)
            => $"Created successfully Order with Id is {Id}";
        public static string DeleteSuccess(Guid Id)
    => $"Deleted successfully Order with Id is {Id}";
    }
}

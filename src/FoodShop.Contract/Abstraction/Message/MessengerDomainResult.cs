using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Message
{
    public class MessengerDomainResult
    {
        public static string NotFound<T>(Guid Id) 
            => $"{nameof(T)} {Id} Not Found";
        public static string AlreadyExists<T>(Guid Id)
            => $"{nameof(T)} {Id} already Exists";
        public static string CreateSuccess<T>()
            => $"New {nameof(T)} Created Successful";
        public static string DeleteSuccess<T>()
            => $"{nameof(T)} Deleted Successful";
    }
}

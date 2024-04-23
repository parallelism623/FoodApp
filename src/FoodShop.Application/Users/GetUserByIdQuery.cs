using FoodShop.Application.Common.DataTransferObjects.Respone.V1;
using FoodShop.Contract.Abstraction.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Users
{
    public record GetUserByIdQuery(Guid Id) : IQuery<UserResponse>
    {
        public string cacheKey => $"api:User:{Id}";
    }
}

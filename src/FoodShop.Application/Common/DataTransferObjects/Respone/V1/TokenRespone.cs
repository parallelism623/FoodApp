using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Respone.V1
{
    public record TokenRespone(string AccessToken, string RefreshToken, DateTime RefreshTokenExpiryTime);
}

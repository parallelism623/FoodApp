using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Message
{
    public static class MessageProductResult
    {
        public static string OutOfStock(string productName)
            => $"Product: '{productName}' đã hết hàng.";
        public static string NotFounde(Guid productId)
            => $"Không tìm thấy sản phẩm với ID '{productId}'.";
        
    }
}

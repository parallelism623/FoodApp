using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Contract.Abstraction.Message
{
    public static class MessageNotification
    {
        #region Categories
        public static string CreateNewCategories(string name)
            => $"Category {name} created successful";
        #endregion Categories
    }
}

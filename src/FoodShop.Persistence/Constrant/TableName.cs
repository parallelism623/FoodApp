using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Persistence.Constrant
{
    internal static class TableName
    {

        #region ExternalEntities
        internal const string Category = nameof(Category);
        internal const string Product = nameof(Product);
        internal const string ProductDetail = nameof(ProductDetail);
        internal const string Order = nameof(Order);
        internal const string OrderDetail = nameof(OrderDetail);
        internal const string History = nameof(History);
        internal const string Customer = nameof(Customer);
        #endregion ExternalEntities

        #region IdentityEntities
        internal const string AppUser = nameof(AppUser);
        internal const string AppRole = nameof(AppRole);
        internal const string Permission = nameof(Permission);
        #endregion IdentitEntities
    }
}

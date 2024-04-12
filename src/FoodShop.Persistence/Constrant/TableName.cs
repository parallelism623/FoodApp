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
        internal const string Order = nameof(Order);
        internal const string OrderProduct = nameof(OrderProduct);
        internal const string History = nameof(History);
        internal const string Cart = nameof(Cart);
        internal const string CartProduct = nameof(CartProduct);

        #endregion ExternalEntities

        #region IdentityEntities
        internal const string AppUserRole = nameof(AppUserRole);
        internal const string AppUser = nameof(AppUser);
        internal const string AppRole = nameof(AppRole);
        internal const string Permission = nameof(Permission);
        internal const string AppUserClaim = nameof(AppUserClaim); // IdentityUserClaim
        internal const string AppRoleClaim = nameof(AppRoleClaim); // IdentityRoleClaim
        internal const string AppUserLogin = nameof(AppUserLogin); // IdentityRoleClaim
        internal const string AppUserToken = nameof(AppUserToken); // IdentityUserToken
        #endregion IdentitEntities
    }
}

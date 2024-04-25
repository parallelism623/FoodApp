using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Request.V1
{
    public class UpdatePermissionRequest
    {
        public Guid RoleId { get; set; }
        public List<string> Permissions { get; set; } = default!;
    }
}

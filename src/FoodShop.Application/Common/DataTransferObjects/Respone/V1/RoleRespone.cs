using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Application.Common.DataTransferObjects.Respone.V1
{
    public class RoleRespone
    {
        public Guid Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public List<string>? Permissions { get; set; }
    }
}

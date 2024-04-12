using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShop.Domain.Exceptions
{
    public sealed class NotFoundException : DomainException
    {
        public NotFoundException(string title) : base(title, "NotFound") { }

    }
}
